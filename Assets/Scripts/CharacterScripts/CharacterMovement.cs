using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] Camera _characterCamera;
    [SerializeField] private Transform _cameraTransform;
    private CharacterController _characterController;

    private Vector3 _gravity = Physics.gravity;
    private Vector3 _velocity;

    private CharacterInput _characterInput;

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundMask;
    private float _groundDistance = 0.1f;
    private bool _isGrounded = false;

    [SerializeField] private Animator _animator;

    [SerializeField] private float _movementSpeed = 8;
    [SerializeField] private float _jumpHeight = 2;
    private float _sprintModifier = 2;
    private bool _isMoving = false;
    private bool _isJumping = false;
    private bool _isSprinting = false;
    private bool _isCrouching = false;


    private void Awake()
    {
        _characterCamera.gameObject.SetActive(true);
        //Cursor.lockState = CursorLockMode.Locked;
        _characterController = gameObject.GetComponent<CharacterController>();

        InitializeInput();
    }

    private void Update()
    {
        GroundCheck();

        if (_isMoving)
        {
            float x = _characterInput.Player.Movement.ReadValue<Vector2>().x;
            float z = _characterInput.Player.Movement.ReadValue<Vector2>().y;

            float cam = _cameraTransform.rotation.eulerAngles.x;
            transform.rotation = Quaternion.Euler(0, _cameraTransform.rotation.eulerAngles.y, 0);
            _cameraTransform.localEulerAngles = new Vector3(cam, 0, 0);

            Vector3 moveX = new Vector3(_cameraTransform.right.x, 0, _cameraTransform.right.z) * x;
            Vector3 moveZ = new Vector3(_cameraTransform.forward.x, 0, _cameraTransform.forward.z) * z;

            Vector3 move = moveX + moveZ;

            if (_isSprinting)
            {
                _characterController.Move(move * _movementSpeed * _sprintModifier * Time.deltaTime);

                if (!_isJumping)
                {
                    _animator.SetFloat("horizontal", x * _sprintModifier);
                    _animator.SetFloat("vertical", z * _sprintModifier);
                }
            }
            else if (_isCrouching)
            {
                _characterController.Move(move * _movementSpeed / _sprintModifier * Time.deltaTime);

                if (!_isJumping)
                {
                    _animator.SetFloat("horizontal", x / _sprintModifier);
                    _animator.SetFloat("vertical", z / _sprintModifier);
                }
            }
            else
            {
                _characterController.Move(move * _movementSpeed * Time.deltaTime);

                if (!_isJumping)
                {
                    _animator.SetFloat("horizontal", x);
                    _animator.SetFloat("vertical", z);
                }
            }
        }
        else
        {
            _animator.SetFloat("horizontal", 0);
            _animator.SetFloat("vertical", 0);
        }

        ApplyGravity();
    }

    private void InitializeInput()
    {
        _characterInput = new CharacterInput();

        _characterInput.Player.Movement.performed += context => Move();
        _characterInput.Player.Jump.performed += context => Jump();
        _characterInput.Player.Sprint.performed += context => Sprint();
        _characterInput.Player.Crouch.performed += context => Crouch();
    }

    private void GroundCheck()
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);

        if (_isGrounded)
        {
            _isJumping = false;
        }
    }

    private void Move()
    {
        _isMoving = !_isMoving;
    }

    private void Jump()
    {
        GroundCheck();

        if (_isGrounded && !_isCrouching)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity.y);
            _animator.SetTrigger("jump");
            _isJumping = true;
        }
    }

    private void Sprint()
    {
        if (!_isCrouching)
        {
            _isSprinting = !_isSprinting;
        }

    }

    private void Crouch()
    {
        _isCrouching = !_isCrouching;

        if (!_isCrouching)
        {
            _animator.SetBool("isCrouching", false);
        }
        else
        {
            _animator.SetBool("isCrouching", true);
        }
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

    private void OnEnable()
    {
        _characterInput.Enable();
    }

    private void OnDisable()
    {
        _characterInput.Disable();
    }
}
