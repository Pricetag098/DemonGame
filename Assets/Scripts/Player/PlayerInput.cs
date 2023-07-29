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
		[SerializeField] CapsuleCollider standingCollider, crouchedCollider;


		[Header("InputActions")]
		[SerializeField] InputActionProperty moveAction;
		[SerializeField] InputActionProperty sprintAction;
		[SerializeField] InputActionProperty crouchAction;
		[SerializeField] InputActionProperty jumpAction;
		[SerializeField] InputActionProperty mouseAction;
		[SerializeField] InputActionProperty fireAction; //trust me

		[Header("Grounding")]
		public Transform groundingPoint;
		[SerializeField] protected float groundingRadius;
		[SerializeField] protected LayerMask groundingLayer;

		[Header("Walk Settings")]
		[SerializeField] float walkMaxSpeed = 10;
		[SerializeField] float walkAcceleration = 10;
		[SerializeField] float walkSlowForce = 10;
		[SerializeField] float walkControlForce = 10;
		

		[Header("Run Settings")]
		[SerializeField] float runMaxSpeed = 10;
		[SerializeField] float runAcceleration = 10;
		[SerializeField] float runSlowForce = 10;
		[SerializeField] float runControlForce = 10;

		[Header("Crouch Settings")]
		[SerializeField] float crouchMaxSpeed = 10;
		[SerializeField] float crouchAcceleration = 10;
		[SerializeField] float crouchSlowForce = 10;
		[SerializeField] float crouchControlForce = 10;

		[Header("AirControl Settings")]
		[SerializeField] float airMaxSpeed = 10;
		[SerializeField] float airAcceleration = 10;
		[SerializeField] float airSlowForce = 10;
		[SerializeField] float airControlForce = 10;
		[SerializeField] float jumpHeight = 100;

		[Header("Slide Settings")]
		[SerializeField] float slideLaunchVel;
		[SerializeField] float slideGravityModifier = 2;
		[SerializeField] float slideSlowForce = 2;
		[SerializeField] float maxSlideSpeed = float.PositiveInfinity;

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

		bool grounded;

		Vector3 slideEntryVel;
		public enum MoveStates
		{
			walk,
			run,
			crouch,
			slide
		}

		public bool holdToSlide;
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
			fireAction.action.Enable();
		}
		private void OnDisable()
		{
			mouseAction.action.Disable();
			moveAction.action.Disable();
			sprintAction.action.Disable();
			crouchAction.action.Disable();
			jumpAction.action.Disable();
			fireAction.action.Disable();
		}

		void Jump(InputAction.CallbackContext context)
		{
			if (!IsGrounded())
			{
				return;
			}


			if (moveState == MoveStates.slide)
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
				slideInput = false;

			}
			

			float a = (Vector3.Dot(orientation.up,gravityDir) * jumpHeight) / (-0.5f);
			float jumpForce = Mathf.Sqrt(a);
			rb.AddForce(orientation.up *jumpForce,ForceMode.VelocityChange);
		}

		void SetCollider(int i)
		{
			standingCollider.enabled = i == 0;
			crouchedCollider.enabled = i == 1;
			
		}

		bool slideInput;

		// Update is called once per frame
		void Update()
		{
			
			DoCamRot();
			orientation.eulerAngles = new Vector3(orientation.eulerAngles.x, cam.eulerAngles.y, orientation.eulerAngles.z);
			inputDir = moveAction.action.ReadValue<Vector2>();

			camMovementTimer = Mathf.Clamp(camMovementTimer + Time.deltaTime, 0, camMovementTime);

			cam.localPosition = Vector3.LerpUnclamped(lastCamPos, targetCamPos, camMovementEasing.Evaluate(Mathf.Clamp01(camMovementTimer/camMovementTime)));

			
			if (holdToSlide)
			{
				slideInput = crouchAction.action.IsPressed();
			}
			else
			{
				if (crouchAction.action.WasPressedThisFrame())
					slideInput = !slideInput;
			}
			//Different update for each state
			switch (moveState)
			{
				case MoveStates.walk:

					SetCollider(0);
					if (sprintAction.action.IsPressed() && inputDir.y > 0 && !fireAction.action.IsPressed())
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
					if (!sprintAction.action.IsPressed() || inputDir.y <= 0 || fireAction.action.IsPressed())
					{
						moveState = MoveStates.walk;
						lastCamPos = cam.localPosition;
						targetCamPos = camStandingPos;
						camMovementTimer = 0;
						return;
					}
					if (slideInput)
					{
						moveState = MoveStates.slide;
						lastCamPos = cam.localPosition;
						targetCamPos = camCrouchingPos;
						camMovementTimer = 0;
						slideEntryVel = rb.velocity;
						
						
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
					SetCollider(1);
					//Move(crouchMaxSpeed, crouchAcceleration, crouchSlowForce);
					if (!slideInput)
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

		void OnHitGround()
		{
			switch (moveState)
			{
				case MoveStates.slide:
					if (Vector3.Dot(rb.velocity, orientation.forward) < maxSlideSpeed)
					{
						RaycastHit hit;
						Vector3 force = slideLaunchVel * orientation.forward;
						if (Physics.Raycast(orientation.position, -orientation.up, out hit, 5, groundingLayer))
						{
							force = Vector3.ProjectOnPlane(force, hit.normal);
						}
						rb.AddForce(force, ForceMode.VelocityChange);
					}
					break;
				default:
					//do some animation stuff
					break;
			}
		}
		private void FixedUpdate()
		{
			bool groundCheck = IsGrounded();
			if (groundCheck && !grounded)
				OnHitGround();
			grounded = groundCheck;


			if (!grounded)
			{
				Move(airMaxSpeed, airAcceleration, airSlowForce, airControlForce);
			}
			else
			{
				switch (moveState)
				{
					case MoveStates.walk:
						Move(walkMaxSpeed, walkAcceleration, walkSlowForce, walkControlForce);
						break;
					case MoveStates.run:
						Move(runMaxSpeed, runAcceleration, runSlowForce, runControlForce);
						break;
					case MoveStates.crouch:
						Move(crouchMaxSpeed, crouchAcceleration, crouchSlowForce, crouchControlForce);
						break;
					case MoveStates.slide:
						//Move(crouchMaxSpeed, crouchAcceleration, crouchSlowForce);
						if (grounded)
						{
							rb.AddForce(gravityDir * slideGravityModifier, ForceMode.Acceleration);
						}
						else
						{
							rb.AddForce(gravityDir, ForceMode.Acceleration);
						}
						rb.AddForce(-rb.velocity.normalized * slideSlowForce * Time.fixedDeltaTime,ForceMode.Acceleration);
						if(rb.velocity.magnitude < crouchMaxSpeed || Vector3.Dot(rb.velocity,slideEntryVel) < 0)
						{
							moveState = MoveStates.crouch;
							lastCamPos = cam.localPosition;
							targetCamPos = camCrouchingPos;
							camMovementTimer = 0;
							slideInput = false;
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

		


		void Move(float maxSpeed,float acceleration,float slowForce,float controlForce)
		{

			acceleration *= Time.fixedDeltaTime;
			slowForce *= Time.fixedDeltaTime;
			controlForce *= Time.fixedDeltaTime;
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
				Vector3 forceDir = orientation.right * inputDir.x * acceleration * playerStats.accelerationMulti;
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
			if(rb.velocity.magnitude > maxSpeed)
			{
				rb.AddForce(-rb.velocity.normalized * controlForce);
			}
			
		}

		bool CanStopCrouch()
		{
			return !Physics.Raycast(transform.position, orientation.up, headCheckDistance, groundingLayer);
		}

		bool IsGrounded()
		{
			return Physics.CheckSphere(groundingPoint.position, groundingRadius, groundingLayer);
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(groundingPoint.position, groundingRadius);
		}
	}
}

