using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Commandables
{
    [RequireComponent(typeof(CommandTarget))]
    [RequireComponent(typeof(Rigidbody))]
    public class Pickupable : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private float _followForce = 100f;
        [SerializeField] private float _maxFollowVelocity = 10f;

        private Transform _followTransform;
        private bool _isPickedUp;

        private void Awake()
        {
            if(_rb == null)
            {
                _rb = GetComponent<Rigidbody>();
            }

            _rb.maxLinearVelocity = _maxFollowVelocity;
        }

        private void FixedUpdate()
        {
            if (!_isPickedUp)
                return;

            Vector3 direction = _followTransform.position - _rb.position;

            if(direction.sqrMagnitude < 0.01f)
            {
                _rb.position = _followTransform.position;
                _rb.linearVelocity = Vector3.zero;
                return;
            }

            _rb.AddForce(direction * _followForce, ForceMode.Force);

            //if(_rb.linearVelocity.magnitude > _maxFollowVelocity)
            //{
            //    _rb.linearVelocity = _rb.linearVelocity.normalized * _maxFollowVelocity;
            //}
        }

        public void PickUp(Transform followTransform)
        {
            _followTransform = followTransform;
            _isPickedUp = true;

            _rb.useGravity = false;
            _rb.linearVelocity = Vector3.zero;
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
