using Cinemachine;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static UnityEditor.Experimental.GraphView.GraphView;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        public static ThirdPersonController instance;
        #region public field

        [Header("Player")]
        public float MoveSpeed = 2.0f;

        public float SprintSpeed = 5.335f;

        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        public float JumpHeight = 1.2f;

        public float Gravity = -15.0f;

        [Space(10)]
        public float JumpTimeout = 0.50f;

        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        public bool Grounded = true;

        public float GroundedOffset = -0.14f;

        public float GroundedRadius = 0.28f;

        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        public CinemachineVirtualCamera CinemachineCamera;
        public CinemachineVirtualCameraBase _Rig;
        public GameObject CinemachineCameraTarget;

        [Space(10)]
        public float _LocoDis = 2;
        public float _ZoomDis = 2;
        public float _CenterDis = 4;
        public Vector3 LocoOffset = new Vector3(4.2f, 0, 0.8f);
        public Vector3 ZoomOffset = new Vector3(4.2f, 0, 0.8f);
        public Vector3 CenterOffset = Vector3.zero;

        public float TopClamp = 70.0f;

        public float BottomClamp = -30.0f;

        public float CameraAngleOverride = 0.0f;

        public bool LockCameraPosition = false;

        [Header("UI")]
        public GameObject CrossHair;

        #endregion

        #region private field
        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;
        private float _cinemachineFOV;
        private Cinemachine3rdPersonFollow thirdFollow;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
        private float _Locomotion;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;
        private int _animIDIsLocomotion;
        private int _animIDHorDir;
        private int _animIDVerDir;
        private int _animIDHotMode;
        private int _animIDPlayGun;

        // SmoothNess
        private float smoothness = 0.3f; // ความนุ่มนวล
        private float currentVelocity; // ความเร็วปัจจุบัน
        private float currentSholderoffsetX;
        private float currentSholderoffsetY;
        private float currentDistance;
        private float _SmoothSholderOffset = 0.07f;

        private int _HotMode;
        

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

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

        #endregion
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();

