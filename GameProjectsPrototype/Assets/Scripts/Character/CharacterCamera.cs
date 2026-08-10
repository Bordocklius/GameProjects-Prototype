using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Character
{
    public class CharacterCamera : MonoBehaviour
    {
        [Space(10), Header("Input Actions")]
        [SerializeField] private PlayerInput _playerInput;

        [Space(10), Header("Camera Settings")]
        [SerializeField] private Transform _characterBody;
        [SerializeField] private float _mouseSensitivity = 2f;
        [SerializeField] private float _minVerticalAngle = -90f;
        [SerializeField] private float _maxVerticalAngle = 90f;

        private float _rotationX;
        private Vector2 _lookInput;

        private void Awake()
        {
            if (_playerInput == null)
                _playerInput = GetComponent<PlayerInput>();

            if (_characterBody == null)
                _characterBody = transform.parent;

            //Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnEnable()
        {
            _playerInput.actions["Look"].performed += PlayerInput_Look;
            _playerInput.actions["Look"].canceled += PlayerInput_Look;
        }

        private void OnDisable()
        {
            _playerInput.actions["Look"].performed -= PlayerInput_Look;
            _playerInput.actions["Look"].canceled -= PlayerInput_Look;
        }

        private void Update()
        {
            HandleCameraRotation();
        }
        private void HandleCameraRotation()
        {
            // Rotate character body left/right
            _characterBody.Rotate(0, _lookInput.x * _mouseSensitivity, 0);

            // Rotate camera up/down
            _rotationX -= _lookInput.y * _mouseSensitivity;
            _rotationX = Mathf.Clamp(_rotationX, _minVerticalAngle, _maxVerticalAngle);

            transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        }

        private void PlayerInput_Look(InputAction.CallbackContext context)
        {
            _lookInput = context.ReadValue<Vector2>();
        }

    }
}