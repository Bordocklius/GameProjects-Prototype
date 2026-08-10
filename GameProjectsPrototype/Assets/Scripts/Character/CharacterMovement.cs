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
        [SerializeField] private float _movespeed = 10f;
        [SerializeField] private float _gravity = -9.81f;
        [SerializeField] private float _jumpHeight = 1.5f;

        private Vector2 _moveInput;
        private float _verticalVelocity;

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

            _playerInput.actions["Jump"].performed += PlayerInput_Jump;
        }

        private void Start()
        {         

        }

        private void Update()
        {
            HandleGravity();
            HandleMovement();            
        }

        #region InputHandlers & Movement

        private void HandleMovement()
        {
            //if (_moveInput.sqrMagnitude < 0.0001f)
            //    return;

            Vector3 cameraForward = new Vector3(_mainCamera.transform.forward.x, 0, _mainCamera.transform.forward.z).normalized;
            Vector3 cameraRight = new Vector3(_mainCamera.transform.right.x, 0, _mainCamera.transform.right.z).normalized;

            Vector3 velocity = (cameraRight * _moveInput.x + cameraForward * _moveInput.y).normalized;
            velocity.y = _verticalVelocity;
            _cc.Move(velocity * _movespeed * Time.deltaTime);
        }

        private void HandleGravity()
        {
            if (_cc.isGrounded && _verticalVelocity < 0f)
            {
                _verticalVelocity = -2f;
            }
            else
            {
                _verticalVelocity += _gravity * Time.deltaTime;
            }
        }

        private void PlayerInput_Move(InputAction.CallbackContext ctx)
        {
            _moveInput = ctx.ReadValue<Vector2>();
        }

        private void PlayerInput_Jump(InputAction.CallbackContext ctx)
        {
            if (!_cc.isGrounded)
                return;

            _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }

        #endregion

    }
}
