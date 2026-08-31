using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Interactables
{
    public class Button : MonoBehaviour
    {
        [Header("Mass")]
        [SerializeField] private float _requiredMass = 4f;

        [Header("Doors")]
        [SerializeField] private Transform _leftDoor;
        [SerializeField] private Transform _rightDoor;
        [SerializeField] private float _doorMoveDistance = 2f;

        [Header("Bridge")]
        [SerializeField] private bool _useBridge = false;
        [SerializeField] private Transform _bridgeRotation;
        [SerializeField] private float _bridgeRotationAngle = 90f;

        [Header("Button")]
        [SerializeField] private Transform _buttonVisual;
        [SerializeField] private float _buttonPressDistance = 0.1f;

        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 5f;

        [Header("UI")]
        [SerializeField] private TutorialUI _tutorialUI;

        private bool _isActivated;
        private Rigidbody _detectedRigidbody;

        private Vector3 _leftDoorStartPos;
        private Vector3 _rightDoorStartPos;
        private Vector3 _buttonStartPos;

        private Quaternion _bridgeStartRotation;

        private void Awake()
        {
            _leftDoorStartPos = _leftDoor.localPosition;
            _rightDoorStartPos = _rightDoor.localPosition;
            _buttonStartPos = _buttonVisual.localPosition;

            if (_useBridge && _bridgeRotation != null)
            {
                _bridgeStartRotation = _bridgeRotation.localRotation;
            }
        }

        private void Update()
        {
            CheckMass();

            MoveDoors();
            MoveButton();

            if (_useBridge)
                RotateBridge();
        }

        private void CheckMass()
        {
            if (_detectedRigidbody == null)
            {
                _isActivated = false;
                return;
            }

            _isActivated = _detectedRigidbody.mass >= _requiredMass;

            if (_tutorialUI != null)
            {
                _tutorialUI.SetHint(
                    _isActivated ? "" : "Too light?"
                );
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Rigidbody rb = other.attachedRigidbody;

            if (rb == null)
                return;

            _detectedRigidbody = rb;

            Debug.Log($"Detected object with mass: {rb.mass}");
        }

        private void OnTriggerExit(Collider other)
        {
            Rigidbody rb = other.attachedRigidbody;

            if (rb == null)
                return;

            if (rb == _detectedRigidbody)
            {
                _detectedRigidbody = null;
                _isActivated = false;
            }

            if (_tutorialUI != null)
                _tutorialUI.SetHint("");
        }

        private void MoveDoors()
        {
            Vector3 leftDoorTarget = _isActivated
                ? _leftDoorStartPos + Vector3.left * _doorMoveDistance
                : _leftDoorStartPos;

            Vector3 rightDoorTarget = _isActivated
                ? _rightDoorStartPos + Vector3.right * _doorMoveDistance
                : _rightDoorStartPos;

            _leftDoor.localPosition = Vector3.Lerp(
                _leftDoor.localPosition,
                leftDoorTarget,
                Time.deltaTime * _moveSpeed
            );

            _rightDoor.localPosition = Vector3.Lerp(
                _rightDoor.localPosition,
                rightDoorTarget,
                Time.deltaTime * _moveSpeed
            );
        }

        private void MoveButton()
        {
            Vector3 buttonTarget = _isActivated
                ? _buttonStartPos + Vector3.down * _buttonPressDistance
                : _buttonStartPos;

            _buttonVisual.localPosition = Vector3.Lerp(
                _buttonVisual.localPosition,
                buttonTarget,
                Time.deltaTime * _moveSpeed
            );
        }

        private void RotateBridge()
        {
            if (_bridgeRotation == null)
                return;

            Quaternion targetRotation = _isActivated
                ? _bridgeStartRotation *
                  Quaternion.Euler(_bridgeRotationAngle, 0f, 0f)
                : _bridgeStartRotation;

            _bridgeRotation.localRotation = Quaternion.Slerp(
                _bridgeRotation.localRotation,
                targetRotation,
                Time.deltaTime * _moveSpeed
            );
        }
    }
}