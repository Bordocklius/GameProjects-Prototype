using Assets.Scripts.Commands;
using Assets.Scripts.Interfaces;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Character
{
    public class CharacterCamera : MonoBehaviour
    {
        [Space(10), Header("Input Actions")]
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private PlayerInputHandler _playerInputHandler;

        [Space(10), Header("Camera Settings")]
        [SerializeField] private Transform _characterBody;
        [SerializeField] private Camera _camera;
        [SerializeField] private float _mouseSensitivity = 2f;
        [SerializeField] private float _minVerticalAngle = -90f;
        [SerializeField] private float _maxVerticalAngle = 90f;

        [Space(10), Header("Crosshair settings")]
        [SerializeField] private RectTransform _crosshair;

        private float _rotationX;
        private Vector2 _lookInput;

        private void Awake()
        {
            if (_playerInput == null)
                _playerInput = GetComponent<PlayerInput>();

            if (_characterBody == null)
                _characterBody = transform.parent;

            if (_camera == null)
                _camera = Camera.main;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnEnable()
        {
            _playerInputHandler.Look += PlayerInput_Look;
            _playerInputHandler.Attack += PlayerInput_Attack;
        }

        private void OnDisable()
        {
            _playerInputHandler.Look -= PlayerInput_Look;
            _playerInputHandler.Attack -= PlayerInput_Attack;
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

        private void PlayerInput_Look(object sender, Vector2 e)
        {
            _lookInput = e;
        }

        private void PlayerInput_Attack(object sender, EventArgs e)
        {
            Ray ray = _camera.ScreenPointToRay(_crosshair.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                GameObject obj = hit.collider.gameObject;
                if (obj.TryGetComponent<ICommandTarget>(out var target))
                {
                    Debug.Log("Commandable");
                    CommandSystem.Instance.SetTarget(obj);
                    CommandInput.Instance.Activate();
                }                   
            }
        }
    }
}