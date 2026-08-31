using UnityEngine;

namespace Assets.Scripts.Commandables
{
    [RequireComponent(typeof(CommandTarget))]
    [RequireComponent(typeof(Rigidbody))]
    public class Throwable : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;

        [SerializeField] private float _throwForce = 20f;
        [SerializeField] private float _throwMaxVelocity = 50f;

        public void Throw()
        {
            Vector3 direction = Camera.main.transform.forward;

            if (TryGetComponent(out Pickupable pickupable))
            {
                pickupable.Drop();
            }
            _rb.maxLinearVelocity = _throwMaxVelocity;
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;

            _rb.AddForce(direction * _throwForce,ForceMode.Impulse);
        }
    }
}