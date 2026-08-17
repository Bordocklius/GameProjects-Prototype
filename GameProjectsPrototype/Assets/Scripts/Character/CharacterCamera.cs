using Assets.Scripts.Commandables;
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
        [SerializeField] private Transform _carryPoint;
        [SerializeField] private float _commandRange = 50f;
        [SerializeField] private float _carryRange = 10f;

        private float _rotationX;
        private Vector2 _lookInput;
        private bool _isCarrying;
        private Pickupable _carrying;

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
            _playerInputHandler.Interact += PlayerInput_Interact;
        }

        private void OnDisable()
        {
            _playerInputHandler.Look -= PlayerInput_Look;
            _playerInputHandler.Attack -= PlayerInput_Attack;
            _playerInputHandler.Interact -= PlayerInput_Interact;
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
            if (Physics.Raycast(ray, out hit, _commandRange))
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

        private void PlayerInput_Interact(object sender, EventArgs e)
        {
            // Drop carrying object if carrying one
            if(_isCarrying)
            {
                _isCarrying = false;
                _carrying.Drop();
                _carrying = null;
                return;
            }

            Ray ray = _camera.ScreenPointToRay(_crosshair.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, _carryRange))
            {
                GameObject obj = hit.collider.gameObject;
                if(obj.TryGetComponent<Pickupable>(out var target))
                {
                    HandlePickup(target);
                }
            }
        }

        private void HandlePickup(Pickupable target)
        {
            _carrying = target;
            _isCarrying = true;
            _carrying.PickUp(_carryPoint);
        }
    }
}