using Assets.Scripts.Character;
using Assets.Scripts.Commandables;
using Assets.Scripts.Commands;
using Assets.Scripts.Interfaces;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

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

    [Space(10), Header("Crosshair Settings")]
    [SerializeField] private RectTransform _crosshair;

    [SerializeField] private float _commandRange = 5f;
    [SerializeField] private float _carryRange = 5f;

    [Space(10), Header("Carry Settings")]
    [SerializeField] private Transform _carryPoint;
    [SerializeField] private float _carryDistance = 3f;

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
        HandleCarryPoint();
    }

    private void HandleCameraRotation()
    {
        // Horizontal rotation
        _characterBody.Rotate(
            0,
            _lookInput.x * _mouseSensitivity,
            0
        );

        // Vertical rotation
        _rotationX -= _lookInput.y * _mouseSensitivity;

        _rotationX = Mathf.Clamp(
            _rotationX,
            _minVerticalAngle,
            _maxVerticalAngle
        );

        transform.localRotation = Quaternion.Euler(
            _rotationX,
            0,
            0
        );
    }

    private void HandleCarryPoint()
    {
        if (_carryPoint == null || _camera == null)
            return;

        _carryPoint.position =
            _camera.transform.position +
            _camera.transform.forward * _carryDistance;
    }

    private void PlayerInput_Look(object sender, Vector2 e)
    {
        _lookInput = e;
    }

    private void PlayerInput_Attack(object sender, EventArgs e)
    {
        Ray ray =
            _camera.ScreenPointToRay(_crosshair.position);

        if (!TryGetValidHit(
            ray,
            _commandRange,
            out RaycastHit hit))
        {
            return;
        }

        GameObject obj = hit.collider.gameObject;

        // Try current object first
        if (obj.TryGetComponent<ICommandTarget>(out var target))
        {
            CommandSystem.Instance.SetTarget(obj);
            CommandInput.Instance.Activate();

            return;
        }

        // Also support colliders on children
        ICommandTarget parentTarget =
            hit.collider.GetComponentInParent<ICommandTarget>();

        if (parentTarget != null)
        {
            GameObject targetObject =
                ((Component)parentTarget).gameObject;

            CommandSystem.Instance.SetTarget(targetObject);
            CommandInput.Instance.Activate();
        }
    }

    private void PlayerInput_Interact(object sender, EventArgs e)
    {
        // Drop current object
        if (_isCarrying)
        {
            _isCarrying = false;

            _carrying.Drop();
            _carrying = null;

            return;
        }

        Ray ray =
            _camera.ScreenPointToRay(_crosshair.position);

        if (!TryGetValidHit(
            ray,
            _carryRange,
            out RaycastHit hit))
        {
            return;
        }

        Pickupable pickupable =
            hit.collider.GetComponentInParent<Pickupable>();

        if (pickupable != null)
        {
            HandlePickup(
                pickupable,
                hit.point
            );
        }
    }

    private void HandlePickup(
        Pickupable target,
        Vector3 grabPoint)
    {
        _carrying = target;
        _isCarrying = true;

        _carrying.PickUp(
            _carryPoint,
            grabPoint
        );
    }

    private bool TryGetValidHit(
        Ray ray,
        float range,
        out RaycastHit validHit)
    {
        RaycastHit[] hits =
            Physics.RaycastAll(ray, range);

        Array.Sort(
            hits,
            (a, b) =>
                a.distance.CompareTo(b.distance)
        );

        foreach (RaycastHit hit in hits)
        {
            if (HasIgnoreTag(hit.collider.transform))
                continue;

            validHit = hit;
            return true;
        }

        validHit = default;

        return false;
    }

    private bool HasIgnoreTag(Transform target)
    {
        Transform current = target;

        while (current != null)
        {
            if (current.CompareTag("Ignore"))
                return true;

            current = current.parent;
        }

        return false;
    }
}