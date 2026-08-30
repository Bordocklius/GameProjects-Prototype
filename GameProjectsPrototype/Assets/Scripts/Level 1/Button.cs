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

        [Header("Button")]
        [SerializeField] private Transform _buttonVisual;
        [SerializeField] private float _buttonPressDistance = 0.1f;

        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 5f;

        [Header("UI")]
        [SerializeField] private TutorialUI _tutorialUI;

        private bool _isActivated;

        private Vector3 _leftDoorStartPos;
        private Vector3 _rightDoorStartPos;
        private Vector3 _buttonStartPos;

        private void Awake()
        {
            _leftDoorStartPos = _leftDoor.localPosition;
            _rightDoorStartPos = _rightDoor.localPosition;
            _buttonStartPos = _buttonVisual.localPosition;
        }

        private void Update()
        {
            Vector3 leftDoorTarget = _isActivated ? _leftDoorStartPos + Vector3.left * _doorMoveDistance : _leftDoorStartPos;

            Vector3 rightDoorTarget = _isActivated ? _rightDoorStartPos + Vector3.right * _doorMoveDistance : _rightDoorStartPos;

            Vector3 buttonTarget = _isActivated ? _buttonStartPos + Vector3.down * _buttonPressDistance : _buttonStartPos;

            _leftDoor.localPosition = Vector3.Lerp(_leftDoor.localPosition, leftDoorTarget, Time.deltaTime * _moveSpeed);

            _rightDoor.localPosition = Vector3.Lerp(_rightDoor.localPosition, rightDoorTarget, Time.deltaTime * _moveSpeed);

            _buttonVisual.localPosition = Vector3.Lerp(_buttonVisual.localPosition, buttonTarget, Time.deltaTime * _moveSpeed);
        }

        private void OnTriggerStay(Collider other)
        {
            Rigidbody rb = other.attachedRigidbody;

            if (rb == null)
                return;

            if (rb.mass >= _requiredMass)
            {
                if (!_isActivated)
                {
                    _isActivated = true;
                    Debug.Log($"Button activated! Mass: {rb.mass}");
                }

                if (_tutorialUI != null)
                    _tutorialUI.SetHint("");
            }
            else
            {
                if (_isActivated)
                {
                    _isActivated = false;
                }

                Debug.Log($"Not heavy enough. Mass: {rb.mass}");

                if (_tutorialUI != null)
                    _tutorialUI.SetHint("Too light?");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Rigidbody rb = other.attachedRigidbody;

            if (rb == null)
                return;

            _isActivated = false;
            Debug.Log("Object left the button.");

            if (_tutorialUI != null)
                _tutorialUI.SetHint("");
        }
    }
}