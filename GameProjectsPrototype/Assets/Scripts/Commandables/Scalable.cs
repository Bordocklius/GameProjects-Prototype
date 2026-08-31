using Assets.Scripts.Commands;
using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Commandables
{
    [RequireComponent(typeof(CommandTarget))]
    [RequireComponent(typeof(Rigidbody))]
    public class Scalable: MonoBehaviour
    {
        [SerializeField] private float _growAmount = 1f;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Collider _collider;

        [SerializeField] private float _minScale = 0.5f;
        [SerializeField] private float _maxScale = 5f;

        private Vector3 _originalScale;
        private float _currentScale = 1f;

        private void Awake()
        {
            if(_rb == null)
                _rb = GetComponent<Rigidbody>();
            if(_collider == null)
                _collider = GetComponent<Collider>();

            _originalScale = transform.localScale;
        }

        public void Grow()
        {
            ChangeSize(_growAmount);
        }

        public void Shrink()
        {
            ChangeSize(-_growAmount);
        }        

        private void ChangeSize(float amount)
        {
            float oldScale = _currentScale;

            _currentScale = Mathf.Clamp(_currentScale + amount, _minScale, _maxScale);
            float actualChange = _currentScale - oldScale;

            if (Mathf.Approximately(actualChange, 0f))
                return;

            //float oldscale = transform.localScale.x;
            //float newScale = transform.localScale.x + amount;
            //newScale = Mathf.Clamp(newScale, _minScale, _maxScale)  ;

            //float actualChange = newScale - oldscale;

            //transform.localScale = Vector3.one * newScale;
            transform.localScale = _originalScale * _currentScale;

            // Compensate for larger scale if scaling up
            if (actualChange > 0f)
            {
                Vector3 pos = _rb.position;
                pos.y += actualChange / 2f;

                _rb.MovePosition(pos);
            }
        }
    }
}
