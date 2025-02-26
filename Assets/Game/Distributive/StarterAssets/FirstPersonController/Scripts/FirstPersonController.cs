﻿using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;
#endif

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class FirstPersonController : MonoBehaviour
	{
		[Header("Player")]
		public Animator Animator;
        public float targetSpeed;  // расчетная скорость игрока
        [Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 4.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 6.0f;
        [Tooltip("Move and reload speed of the character in m/s")]
        public float MoveReloadSpeed = 2.0f;
        public float MoveSigthSpeed = 2.0f;
        public bool isReloading;
        public bool isSight;
        [Tooltip("Rotation speed X of the character")]
		public float RotationSpeedX = 2.0f;
        [Tooltip("Rotation speed of Y the character")]
        public float RotationSpeedY = 1.0f;
        [Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeightArmed = 1.2f;
        public float JumpHeight = 1.5f;
        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.1f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public bool _isFalling = false;
        public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.5f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = -70.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = 60.0f;

		// cinemachine
		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;


#if ENABLE_INPUT_SYSTEM
		private PlayerInput _playerInput;
#endif
		private CharacterController _controller;
		private StarterAssetsInputs _input;
		private GameObject _mainCamera;
		public SoundController SoundController;
		//public Ui_Control Ui_Control;
        public My_Weapon_Controller My_Weapon_Controller;

        private const float _threshold = 0.01f;

		private bool IsCurrentDeviceMouse
		{
			get
			{
#if ENABLE_INPUT_SYSTEM
				return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
			}
		}

		private void Awake()
		{
			// get a reference to our main camera
			if (_mainCamera == null)
			{
				_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
		}

		private void Start()
		{
			//Отключить и спрятать курсор
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //_animator = GetComponent<Animator>();
            //Animator = GetComponentInChildren<Animator>();
            _controller = GetComponent<CharacterController>();
			_input = GetComponent<StarterAssetsInputs>();
            //SoundController = GetComponentInChildren<SoundController>();

#if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;
		}

		private void Update()
		{
			if (!Animator.GetBool("isDead"))
			{
                JumpAndGravity();
                GroundedCheck();				
                Move();                
            }
            else
            {
				Animator.SetBool("isRun", false);
                Animator.SetBool("isMove", false);
                Animator.SetBool("isJump", false);
                Animator.SetBool("isReloading", false);                
                Animator.SetBool("isSight", false);
            }
		}

		private void LateUpdate()
		{
			if (!My_Weapon_Controller.RadialWeaponMenu)
			CameraRotation();
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
			//if (!Grounded)
			//{
            //    Animator.SetBool("isGround", false);
            //}

			if ((!Grounded) && (!_isFalling))
            {
                // падаем
                
                _isFalling = true;
				My_Weapon_Controller.OnSightOff(null);

                //Ui_Control.Scope_Off();

            }
			else 
			{
				if ((Grounded) && (_isFalling))
				{
                    // упали
                    _isFalling = false;
                    //Запуск звука приземления
                    //Debug.Log("Прыжок закончен!");
                    Animator.SetBool("isJump", false);
                    Animator.SetBool("isGround", true);
                    SoundController.LandingSound();

					if (_input.sight)
					{
                        My_Weapon_Controller.OnSight(null);
                    }


                }

            }
		}

		private void CameraRotation()
		{
			if (!Animator.GetBool("isDead"))// && My_Weapon_Controller.RadialWeaponMenu)
				{
                // if there is an input
                if (_input.look.sqrMagnitude >= _threshold)  // _input.look- последнее перемещение мыши
                                                             //_input.look.sqrMagnitude - длина вектора перемещения
                {                    
                    //Don't multiply mouse input by Time.deltaTime
                    float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                    _cinemachineTargetPitch += _input.look.y * RotationSpeedY * deltaTimeMultiplier; // требуемый угол наклона камеры
                    _rotationVelocity = _input.look.x * RotationSpeedX * deltaTimeMultiplier;

                    Animator.SetFloat("z", _cinemachineTargetPitch / -50f);
                    //if (_cinemachineTargetPitch <= 0)
				 	//{
                    //    Animator.SetFloat("z", _cinemachineTargetPitch / TopClamp);
                    //}
					//else
					//{
                    //    Animator.SetFloat("z", _cinemachineTargetPitch / (-1 * BottomClamp));
                    //}
					

                    //Debug.Log(_cinemachineTargetPitch.ToString());

                    // clamp our pitch rotation
                    _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, TopClamp, BottomClamp);

                    // Update Cinemachine camera target pitch
                    CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

                    // rotate the player left and right
                    transform.Rotate(Vector3.up * _rotationVelocity);  // поворот вправо-влево
                    if (isSight && !Animator.GetBool("isShooting"))
					{
                        My_Weapon_Controller.Sighting();
                        //My_Weapon_Controller.CinemachineCameraTarget.transform.position = My_Weapon_Controller.SightPoint.position;
                    }
                }
            }
			
		}

		private void Move()
		{
			if (_input.move.magnitude > 0f)
			{
                Animator.SetBool("isMove", true);
                if (_input.sprint && !isReloading && !isSight && Grounded && (_input.move.y == 1f))
                {
                    Animator.SetBool("isRun", true);
                }
                else
                {
                    Animator.SetBool("isRun", false);
                }
            }
			else
			{
				Animator.SetBool("isMove", false);
                Animator.SetBool("isRun", false);
            }
			Animator.SetFloat("x", _input.move.x);
			Animator.SetFloat("y", _input.move.y);

			// set target speed based on move speed, sprint speed and if sprint is pressed
			if (!isReloading && !isSight && (_input.move.y == 1f))
			{
                targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
                //targetSpeed = _input.reload ? MoveReloadSpeed : targetSpeed;
            }
            else 
			{   //targetSpeed = MoveReloadSpeed;
                targetSpeed = isReloading ? MoveReloadSpeed : MoveSigthSpeed;
            }            

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}

			// normalise input direction
			Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (_input.move != Vector2.zero)
			{
				// move
				inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
			}

			// move the player
			_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
		}

        
        public void Jump()
		{
            // Jump
			if (Animator.GetBool("isArmed"))
			{
                _verticalVelocity = Mathf.Sqrt(JumpHeightArmed * -2f * Gravity);
            }
			else
			{
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
            }

            
        }

        private void JumpAndGravity()
		{
			if (Grounded)
			{
                Animator.SetBool("isGround", true);
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump Check
				if ((_input.jump && _jumpTimeoutDelta <= 0.0f) && (!Animator.GetBool("isReloading")))
				{
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    //_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                    Animator.SetTrigger("Jump");
                    _input.jump = false;
                }

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
                Animator.SetBool("isGround", false);
                // reset the jump timeout timer               
                _jumpTimeoutDelta = JumpTimeout;
                _input.jump = false;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}

				// if we are not grounded, do not jump
				
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}
	}
}