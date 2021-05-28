using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public enum MoveState
    {
        Crouching = 0,
        Walking = 1,
        Sprinting = 2
    }
    
    public static bool IsEnabled = true;
    
    [SerializeField] private Camera _characterCamera;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _maxCameraAngle = 90;

    private CharacterController _characterController;
    private readonly Vector3 _gravity = Physics.gravity;
    private Vector3 _velocity;
    private CharacterInput _characterInput;

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundMask;
    private float _groundDistance = 0.3f;
    private bool _isGrounded = false;

    [SerializeField] private Animator _animator;

    [SerializeField] private float _movementSpeed = 8;
    [SerializeField] private float _jumpHeight = 2;
    [SerializeField] private float _movementModifier = 2;

    private bool _isMoving = false;
    private bool _isHoldingWeapon = false;

    private MoveState _moveState = MoveState.Walking;
    private static readonly int horizontal = Animator.StringToHash("horizontal");
    private static readonly int vertical = Animator.StringToHash("vertical");
    private static readonly int jump = Animator.StringToHash("jump");
    private static readonly int isCrouching = Animator.StringToHash("isCrouching");
    private static readonly int isHoldingWeapon = Animator.StringToHash("isHoldingWeapon");

    private void Awake()
    {
        _characterCamera.gameObject.SetActive(true);
        _characterController = gameObject.GetComponent<CharacterController>();

        InitializeInput();
    }

    private void Update()
    {
        if (IsEnabled)
        {
            GroundCheck();
            CheckIfWeaponIsEquipped();

            float modifier = 0;
            
            float x = _characterInput.Player.Movement.ReadValue<Vector2>().x;
            float z = _characterInput.Player.Movement.ReadValue<Vector2>().y;
            
            if (_isMoving)
            {
                modifier = UpdateMovement(x, z);
            }
            else
            {
                UpdateIdle();
            }
            
            _animator.SetFloat(horizontal, x * modifier);
            _animator.SetFloat(vertical, z * modifier);

            ApplyGravity();
        }
    }

    private void UpdateIdle()
    {
        var cameraForward = _cameraTransform.forward;
        cameraForward.y = 0;
        var angle = Vector3.SignedAngle(transform.forward, cameraForward, Vector3.up);

        if (Mathf.Abs(angle) > _maxCameraAngle)
        {
            angle = Mathf.Sign(angle) * (Mathf.Abs(angle) - _maxCameraAngle);
            transform.Rotate(Vector3.up, angle, Space.World);
        }
    }

    private float UpdateMovement(float x, float z)
    {
        float modifier;
        float cam = _cameraTransform.rotation.eulerAngles.x;
        transform.rotation = Quaternion.Euler(0, _cameraTransform.rotation.eulerAngles.y, 0);
        _cameraTransform.localEulerAngles = new Vector3(cam, 0, 0);

        Vector3 moveX = new Vector3(_cameraTransform.right.x, 0, _cameraTransform.right.z).normalized * x;
        Vector3 moveZ = new Vector3(_cameraTransform.forward.x, 0, _cameraTransform.forward.z).normalized * z;

        Vector3 move = moveX + moveZ;

        modifier = 1;

        if (_moveState == MoveState.Sprinting)
        {
            modifier = _movementModifier;
        }
        else if (_moveState == MoveState.Crouching)
        {
            modifier = 1 / _movementModifier;
        }

        _characterController.Move(move * (_movementSpeed * modifier * Time.deltaTime));
        return modifier;
    }

    private void InitializeInput()
    {
        _characterInput = new CharacterInput();

        _characterInput.Player.Movement.performed += Move;
        _characterInput.Player.Jump.performed += Jump;
        _characterInput.Player.StartSprint.performed += StartSprint;
        _characterInput.Player.StopSprint.performed += StopSprint;
        _characterInput.Player.Crouch.performed += Crouch;
    }

    private void GroundCheck()
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);
    }
    
    private void Move(InputAction.CallbackContext context)
    {
        _isMoving = !_isMoving;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        GroundCheck();

        if (_isGrounded && _moveState != MoveState.Crouching)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity.y);
            _animator.SetTrigger(jump);
        }
    }

    private void StartSprint(InputAction.CallbackContext context)
    {
        if (_moveState != MoveState.Crouching && !_isHoldingWeapon)
        {
            if (_moveState != MoveState.Sprinting)
            {
                _moveState = MoveState.Sprinting;
            }
            else
            {
                _moveState = MoveState.Walking;
            }
        }
    }

    private void StopSprint(InputAction.CallbackContext context)
    {
        if (_moveState == MoveState.Sprinting)
        {
            _moveState = MoveState.Walking;
        }
    }

    private void Crouch(InputAction.CallbackContext context)
    {
        if (_moveState != MoveState.Crouching)
        {
            _moveState = MoveState.Crouching;
            _animator.SetBool(isCrouching, true);
        }
        else
        {
            _moveState = MoveState.Walking;
            _animator.SetBool(isCrouching, false);
        }
    }

    private void CheckIfWeaponIsEquipped()
    {
        _isHoldingWeapon = _animator.GetBool(isHoldingWeapon);
    }

    private void ApplyGravity()
    {
        _velocity += _gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = 0;
        }
    }

    public int GetPlayerStealthProfile()
    {
        if (_isMoving)
        {
            return (int)_moveState;
        }
        
        return -1;
    }

    private void OnEnable()
    {
        _characterInput.Enable();
    }

    private void OnDisable()
    {
        _characterInput.Disable();
    }
}
