using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        private const float GRAVITY_FORCE = 9.81f;

        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _runSpeed;
        [SerializeField] private float _gravityMultiplyer = 1;
        [SerializeField] private InputActionReference _runButton;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _jumpDebounceTime = 0.35f;
        [SerializeField] private Transform _orientation;
        [SerializeField] private bool _invertedInputX = false;
        [Space]
        [SerializeField] private float _playerHeight;
        [SerializeField] private float _groundDrag;
        [SerializeField] private float _defaultDrag = 4;
        [SerializeField] private LayerMask _whatIsGround;
        [SerializeField] private bool _grounded;

        private float _speed;
        private Quaternion _viewDirecton;
        private Vector3 _localPlayerGravity;

        public event Action<float,float> SwapCameraDutch;
        public event Action<bool> InvertCameraX;
        public event Action<bool> InvertCameraViewY;
        public event Action<Vector3> ChangePlayerMeshRotation;

        public event EventHandler OnPlayerIdle;
        public event EventHandler OnPlayerRun;
        public event EventHandler OnPlayerJump;

        private float _jumpDebounceTimer;
        private float _horizontalInput = 0;
        private float _verticalInput = 0;
        private Rigidbody _rb;
        private Vector3 _checkGroundVector;
        private bool _ableToMove = true;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            ChangeGravity(GravityDir.yM);

            _jumpDebounceTimer = _jumpDebounceTime;
            _speed = _moveSpeed;

            InvertCameraX += InvertViewDirX;
        }
        private void OnDestroy() 
        {
            InvertCameraX -= InvertViewDirX;
        }
        public void Update()
        {
            if (!_ableToMove) { return; }

            _grounded = Physics.Raycast(transform.position, _checkGroundVector, _playerHeight, _whatIsGround);
            
            // Drag control
            if (_grounded) _rb.drag = _groundDrag;
            else _rb.drag = _defaultDrag;

            // Jump Cooldown
            if (_jumpDebounceTimer >= 0)  _jumpDebounceTimer -= Time.deltaTime;

            // Switch Run Mode
            if (_runButton.action.IsPressed())
            {
                //OnPlayerRun?.Invoke(this, EventArgs.Empty);
                _speed = _runSpeed;
            }
            else
            {
                _speed = _moveSpeed;
            }
        }

        private void FixedUpdate()
        {
            //_rb.AddForce(_localPlayerGravity, ForceMode.Acceleration);
            if (!_ableToMove) { return; }

            Vector3 rightDir = _invertedInputX ? -_orientation.right : _orientation.right;
            Vector3 moveDir = _viewDirecton * _orientation.forward * _verticalInput + 
                              _viewDirecton * rightDir * _horizontalInput;
            if (moveDir != Vector3.zero)
            {
                OnPlayerRun?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                OnPlayerIdle?.Invoke(this, EventArgs.Empty);
            }
            _rb.velocity += _speed * 20f * Time.deltaTime * moveDir.normalized;
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
            bool grounded = Physics.Raycast(transform.position, _checkGroundVector, _playerHeight, _whatIsGround);

            if (!_grounded || _jumpDebounceTimer > 0)
            {
                return;
            }

            OnPlayerJump?.Invoke(this, EventArgs.Empty);

            _rb.velocity = _jumpForce * -_checkGroundVector;
            _jumpDebounceTimer = _jumpDebounceTime;
        }

        public void ChangeGravity(GravityDir gravityDir)
        {
            Vector3 gravityVector = Vector3.down;
            switch (gravityDir)
            {
                case GravityDir.yM:
                    gravityVector = new Vector3(0, -GRAVITY_FORCE * _gravityMultiplyer, 0);
                    _viewDirecton = Quaternion.Euler(0, 0, 0); // completed
                    InvertCameraX?.Invoke(false);
                    InvertCameraViewY?.Invoke(false);
                    break;
                case GravityDir.yP:
                    gravityVector = new Vector3(0, GRAVITY_FORCE * _gravityMultiplyer, 0);
                    InvertCameraX?.Invoke(true);
                    InvertCameraViewY?.Invoke(true);
                    break;
                case GravityDir.xM:
                    _viewDirecton = Quaternion.Euler(0, 0, -90);
                    gravityVector = new Vector3(-GRAVITY_FORCE * _gravityMultiplyer, 0, 0);
                    InvertCameraX?.Invoke(false);
                    InvertCameraViewY?.Invoke(false);
                    break;
                case GravityDir.xP:
                    _viewDirecton = Quaternion.Euler(0, 0, 90); // completed
                    gravityVector = new Vector3(GRAVITY_FORCE * _gravityMultiplyer, 0, 0);
                    InvertCameraX?.Invoke(false);
                    InvertCameraViewY?.Invoke(false);
                    break;
                case GravityDir.zM:
                    _viewDirecton = Quaternion.Euler(90, 0, 0); // completed
                    gravityVector = new Vector3(0, 0, -GRAVITY_FORCE * _gravityMultiplyer);
                    InvertCameraX?.Invoke(false);
                    InvertCameraViewY?.Invoke(false);
                    break;
                case GravityDir.zP:
                    _viewDirecton = Quaternion.Euler(-90, 0, 0);
                    gravityVector = new Vector3(0, 0, GRAVITY_FORCE * _gravityMultiplyer);
                    InvertCameraX?.Invoke(false);
                    InvertCameraViewY?.Invoke(false);
                    break;
            }
            Physics.gravity = gravityVector;
            _checkGroundVector = gravityVector.normalized;
        }

        private void InvertViewDirX(bool state) => _invertedInputX = state;

        public void RotatePlayerByGravityDir(GravityDir gravityDir) 
        {
            switch (gravityDir)
            {
                case GravityDir.yM:
                    ChangePlayerMeshRotation?.Invoke(new Vector3(0, -90, 0));
                    break;
                case GravityDir.yP:
                    ChangePlayerMeshRotation?.Invoke(new Vector3(0, -90, 180));
                    break;
                case GravityDir.xM:
                    ChangePlayerMeshRotation?.Invoke(new Vector3(0, -90, -90));
                    break;
                case GravityDir.xP:
                    ChangePlayerMeshRotation?.Invoke(new Vector3(0, -90, 90));
                    break;
                case GravityDir.zM:
                    ChangePlayerMeshRotation?.Invoke(new Vector3(90, -90, 0));
                    break;
                case GravityDir.zP:
                    ChangePlayerMeshRotation?.Invoke(new Vector3(-90, -90, 0));
                    break;
            }
        }
        public void ChangeCameraDutch(GravityDir gravityDir, float time) 
        {
            switch (gravityDir)
            {
                case GravityDir.yM:
                    SwapCameraDutch?.Invoke(0,time);
                    break;
                case GravityDir.yP:
                    SwapCameraDutch?.Invoke(90,time);
                    break;
                case GravityDir.xM:
                    SwapCameraDutch?.Invoke(45,time);
                    break;
                case GravityDir.xP:
                    SwapCameraDutch?.Invoke(135,time);
                    break;
                case GravityDir.zM:
                    SwapCameraDutch?.Invoke(45,time);
                    break;
                case GravityDir.zP:
                    SwapCameraDutch?.Invoke(135,time);
                    break;
            }
        }
        public void SwitchWalkAbilityState(bool state) 
        {
            _rb.isKinematic = state;
            _ableToMove = !state;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Laser"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}