#if ENABLE_INPUT_SYSTEM 
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif
            AssignAnimationIDs();
            
            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;

            _cinemachineFOV = CinemachineCamera.m_Lens.FieldOfView;

            if (CinemachineCamera != null)
            {
                thirdFollow = CinemachineCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            }
            else
            {
                Debug.Log("CinemachineVirtualCamera not found.");
            }
            CrossHair.SetActive(false);
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);
            _Locomotion = _input.LockLocomotion ? 1.2f : 1.1f;
            _HotMode = HotbarScroll.instance != null ? HotbarScroll.instance.CurrentTag : 0;
            _animator.SetFloat(_animIDHotMode, _HotMode);

            if (_HotMode == 3)
            {
                SetWeightLayer(1, 1);
            }
            else if (_HotMode == 1)
            {
                SetWeightLayer(2, 0);
            }
            else
            {
                SetWeightLayer(1, 0);
                SetWeightLayer(2, 0);
            }

            JumpAndGravity();
            GroundedCheck();
            Moveing();
        }

        private void LocoCamera()
        {
            if (_input.LockLocomotion && _HotMode == 3)
            {
                thirdFollow.ShoulderOffset.x = CameraOffset
                (thirdFollow.ShoulderOffset.x, ZoomOffset.x, currentSholderoffsetX);

                thirdFollow.ShoulderOffset.y = CameraOffset
                (thirdFollow.ShoulderOffset.y, ZoomOffset.y, currentSholderoffsetY);

                thirdFollow.CameraDistance = CameraOffset
                (thirdFollow.CameraDistance, _ZoomDis, currentDistance);
            }
            else
            {
                thirdFollow.ShoulderOffset.x = CameraOffset
                (thirdFollow.ShoulderOffset.x, LocoOffset.x, currentSholderoffsetX);

                thirdFollow.ShoulderOffset.y = CameraOffset
                (thirdFollow.ShoulderOffset.y, LocoOffset.y, currentSholderoffsetY);

                thirdFollow.CameraDistance = CameraOffset
                (thirdFollow.CameraDistance, _LocoDis, currentDistance);
            }
            CrossHair.SetActive(true);
        }
        private void CenterCamera()
        {
            thirdFollow.ShoulderOffset.x = CameraOffset
            (thirdFollow.ShoulderOffset.x, CenterOffset.x, currentSholderoffsetX);

            thirdFollow.CameraDistance = CameraOffset
            (thirdFollow.CameraDistance, _CenterDis, currentDistance);

            CrossHair.SetActive(false);
        }

        private float CameraOffset(float CameraOffset ,float ToOffset, float CurrentSholderOffset)
        {
            return Mathf.SmoothDamp(CameraOffset, ToOffset, ref CurrentSholderOffset, _SmoothSholderOffset);
        }
        private void LateUpdate()
        {
            CameraRotation();
        }
        #region Start
        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
            _animIDIsLocomotion = Animator.StringToHash("IsLocomotion");
            _animIDHorDir = Animator.StringToHash("HorDir");
            _animIDVerDir = Animator.StringToHash("VerDir");
            _animIDHotMode = Animator.StringToHash("CurrentMode");
            _animIDPlayGun = Animator.StringToHash("PlayGun");
        }
        #endregion

        #region LateUpdate
        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }
        private void AutoFOV(float speed)
        {
            if (_input.LockLocomotion && _HotMode == 3)
            {
                _cinemachineFOV = Mathf.SmoothDamp(_cinemachineFOV, 15, ref currentVelocity, smoothness);
            }else
            {
                switch (speed)
                {
                    case < 4.5f:
                        _cinemachineFOV = Mathf.SmoothDamp(_cinemachineFOV, 40, ref currentVelocity, smoothness);
                        break;
                    case > 4.5f:
                        if (_input.sprint) _cinemachineFOV = Mathf.SmoothDamp(_cinemachineFOV, 60, ref currentVelocity, smoothness);
                        break;
                }
            }
            
            CinemachineCamera.m_Lens.FieldOfView = _cinemachineFOV;
            _animator.SetFloat(_animIDIsLocomotion, _Locomotion);
        }
        #endregion
        
        #region Update
        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }
        private void Moveing()
        {
            switch (_HotMode)
            {
                case 0:
                    Move_forward();
                    CenterCamera();
                    break;

                case 1:
                    Move_Locomotion();
                    CenterCamera();
                    break;

                case 2:
                    Move_Locomotion();
                    LocoCamera();
                    break;

                case 3:
                    Move_Locomotion();
                    LocoCamera();
                    break;
            }
        }
        private void Move_forward()
        {
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            if (_input.move == Vector2.zero) targetSpeed = 0.0f;
            
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            AutoFOV(_animationBlend);
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }
        private void Move_Locomotion()
        {
            float targetSpeed = SpeedControl();

            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            ////////////////////////////////////////////

            if (_input.move == new Vector2(1, 1) || _input.move == new Vector2(-1, 1))
            {
                Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

                if (_input.move != Vector2.zero)
                {
                    _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                      _mainCamera.transform.eulerAngles.y;
                    float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                        RotationSmoothTime);

                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                }

                Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

                // move the player
                _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                                 new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            }

            else if (_input.move == new Vector2(1, -1) || _input.move == new Vector2(-1, -1))
            {
                Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

                if (_input.move != Vector2.zero)
                {
                    _targetRotation = Mathf.Atan2(-inputDirection.x, -inputDirection.z) * Mathf.Rad2Deg +
                                      _mainCamera.transform.eulerAngles.y;
                    float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                        RotationSmoothTime);

                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                }

                Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.back;

                // move the player
                _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                                 new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            }

            else
            {
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _mainCamera.transform.eulerAngles.y, ref _rotationVelocity,
                RotationSmoothTime);

                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

                Vector3 movementDirection = new Vector3(_input.move.x, 0.0f, _input.move.y);

                movementDirection = Quaternion.AngleAxis(_mainCamera.transform.rotation.eulerAngles.y, Vector3.up) * movementDirection; //อ้างอิงทิศทางจากกล้อง
                movementDirection.Normalize();

                // move the player
                _controller.Move(movementDirection * (_speed * Time.deltaTime) +
                                 new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            }
            
            AutoFOV(_animationBlend);

            
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
                _animator.SetFloat(_animIDHorDir, _input.move.x);
                _animator.SetFloat(_animIDVerDir, _input.move.y);
            }
        }
        private float SpeedControl()    
        {
            if(_HotMode == 3 && !_input.LockLocomotion || _HotMode == 1)
            {
                if (_input.move == Vector2.up || _input.move == new Vector2(1, 1) || _input.move == new Vector2(-1, 1))
                {
                    return _input.sprint ? SprintSpeed : MoveSpeed;
                }
            }
            return MoveSpeed;
        }
        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }
        #endregion

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
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = UnityEngine.Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }

        public void PlayGun()
        {
            _animator.SetTrigger(_animIDPlayGun);
        }

        private void SetWeightLayer(int layer, float weight)
        {
            if (_animator != null && layer >= 0 && layer < _animator.layerCount)
            {
                _animator.SetLayerWeight(layer, weight);
            }
        }
    }
}