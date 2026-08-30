using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Commandables
{
    public class Massable : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody _rb;

        [Header("Mass Settings")]
        [SerializeField] private float _massChange = 1f;
        [SerializeField] private float _minMass = 0.5f;
        [SerializeField] private float _maxMass = 5f;

        public float CurrentMass => _rb.mass;
        public float MinMass => _minMass;
        public float MaxMass => _maxMass;

        public bool IsAtMaxMass =>
            Mathf.Approximately(_rb.mass, _maxMass);

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
            float newMass = _rb.mass + amount;

            newMass = Mathf.Clamp(
                newMass,
                _minMass,
                _maxMass
            );

            _rb.mass = newMass;
        }
    }
}
