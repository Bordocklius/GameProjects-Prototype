using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Commandables
{
    [RequireComponent(typeof(CommandTarget))]
    public class Glowable : MonoBehaviour
    {
        [SerializeField] private float _glowIncrease;
        [SerializeField] private float _minGlow = 0f;
        [SerializeField] private float _maxGlow = 5f;

        [Space(10), Header("Light settings")]
        [SerializeField] private Light _light;
        [SerializeField] private float _lightIntensity = 5f;
        [SerializeField] private Color _lightColor = Color.white;
        [SerializeField] private float _lightRange = 10f;

        private float _currentGlowLevel = 0f;
        private Renderer _renderer;
        private MaterialPropertyBlock _matPropertyBlock;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();           

            _matPropertyBlock = new MaterialPropertyBlock();
        }

        private void Start()
        {
            CreateLight();
        }

        private void CreateLight()
        {
            // If light already exists, disable for sure
            if (_light != null)
            {
                _light.enabled = false;
                return;
            }
                
            // Create light and set up according to settings
            GameObject light = new GameObject("Glow Light");
            light.transform.SetParent(this.transform);
            light.transform.position = transform.position;
            light.transform.rotation = transform.rotation;

            _light = light.AddComponent<Light>();
            _light.type = LightType.Point;
            _light.color = _lightColor;
            _light.intensity = 1f;
            _light.range = _lightRange;

            _light.enabled = false;
            Debug.Log(_light.transform.position);
        }

        public void EnableGlow()
        {
            SetEmission(true);
            Debug.Log(_light.transform.position);

            if (!_light.enabled)
                _light.enabled = true;
        }

        public void DisableGlow()
        {
            SetEmission(false);

            if (_light.enabled)
                _light.enabled = false;
        }

        private void SetEmission(bool enabled)
        {
            _renderer.GetPropertyBlock(_matPropertyBlock);

            Color emissionColor = enabled ? _lightColor * _lightIntensity : Color.black;

            _matPropertyBlock.SetColor("_EmissionColor", emissionColor);
            _renderer.SetPropertyBlock(_matPropertyBlock);
        }
    }
}
