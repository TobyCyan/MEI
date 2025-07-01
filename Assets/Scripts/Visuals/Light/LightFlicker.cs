using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] private float _flickerInterval = 1.5f;
    [SerializeField] private float _minIntensity = 0.6f;
    [SerializeField] private float _maxIntensity = 1.0f;
    [Header("Disable This Component Entirely and Enabled This Field If Flickering Only Happens During Dark.")]
    [SerializeField] private bool _isFlickerDuringDark = false;

    private Light2D _light;
    private float _timer = 0.0f;

    private void Start()
    {
        _light = GetComponent<Light2D>();
        _light.intensity = _maxIntensity;
        if (_isFlickerDuringDark)
        {
            enabled = GameManager.Instance.HasTransitionedToDarkScene();
        }
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        
        if (_timer >= _flickerInterval)
        {
            ToggleLight();
            _timer = 0.0f;
        }
    }

    private void ToggleLight()
    {
        if (_light.intensity == _maxIntensity)
        {
            _light.intensity = _minIntensity;
        }
        else
        {
            _light.intensity = _maxIntensity;
        }
    }
}
