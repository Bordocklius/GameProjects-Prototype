using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Character
{
    public class CharacterMovement : MonoBehaviour
    {
        [Space(10), Header("Input Actions")]
        [SerializeField] private PlayerInput _playerInput;

        [Space(10), Header("Movement Settings")]
        [SerializeField] private CharacterController _cc;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private float _movespeed;
        [SerializeField] private float _gravity;
        [SerializeField] private float _jumpHeight;

        private Vector2 _moveInput;

        private void Awake()
        {
            if (_playerInput == null)
                _playerInput = GetComponent<PlayerInput>();

            if (_cc == null)
                _cc = GetComponent<CharacterController>();

            if (_mainCamera == null)
                _mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            _playerInput.actions["Move"].performed += PlayerInput_Move;
            _playerInput.actions["Move"].canceled += PlayerInput_Move;
        }

        private void Start()
        {         

        }

        private void Update()
        {
            HandleMovement();
        }

        #region InputHandlers & Movement

        private void HandleMovement()
        {
            if (_moveInput.sqrMagnitude < 0.0001f)
                return;

            Vector3 cameraForward = new Vector3(_mainCamera.transform.forward.x, 0, _mainCamera.transform.forward.z).normalized;
            Vector3 cameraRight = new Vector3(_mainCamera.transform.right.x, 0, _mainCamera.transform.right.z).normalized;

            Vector3 movement = (cameraRight * _moveInput.x + cameraForward * _moveInput.y).normalized;

            _cc.Move(movement * _movespeed * Time.deltaTime);
        }

        private void PlayerInput_Move(InputAction.CallbackContext ctx)
        {
            _moveInput = ctx.ReadValue<Vector2>();
        }

        #endregion

    }
}
