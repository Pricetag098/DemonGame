using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
	public class PlayerInputt : MonoBehaviour
	{
		[Header("Set this stuff pls")]
		public Transform orientation;
		public Transform cam,aimAssistGo,camGo;
		public Vector3 gravityDir;
		public float sensitivity =1;
		public float minSurface = .5f;
		[SerializeField] AimAssist aimAssist;
		public MoveStates moveState;
		[HideInInspector] public Rigidbody rb;
		[SerializeField] CapsuleCollider standingCollider, crouchedCollider;

		public bool canSprintAndShoot = false;
		[Header("InputActions")]
		[SerializeField] InputActionProperty moveAction;
		[SerializeField] InputActionProperty sprintAction;
		[SerializeField] InputActionProperty crouchAction;
		[SerializeField] InputActionProperty jumpAction;
		[SerializeField] InputActionProperty mouseAction;
		[SerializeField] InputActionProperty fireAction;
        [SerializeField] InputActionProperty castAction; //trust me

        [Header("Grounding")]
		public Transform groundingPoint;
		[SerializeField] protected float groundingRadius;
		[SerializeField] protected LayerMask groundingLayer;


		[SerializeField] MovementData walkData,runData,crouchData,airData;
		//[Header("Walk Settings")]
		//[SerializeField] float walkMaxSpeed = 10;
		//[SerializeField] float walkAcceleration = 10;
		//[SerializeField] float walkSlowForce = 10;
		//[SerializeField] float walkControlForce = 10;
		//[SerializeField] float walkOppositeVelMulti = 2;
		

		//[Header("Run Settings")]
		//[SerializeField] float runMaxSpeed = 10;
		//[SerializeField] float runAcceleration = 10;
		//[SerializeField] float runSlowForce = 10;
		//[SerializeField] float runControlForce = 10;
		//[SerializeField] float runOppositeVelMulti = 2;

		//[Header("Crouch Settings")]
		//[SerializeField] float crouchMaxSpeed = 10;
		//[SerializeField] float crouchAcceleration = 10;
		//[SerializeField] float crouchSlowForce = 10;
		//[SerializeField] float crouchControlForce = 10;
		//[SerializeField] float crouchOppositeVelMulti = 2;

		//[Header("AirControl Settings")]
		//[SerializeField] float airMaxSpeed = 10;
		//[SerializeField] float airAcceleration = 10;
		//[SerializeField] float airSlowForce = 10;
		//[SerializeField] float airControlForce = 10;
		[SerializeField] float jumpHeight = 100;
		[SerializeField] float jumpForwadBoost = 2;
        [SerializeField] float onHitSlowTime = 1;
		//[SerializeField] float airOppositeVelMulti = 2;

		bool hasBeenHit;
		float onHitTimer;

        [Header("Slide Settings")]
		[SerializeField] float slideLaunchVel;
        [SerializeField] float jumpSlideLaunchVel;
        [SerializeField] float slideGravityModifier = 2;
		[SerializeField] float slideSlowForce = 2;
		[SerializeField] float maxSlideSpeed = float.PositiveInfinity;
		[SerializeField] float slideMinVel = 2;
		[SerializeField] float slideHorizontalAcceleration;
		[SerializeField] float minSpeedToSlide;
		[SerializeField] float slideJumpCounterTime = 0.1f;
        [SerializeField] float slideDelayTime = 0.1f;


        float slideTimer;
        float jumpTimer;
		bool jumped;

        [Header("CameraFollowSettings")]
		[SerializeField] AnimationCurve camMovementEasing = AnimationCurve.Linear(0,0,1,1);
		[SerializeField] Vector3 camStandingPos;
		[SerializeField] Vector3 camCrouchingPos;
		[SerializeField] float camMovementTime;
		[SerializeField] float headCheckDistance = .5f;
		float camMovementTimer;
		Vector3 lastCamPos;
		Vector3 targetCamPos;

		public Vector2 recoilVal;
		float camRotX = 0;
		Vector2 inputDir;
		PlayerStats playerStats;

		[HideInInspector] public bool grounded;
		[HideInInspector] public bool touchingSurface;
		[HideInInspector] public RaycastHit lastSurface;
		 public SurfaceData lastSurfaceData;
		[SerializeField] float surfaceCheckRange;

		Vector3 slideEntryVel;
		Vector3 surfaceNormal;

		PlayerDeath playerDeath;

		public bool tabing = false;

		public enum MoveStates
		{
			walk,
			run,
			crouch,
			slide
		}


		public void SetRecoil(Vector3 recoil)
		{
			if (!playerDeath.dead)
			{
                recoilVal.x = recoil.x;
                recoilVal.y = recoil.y;
            }
        }

        public void IDied()
		{
            recoilVal.x = 0f;
            recoilVal.y = 0f;

            hasBeenHit = false;


        }
		public bool toggleSlide;
		public bool toggleSprint;

		Holster holster;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }
        // Start is called before the first frame update
        void Start()
		{
			lastCamPos = camStandingPos;
			targetCamPos = camStandingPos;
			jumpAction.action.performed += Jump;
			Cursor.lockState = CursorLockMode.Locked;
			playerStats = GetComponent<PlayerStats>();
			camRotX = 0;

			playerDeath = FindObjectOfType<PlayerDeath>();

			holster = FindObjectOfType<Holster>();
		}
		private void OnDestroy()
		{
            jumpAction.action.performed -= Jump;
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
			castAction.action.Enable();
		}
		private void OnDisable()
		{
			mouseAction.action.Disable();
			moveAction.action.Disable();
			sprintAction.action.Disable();
			crouchAction.action.Disable();
			jumpAction.action.Disable();
			fireAction.action.Disable();
			castAction.action.Disable();
		}

		void Jump(InputAction.CallbackContext context)
		{
			if (!IsGrounded())
			{
				return;
			}

			jumpTimer = 0f;

			jumped = true;

            if (moveState == MoveStates.slide)
			{
				if (CanStopCrouch())
				{
					moveState = MoveStates.run;
					lastCamPos = cam.localPosition;
					targetCamPos = camStandingPos;
					camMovementTimer = 0;

					if(slideTimer > slideJumpCounterTime)
					{
                        Vector3 force = jumpForwadBoost * orientation.forward;
                        rb.AddForce(force, ForceMode.VelocityChange);
                        slideTimer = 0;
                    }
                    else
					{
                        Vector3 force = -jumpSlideLaunchVel * orientation.forward;
                        rb.AddForce(force, ForceMode.VelocityChange);
                    }
					
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
		bool sprintInput;


		public bool Running()
		{
			return moveState == MoveStates.run && !canSprintAndShoot;
		}

		public void GotHit()
		{
			hasBeenHit = true;
			onHitTimer = 0f;

			if(moveState == MoveStates.slide)
			{
                Vector3 force = -jumpSlideLaunchVel * orientation.forward;
                rb.AddForce(force, ForceMode.VelocityChange);

                sprintInput = false;
                moveState = MoveStates.crouch;
                lastCamPos = cam.localPosition;
                targetCamPos = camCrouchingPos;
                camMovementTimer = 0;
            }
		}

		// Update is called once per frame
		void Update()
		{
			if(hasBeenHit)
			{
				onHitTimer += Time.deltaTime;
				if (onHitTimer > onHitSlowTime) 
				{
					hasBeenHit = false;
				}
			}


			if(moveState == MoveStates.slide)
			{
				slideTimer += Time.deltaTime;
			}

			if (jumped)
			{
				jumpTimer += Time.deltaTime;
			}
			
			DoCamRot();
			orientation.eulerAngles = new Vector3(orientation.eulerAngles.x, cam.eulerAngles.y, orientation.eulerAngles.z);
			inputDir = moveAction.action.ReadValue<Vector2>();
			if(grounded)
			camMovementTimer = Mathf.Clamp(camMovementTimer + Time.deltaTime, 0, camMovementTime);

			cam.localPosition = Vector3.LerpUnclamped(lastCamPos, targetCamPos, camMovementEasing.Evaluate(Mathf.Clamp01(camMovementTimer/camMovementTime)));

			
			if (!toggleSlide)
			{
				slideInput = crouchAction.action.IsPressed();
			}
			else
			{
				if (crouchAction.action.WasPressedThisFrame())
					slideInput = !slideInput;
			}
			if (!toggleSprint)
			{
				sprintInput = sprintAction.action.IsPressed();
			}
			else
			{
				if (sprintAction.action.WasPressedThisFrame())
					sprintInput = !sprintInput;
			}
			//Different update for each state
			switch (moveState)
			{
				case MoveStates.walk:

					SetCollider(0);
					if (sprintInput && inputDir.y > 0 && ((!fireAction.action.IsPressed() || !castAction.action.IsPressed() || holster.HeldGun.gunState != Gun.GunStates.reloading) || playerDeath.dead) && !hasBeenHit)
					{
						moveState = MoveStates.run;
						lastCamPos = cam.localPosition;
						targetCamPos = camStandingPos;
						camMovementTimer = 0;
						return;
					}
					if (crouchAction.action.IsPressed() && grounded)
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
					if (!sprintInput || inputDir.y <= 0 || (fireAction.action.IsPressed() && !canSprintAndShoot && !playerDeath.dead) || (castAction.action.IsPressed() && !canSprintAndShoot && !playerDeath.dead) || (holster.HeldGun.gunState == Gun.GunStates.reloading && !playerDeath.dead) || hasBeenHit)
					{
						sprintInput = false;
						moveState = MoveStates.walk;
						lastCamPos = cam.localPosition;
						targetCamPos = camStandingPos;
						camMovementTimer = 0;
                        return;
					}
					if (slideInput)
					{
						if(rb.velocity.magnitude >= minSpeedToSlide || !hasBeenHit && slideTimer > slideDelayTime)
						{
                            sprintInput = false;
                            moveState = MoveStates.slide;
                            slideTimer = 0;
                            lastCamPos = cam.localPosition;
                            targetCamPos = camCrouchingPos;
                            camMovementTimer = 0;
                            slideEntryVel = rb.velocity;
                            if (Vector3.Dot(rb.velocity, orientation.forward) < maxSlideSpeed && grounded)
                            {
								if(jumpTimer > slideJumpCounterTime)
								{
                                    RaycastHit hit;
                                    Vector3 force = slideLaunchVel * orientation.forward;
                                    if (Physics.Raycast(orientation.position, -orientation.up, out hit, 5, groundingLayer))
                                    {
                                        force = Vector3.ProjectOnPlane(force, hit.normal);
                                    }
                                    rb.AddForce(force, ForceMode.VelocityChange);
                                }
                            }
                        }
						else
						{
                            sprintInput = false;
                            moveState = MoveStates.crouch;
                            lastCamPos = cam.localPosition;
                            targetCamPos = camCrouchingPos;
                            camMovementTimer = 0;
                        }


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
							moveState = MoveStates.walk;
							lastCamPos = cam.localPosition;
							targetCamPos = camStandingPos;
							camMovementTimer = 0;


						}
						//else
						//{
							
						//	moveState = MoveStates.crouch;
						//	lastCamPos = cam.localPosition;
						//	targetCamPos = camCrouchingPos;
						//	camMovementTimer = 0;
						//}
						
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
						Vector3 force = jumpSlideLaunchVel * orientation.forward;
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

			//Debug.Log("Grounded " + grounded);
			//Debug.Log("onSurface" + touchingSurface);
			if (!grounded)
			{
				Move(airData);
			}
			else
			{
				switch (moveState)
				{

					case MoveStates.walk:
						Move(walkData);
                        break;
					case MoveStates.run:
						Move(runData);
                        break;
					case MoveStates.crouch:
						Move(crouchData);
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
						if(rb.velocity.magnitude < slideMinVel)
						{
							moveState = MoveStates.crouch;
							lastCamPos = cam.localPosition;
							targetCamPos = camCrouchingPos;
							slideInput = false;
							camMovementTimer = 0;
							
						}
						if (Vector3.Dot(orientation.right * inputDir.x, rb.velocity) < maxSlideSpeed * inputDir.x)
						{
							rb.AddForce(orientation.right * (slideHorizontalAcceleration));
						}
						rb.AddForce(-rb.velocity * slideSlowForce, ForceMode.Acceleration);


						break;

				}
			}

			
		}



		void DoCamRot()
		{
            if (!tabing)
            {
                Vector2 camDir = mouseAction.action.ReadValue<Vector2>();

                camRotX = Mathf.Clamp(-camDir.y * sensitivity * Time.deltaTime + camRotX + recoilVal.x, -90, 90);



                camGo.rotation = Quaternion.Euler(camRotX, camGo.rotation.eulerAngles.y + camDir.x * sensitivity * Time.deltaTime + recoilVal.y, camGo.rotation.eulerAngles.z);

				cam.rotation = camGo.rotation;//.forward = aimAssist.GetAssistedAimDir(camGo.forward, cam.position, float.PositiveInfinity, new List<Health>());
            }
        }
		[System.Serializable]
		class MovementData 
		{
			public float speed;
			public float accleration;
		}

		void Move(MovementData data)
		{
			Vector3 idealVel = Vector3.ProjectOnPlane((orientation.forward * inputDir.y + orientation.right * inputDir.x) * data.speed * playerStats.speedMulti * lastSurfaceData.speedModifier, surfaceNormal );
			Vector3 vel = rb.velocity;
			Vector3 turningForce = idealVel - vel;
			rb.AddForce(turningForce * data.accleration * playerStats.accelerationMulti);

            if (!touchingSurface)
			{
				rb.AddForce(gravityDir);
			}

			if (data == runData)
			{
                //holster.SetSprint(true);
            }
			else
			{
                //holster.SetSprint(false);
            }
        }



        //void Move(float maxSpeed,float acceleration,float slowForce,float controlForce,float oppositeVelMulti)
        //{

        //	acceleration *= Time.fixedDeltaTime;
        //	slowForce *= Time.fixedDeltaTime;
        //	controlForce *= Time.fixedDeltaTime;
        //	Vector3 force = Vector3.zero;

        //	Vector3 playerVel = rb.velocity;

        //	//Project player velocity to relative vectors on the player
        //	//basicaly calculating what the velocity is in the players forward/backward and left/right direction
        //	float fwVel = Vector3.Dot(playerVel, orientation.forward * Mathf.Sign(inputDir.y));
        //	float rightVel = Vector3.Dot(playerVel, orientation.right * Mathf.Sign(inputDir.x));


        //	if (fwVel < maxSpeed * playerStats.speedMulti * Mathf.Abs(inputDir.y))
        //	{
        //		Vector3 forceDir = orientation.forward * inputDir.y * acceleration * playerStats.accelerationMulti;

        //		if (Mathf.Sign(fwVel) < 0)
        //		{
        //			forceDir *= oppositeVelMulti;
        //		}
        //		force += forceDir;
        //	}

        //	if (rightVel < maxSpeed * playerStats.speedMulti * Mathf.Abs(inputDir.x))
        //	{
        //		Vector3 forceDir = orientation.right * inputDir.x * acceleration * playerStats.accelerationMulti;
        //		if (Mathf.Sign(rightVel) < 0)
        //		{
        //			forceDir *= oppositeVelMulti;
        //		}
        //		force += forceDir;
        //	}

        //	//reduce velocity in directions were not moving
        //	if (inputDir.y == 0)
        //		force += slowForce * Vector3.Dot(playerVel, orientation.forward) * -orientation.forward;
        //	if (inputDir.x == 0)
        //		force += slowForce * Vector3.Dot(playerVel, orientation.right) * -orientation.right;
        //	if (!touchingSurface)
        //	{
        //		rb.AddForce(gravityDir);
        //	}



        //	//project the forward velocity onto the floor for walking on slopes
        //	force = Vector3.ProjectOnPlane(force, surfaceNormal);

        //	rb.AddForce(force);
        //	if(rb.velocity.magnitude > maxSpeed)
        //	{
        //		rb.AddForce(-rb.velocity.normalized * controlForce);
        //	}

        //}

        bool CanStopCrouch()
		{
			return !Physics.Raycast(transform.position, orientation.up, headCheckDistance, groundingLayer);
		}

		bool IsGrounded()
		{
			RaycastHit hit;
			bool isGrounded = Physics.CheckSphere(groundingPoint.position, groundingRadius, groundingLayer) && Vector3.Dot(surfaceNormal, orientation.up) > minSurface;
			
			if (Physics.SphereCast(orientation.position,groundingRadius, -orientation.up, out hit, surfaceCheckRange * 5, groundingLayer))
			{
				surfaceNormal = hit.normal;
				touchingSurface = hit.distance <= surfaceCheckRange;
				lastSurface = hit;
				
				if (lastSurface.collider.TryGetComponent(out Surface s))
				{
					lastSurfaceData = s.data;
				}
				else
				{
					lastSurfaceData = VfxSpawner.DefaultSurfaceData;
				}
			}
			else
			{
				touchingSurface = false;
			}
			return isGrounded;
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(groundingPoint.position, groundingRadius);
			Gizmos.color = Color.magenta;
			Gizmos.DrawLine(orientation.position,orientation.position + -orientation.up * surfaceCheckRange);
		}

		
	}
}

