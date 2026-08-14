using Assets.Scripts.Commands;
using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Commandables
{
    public class Scalable: MonoBehaviour
    {
        [SerializeField] private float _growAmount = 1f;
        [SerializeField] private Rigidbody _rb;

        [SerializeField] private float _minScale = 0.5f;
        [SerializeField] private float _maxScale = 5f;

        private void Awake()
        {
            if(_rb == null)
                _rb = GetComponent<Rigidbody>();
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
            float oldscale = transform.localScale.x;
            float newScale = transform.localScale.x + amount;
            newScale = Mathf.Clamp(newScale, _minScale, _maxScale)  ;

            float actualChange = newScale - oldscale;

            transform.localScale = Vector3.one * newScale;

            // Compensate for larger scale if scaling up
            if(actualChange > 0f)
            {
                Vector3 pos = _rb.position;
                pos.y += actualChange / 2f;

                _rb.MovePosition(pos);
            }
        }
    }
}
