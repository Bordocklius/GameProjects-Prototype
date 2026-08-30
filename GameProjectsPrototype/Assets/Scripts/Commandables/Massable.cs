using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Commandables
{
    [RequireComponent(typeof(CommandTarget))]
    [RequireComponent(typeof(Rigidbody))]
    public class Massable : MonoBehaviour
    {
        [SerializeField] private float _massChange;
        [SerializeField] private Rigidbody _rb;

        [SerializeField] private float _minMass = 0.5f;
        [SerializeField] private float _maxMass = 5f;

        private void Awake()
        {
            if (_rb == null)
                _rb = GetComponent<Rigidbody>();
        }

        public void IncreaseMass()
        {
            ChangeMass(_massChange);
        }

        public void DecreaseMass()
        {
            ChangeMass(-_massChange);
        }

        private void ChangeMass(float amount)
        {
            float oldMass = _rb.mass;
            float newMass = _rb.mass + amount;
            newMass = Mathf.Clamp(newMass, _minMass, _maxMass);

            _rb.mass = newMass;
        }
    }
}
