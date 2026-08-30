using UnityEngine;

namespace Assets.Scripts.Commandables
{
    [RequireComponent(typeof(CommandTarget))]
    public class Pickupable : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody _rb;

        [Header("Light Object")]
        [SerializeField] private float _lightFollowForce = 250f;
        [SerializeField] private float _lightMaxVelocity = 10f;

        [Header("Heavy Object")]
        [SerializeField] private float _heavyFollowForce = 25f;
        [SerializeField] private float _heavyMaxVelocity = 1.5f;

        [Header("Carry Feel")]
        [SerializeField] private float _velocityDamping = 10f;
        [SerializeField] private float _massScalingPower = 2f;

        private Transform _followTransform;
        private bool _isPickedUp;

        private Massable _massable;

        // Where on the object the player grabbed it
        private Vector3 _localGrabPoint;

        private void Awake()
        {
            if (_rb == null)
                _rb = GetComponent<Rigidbody>();

            TryGetComponent(out _massable);
        }

        private void FixedUpdate()
        {
            if (!_isPickedUp || _followTransform == null)
                return;

            // Convert the saved local grab point back into world space
            Vector3 worldGrabPoint = transform.TransformPoint(_localGrabPoint);

            Vector3 direction =
                _followTransform.position - worldGrabPoint;

            float followForce = _lightFollowForce;
            float maxVelocity = _lightMaxVelocity;

            if (_massable != null)
            {
                float massPercentage = Mathf.InverseLerp(
                    _massable.MinMass,
                    _massable.MaxMass,
                    _massable.CurrentMass
                );

                // Makes the weight difference much stronger
                // near the maximum mass.
                float scaledMass = Mathf.Pow(
                    massPercentage,
                    _massScalingPower
                );

                followForce = Mathf.Lerp(
                    _lightFollowForce,
                    _heavyFollowForce,
                    scaledMass
                );

                maxVelocity = Mathf.Lerp(
                    _lightMaxVelocity,
                    _heavyMaxVelocity,
                    scaledMass
                );
            }

            _rb.maxLinearVelocity = maxVelocity;

            Vector3 grabPointVelocity =
                _rb.GetPointVelocity(worldGrabPoint);

            Vector3 force =
                direction * followForce
                - grabPointVelocity * _velocityDamping;

            _rb.AddForceAtPosition(
                force,
                worldGrabPoint,
                ForceMode.Force
            );
        }

        public void PickUp(
            Transform followTransform,
            Vector3 worldGrabPoint)
        {
            _followTransform = followTransform;
            _isPickedUp = true;

            // Save where on the object we clicked
            _localGrabPoint =
                transform.InverseTransformPoint(worldGrabPoint);

            // Keep gravity ON.
            _rb.useGravity = true;

            _rb.angularVelocity = Vector3.zero;
        }

        public void Drop()
        {
            _followTransform = null;
            _isPickedUp = false;

            _rb.useGravity = true;
        }
    }
}