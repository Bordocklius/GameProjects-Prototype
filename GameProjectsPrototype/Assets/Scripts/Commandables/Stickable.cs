using UnityEngine;

namespace Assets.Scripts.Commandables
{
    [RequireComponent(typeof(CommandTarget))]
    [RequireComponent(typeof(Rigidbody))]
    public class Stickable : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;

        private bool _isSticky;

        [SerializeField] private AudioSource _audio;
        [SerializeField] private AudioClip _sticky;

        private void OnCollisionEnter(Collision collision)
        {
            if (_isSticky && collision.gameObject.name != "Player")
            {
                _rb.isKinematic = true;
            }
        }

        private void Awake()
        {
            if (_rb == null)
                _rb = GetComponent<Rigidbody>();
        }

        public void MakeSticky()
        {
            _isSticky = true;
            _audio.PlayOneShot(_sticky);
        }

        public void MakeNotSticky()
        {
            _isSticky = false;
            _rb.isKinematic = false;
        }
    }
}
