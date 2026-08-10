using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Commandables
{
    public class Scalable: MonoBehaviour
    {
        [SerializeField] private float _growAmount = 1f;
        private float _minScale = 0.5f;
        private float _maxScale = 5f;

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
            float newScale = transform.localScale.x + amount;
            newScale = Mathf.Clamp(newScale, _minScale, _maxScale)  ;

            transform.localScale = Vector3.one * newScale;
        }
    }
}
