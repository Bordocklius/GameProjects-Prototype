using Assets.Scripts.Commandables;
using UnityEngine;

public class DeleteItems : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Stickable _stickable;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Item"))
            return;

        Rigidbody rb = other.attachedRigidbody;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            _stickable.MakeNotSticky();

            rb.position = _spawnPoint.position;
            rb.rotation = _spawnPoint.rotation;
        }
        else
        {
            other.transform.position = _spawnPoint.position;
            other.transform.rotation = _spawnPoint.rotation;
        }
    }
}
