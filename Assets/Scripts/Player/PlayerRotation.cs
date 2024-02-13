using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerRotation : MonoBehaviour
    {
        [SerializeField] private Transform _orientation;
        [SerializeField] private Transform _player;
        [SerializeField] private Transform _playerObj;
        [SerializeField] private float _rotationSpeed;

        private float _verticalInput = 0;
        private float _horizontalInput = 0;

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

        public void GetVerticalInput(InputAction.CallbackContext ctx)
        {
            _verticalInput = ctx.ReadValue<float>();
        }

        public void GetHorizontalInput(InputAction.CallbackContext ctx)
        {
            _horizontalInput = ctx.ReadValue<float>();
        }
    }
}