using System;
using System.Threading;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class CameraSettings : MonoBehaviour
    {
        [SerializeField] private Transform _orientation;
        [SerializeField] private Transform _player;
        [SerializeField] private Transform _playerObj;
        [SerializeField] private Transform _rotateMesh;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private CinemachineFreeLook _thirdPersonCamera;
        [SerializeField] private PlayerMovement _playerController;
        private CinemachineRecomposer _recomposer;
        private CancellationToken _token = Application.exitCancellationToken;

        private float _verticalInput = 0;
        private float _horizontalInput = 0;
        
        private void Start()
        {
            _recomposer = _thirdPersonCamera.gameObject.GetComponent<CinemachineRecomposer>();
            _playerController.SwapCameraDutch += ChangeDutchParameter;
            _playerController.InvertCameraX += InvertInputX;
            _playerController.ChangePlayerMeshRotation += ChangePlayerRotation;
            _playerController.InvertCameraViewY += InvertCameraViewY;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        private void OnDestroy() 
        {
            _playerController.SwapCameraDutch -= ChangeDutchParameter;
            _playerController.InvertCameraX -= InvertInputX;
            _playerController.ChangePlayerMeshRotation -= ChangePlayerRotation;
            _playerController.InvertCameraViewY -= InvertCameraViewY;
        }
        private void Update()
        {
            Vector3 playerPosition = _player.position;
            Vector3 thisPosition = transform.position;
            Vector3 viewDir = playerPosition -
                              new Vector3(thisPosition.x, playerPosition.y, thisPosition.z);
            _orientation.forward = viewDir.normalized;

            Vector3 inputDir = _orientation.forward * _verticalInput + _orientation.right * _horizontalInput;
            if (inputDir != Vector3.zero)
            {
                _playerObj.forward = Vector3.Slerp(_playerObj.forward, inputDir.normalized, _rotationSpeed);
            }
        }

        private void InvertInputX(bool state)
        {
            _thirdPersonCamera.m_XAxis.m_InvertInput = state;
        }
        
        private void InvertCameraViewY(bool state)
        {
            Debug.Log("InvertCameraViewY");
            for (int i = 0; i < _thirdPersonCamera.m_Orbits.Length; i++) 
            {
                _thirdPersonCamera.m_Orbits[i].m_Height = Mathf.Abs(_thirdPersonCamera.m_Orbits[i].m_Height);
                if (state) _thirdPersonCamera.m_Orbits[i].m_Height *= -1; 
            }
        }

        private void ChangePlayerRotation(Vector3 rotation) 
        {
            Debug.Log(rotation);
            _rotateMesh.rotation = Quaternion.Euler(rotation);
            Debug.Log(_playerObj.eulerAngles);
        }

        public void GetVerticalInput(InputAction.CallbackContext ctx)
        {
            _verticalInput = ctx.ReadValue<float>();
        }

        public void GetHorizontalInput(InputAction.CallbackContext ctx)
        {
            _horizontalInput = ctx.ReadValue<float>();
        }

        private async void ChangeDutchParameter(float finalDutch, float time) 
        {
            float currentDutch = _recomposer.m_Dutch;
            float currentProgress = 0;

            while (currentProgress < time) 
            {
                _recomposer.m_Dutch = Mathf.Lerp(currentDutch,finalDutch,currentProgress/time); 

                currentProgress += Time.deltaTime;
                await Task.Yield();
                _token.ThrowIfCancellationRequested();
            }
        }
    }
}