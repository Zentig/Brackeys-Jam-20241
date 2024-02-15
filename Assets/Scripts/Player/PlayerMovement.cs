using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _runSpeed;
        [SerializeField] private InputActionReference _runButton;
        [SerializeField] private float _jumpForce;
        [SerializeField] private Transform _orientation;
        [Space]
        [SerializeField] private float _playerHeight;
        [SerializeField] private float _groundDrag;
        [SerializeField] private float _defaultDrag = 4;
        [SerializeField] private LayerMask _whatIsGround;
        [SerializeField] private bool _grounded;
        private float _speed;

        public event EventHandler OnPlayerIdle;
        public event EventHandler OnPlayerWalk;
        public event EventHandler OnPlayerRun;
        public event EventHandler OnPlayerJump;

        [SerializeField] private float _jumpDebounceTime = 0.35f;
        private float _jumpDebounceTimer;

        private float _horizontalInput = 0;
        private float _verticalInput = 0;
        private Rigidbody _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();

            _jumpDebounceTimer = _jumpDebounceTime;
            _speed = _moveSpeed;
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }

        public void Update()
        {
            // Drag control
            float raycastDistance = _playerHeight;
            _grounded = Physics.Raycast(transform.position, Vector3.down, raycastDistance, _whatIsGround);
            if (_grounded)
            {
                _rb.drag = _groundDrag;
            }
            else
            {
                _rb.drag = _defaultDrag;
            }

            // Jump Cooldown
            if (_jumpDebounceTimer >= 0)
            {
                _jumpDebounceTimer -= Time.deltaTime;
            }

            // Switch Run Mode
            if (_runButton.action.IsPressed())
            {
                OnPlayerRun?.Invoke(this, EventArgs.Empty);
                _speed = _runSpeed;
            }
            else
            {
                _speed = _moveSpeed;
            }
        }

        private void FixedUpdate()
        {
            if(Input.GetKey(KeyCode.W))
                print("SS");
            Vector3 moveDir = _orientation.forward * _verticalInput + _orientation.right * _horizontalInput;
            if (moveDir != Vector3.zero)
            {
                OnPlayerWalk?.Invoke(this, EventArgs.Empty);
                print("SS");
            }
            else
            {
                OnPlayerIdle?.Invoke(this, EventArgs.Empty);
            }

            _rb.AddForce(_speed * 150f * Time.deltaTime * moveDir.normalized, ForceMode.Force);
        }

        public void GetVerticalInput(InputAction.CallbackContext ctx)
        {
            _verticalInput = ctx.ReadValue<float>();
        }

        public void GetHorizontalInput(InputAction.CallbackContext ctx)
        {
            _horizontalInput = ctx.ReadValue<float>();
        }

        public void Jump(InputAction.CallbackContext ctx)
        {
            float raycastDistance = _playerHeight;
            bool grounded = Physics.Raycast(transform.position, Vector3.down, raycastDistance, _whatIsGround);

            if (!_grounded || _jumpDebounceTimer > 0)
            {
                return;
            }

            OnPlayerJump?.Invoke(this, EventArgs.Empty);

            _rb.velocity = Vector3.zero;
            _rb.velocity += _jumpForce * 75f * Time.deltaTime * Vector3.up;
            _jumpDebounceTimer = _jumpDebounceTime;
        }
    }
}