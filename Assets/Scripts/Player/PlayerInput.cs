using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
	public class PlayerInput : MonoBehaviour
	{
		[Header("Set this stuff pls")]
		public Transform orientation;
		public Transform cam;
		public Vector3 gravityDir;
		public float sense;
		public MoveStates moveState;
		Rigidbody rb;
		[SerializeField] CapsuleCollider standingCollider, crouchedCollider, slideCollider;


		[Header("InputActions")]
		[SerializeField] InputActionProperty moveAction;
		[SerializeField] InputActionProperty sprintAction;
		[SerializeField] InputActionProperty crouchAction;
		[SerializeField] InputActionProperty jumpAction;
		[SerializeField] InputActionProperty mouseAction;

		[Header("Grounding")]
		public Transform groundingPoint;
		[SerializeField] protected float groundingRadius;
		[SerializeField] protected LayerMask groundingLayer;

		[Header("Walk Settings")]
		[SerializeField] float walkMaxSpeed = 10;
		[SerializeField] float walkAcceleration = 10;
		[SerializeField] float walkSlowForce = 10;
		

		[Header("Run Settings")]
		[SerializeField] float runMaxSpeed = 10;
		[SerializeField] float runAcceleration = 10;
		[SerializeField] float runSlowForce = 10;

		[Header("Crouch Settings")]
		[SerializeField] float crouchMaxSpeed = 10;
		[SerializeField] float crouchAcceleration = 10;
		[SerializeField] float crouchSlowForce = 10;

		[Header("AirControl Settings")]
		[SerializeField] float airMaxSpeed = 10;
		[SerializeField] float airAcceleration = 10;
		[SerializeField] float airSlowForce = 10;
		[SerializeField] float jumpForce = 100;

		[Header("CameraFollowSettings")]
		[SerializeField] AnimationCurve camMovementEasing = AnimationCurve.Linear(0,0,1,1);
		[SerializeField] Vector3 camStandingPos;
		[SerializeField] Vector3 camCrouchingPos;
		[SerializeField] float camMovementTime;
		[SerializeField] float headCheckDistance = .5f;
		float camMovementTimer;
		Vector3 lastCamPos;
		Vector3 targetCamPos;

		float camRotX = 0;
		Vector2 inputDir;
		PlayerStats playerStats;
		public enum MoveStates
		{
			walk,
			run,
			crouch,
			slide
		}
		
		// Start is called before the first frame update
		void Start()
		{
			lastCamPos = camStandingPos;
			targetCamPos = camStandingPos;
			jumpAction.action.performed += Jump;
			Cursor.lockState = CursorLockMode.Locked;
			rb = GetComponent<Rigidbody>();
			playerStats = GetComponent<PlayerStats>();

		}

		//manage all the input actions
		private void OnEnable()
		{
			mouseAction.action.Enable();
			moveAction.action.Enable();
			sprintAction.action.Enable();
			crouchAction.action.Enable();
			jumpAction.action.Enable();
		}
		private void OnDisable()
		{
			mouseAction.action.Disable();
			moveAction.action.Disable();
			sprintAction.action.Disable();
			crouchAction.action.Disable();
			jumpAction.action.Disable();
		}

		void Jump(InputAction.CallbackContext context)
		{
			if (!IsGrounded())
			{
				return;
			}
			rb.AddForce(orientation.up *jumpForce);
		}

		void SetCollider(int i)
		{
			standingCollider.enabled = i == 0;
			crouchedCollider.enabled = i == 1;
			slideCollider.enabled = i == 2;
		}

		// Update is called once per frame
		void Update()
		{
			
			DoCamRot();
			orientation.eulerAngles = new Vector3(orientation.eulerAngles.x, cam.eulerAngles.y, orientation.eulerAngles.z);
			inputDir = moveAction.action.ReadValue<Vector2>();

			camMovementTimer = Mathf.Clamp(camMovementTimer + Time.deltaTime, 0, camMovementTime);

			cam.localPosition = Vector3.LerpUnclamped(lastCamPos, targetCamPos, camMovementEasing.Evaluate(Mathf.Clamp01(camMovementTimer/camMovementTime)));
			//Different update for each state
			switch (moveState)
			{
				case MoveStates.walk:

					SetCollider(0);
					if (sprintAction.action.IsPressed() && inputDir.y > 0)
					{
						moveState = MoveStates.run;
						lastCamPos = cam.localPosition;
						targetCamPos = camStandingPos;
						camMovementTimer = 0;

						return;
					}
					if (crouchAction.action.IsPressed())
					{
						moveState = MoveStates.crouch;
						lastCamPos = cam.localPosition;
						targetCamPos = camCrouchingPos;
						camMovementTimer = 0;
						return;
					}

					break;
				case MoveStates.run:
					SetCollider(0);
					if (!sprintAction.action.IsPressed() || inputDir.y <= 0)
					{
						moveState = MoveStates.walk;
						lastCamPos = cam.localPosition;
						targetCamPos = camStandingPos;
						camMovementTimer = 0;
						return;
					}
					if (crouchAction.action.IsPressed())
					{
						moveState = MoveStates.slide;
						lastCamPos = cam.localPosition;
						targetCamPos = camCrouchingPos;
						camMovementTimer = 0;
						return;
					}
					break;
				case MoveStates.crouch:
					SetCollider(1);
					if (!crouchAction.action.IsPressed() && CanStopCrouch())
					{
						moveState = MoveStates.walk;
						lastCamPos = cam.localPosition;
						targetCamPos = camStandingPos;
						camMovementTimer = 0;
						return;
					}
					break;
				case MoveStates.slide:
					SetCollider(2);
					//Move(crouchMaxSpeed, crouchAcceleration, crouchSlowForce);
					if (!crouchAction.action.IsPressed())
					{
						if (CanStopCrouch())
						{
							moveState = MoveStates.run;
							lastCamPos = cam.localPosition;
							targetCamPos = camStandingPos;
							camMovementTimer = 0;
						}
						else
						{
							moveState = MoveStates.crouch;
							lastCamPos = cam.localPosition;
							targetCamPos = camCrouchingPos;
							camMovementTimer = 0;
						}
						
						return;
					}
					break;
			}

		}


		private void FixedUpdate()
		{
			if (!IsGrounded())
			{
				Move(airMaxSpeed, airAcceleration, airSlowForce);
			}
			else
			{
				switch (moveState)
				{
					case MoveStates.walk:
						Move(walkMaxSpeed, walkAcceleration, walkSlowForce);
						break;
					case MoveStates.run:
						Move(runMaxSpeed, runAcceleration, runSlowForce);
						break;
					case MoveStates.crouch:
						Move(crouchMaxSpeed, crouchAcceleration, crouchSlowForce);
						break;
					case MoveStates.slide:
						//Move(crouchMaxSpeed, crouchAcceleration, crouchSlowForce);
						if (IsGrounded())
						{
							rb.AddForce(gravityDir * 2);
						}
						else
						{
							rb.AddForce(gravityDir);
						}
						break;

				}
			}

			
		}

		

		void DoCamRot()
		{
			Vector2 camDir = mouseAction.action.ReadValue<Vector2>();
			camRotX = Mathf.Clamp(-camDir.y * sense * Time.deltaTime + camRotX, -90, 90);
			cam.rotation = Quaternion.Euler(camRotX, cam.rotation.eulerAngles.y + camDir.x * sense * Time.deltaTime, cam.rotation.eulerAngles.z);
		}

		


		void Move(float maxSpeed,float acceleration,float slowForce)
		{
			Vector3 force = Vector3.zero;

			Vector3 playerVel = rb.velocity;

			//Project player velocity to relative vectors on the player
			//basicaly calculating what the velocity is in the players forward/backward and left/right direction
			float fwVel = Vector3.Dot(playerVel, orientation.forward * Mathf.Sign(inputDir.y));
			float rightVel = Vector3.Dot(playerVel, orientation.right * Mathf.Sign(inputDir.x));
			

			if (fwVel < maxSpeed * playerStats.speedMulti * Mathf.Abs(inputDir.y))
			{
				Vector3 forceDir = orientation.forward * inputDir.y * acceleration * playerStats.accelerationMulti;

				if (Mathf.Sign(fwVel) < 0)
				{
					forceDir *= 2;
				}
				force += forceDir;
			}

			if (rightVel < maxSpeed * playerStats.speedMulti * Mathf.Abs(inputDir.x))
			{
				Vector3 forceDir = orientation.right * inputDir.x * acceleration;
				if (Mathf.Sign(rightVel) < 0)
				{
					forceDir *= 2;
				}
				force += forceDir;
			}

			//reduce velocity in directions were not moving
			if (inputDir.y == 0)
				force += slowForce * Vector3.Dot(playerVel, orientation.forward) * -orientation.forward;
			if (inputDir.x == 0)
				force += slowForce * Vector3.Dot(playerVel, orientation.right) * -orientation.right;
			if (!IsGrounded())
			{
				rb.AddForce(gravityDir);
			}
			

			
			//project the forward velocity onto the floor for walking on slopes
			RaycastHit hit;
			if (Physics.Raycast(orientation.position, -orientation.up, out hit, 5, groundingLayer))
			{
				force = Vector3.ProjectOnPlane(force, hit.normal);
			}

			rb.AddForce(force);

			
		}

		bool CanStopCrouch()
		{
			return !Physics.Raycast(transform.position, orientation.up, headCheckDistance, groundingLayer);
		}

		bool IsGrounded()
		{
			return Physics.CheckSphere(groundingPoint.position, groundingRadius, groundingLayer);
		}
	}
}

