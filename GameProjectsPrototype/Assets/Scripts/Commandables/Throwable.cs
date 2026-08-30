using UnityEngine;

namespace Assets.Scripts.Commandables
{
    [RequireComponent(typeof(CommandTarget))]
    [RequireComponent(typeof(Rigidbody))]
    public class Throwable : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;

        [SerializeField] private float _throwForce;

        public void Throw()
        {
            Vector3 direction = Camera.main.transform.forward;
            GetComponent<Pickupable>().Drop();
            _rb.AddForce(direction * _throwForce, ForceMode.Impulse);
        }
    }
}
