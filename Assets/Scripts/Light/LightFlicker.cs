using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] private float _flickerInterval = 1.5f;
    [SerializeField] private bool _isFlickerDuringDark = false;
    private Light2D _light;
    private float _maxIntensity = 1.0f;
    private float _timer = 0.0f;

    private void Start()
    {
        _light = GetComponent<Light2D>();
        _maxIntensity = GetComponent<ToggleSceneLighting>().GetDarkSceneIntensity();
        _light.intensity = _maxIntensity;

        bool isSceneDark = GameManager.Instance.HasTransitionedToDarkScene();
        enabled = isSceneDark && _isFlickerDuringDark;
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

    /**
     * Flickers the light in a quick manner.
     * Currently doesn't work as it toggles the intensity too quickly to see the difference.
     */
    private void FlickerLight()
    {
        if (_light.intensity == 0.0f)
        {
            _light.intensity = _maxIntensity;
            _light.intensity = 0.0f;
        }
        else
        {
            _light.intensity = 0.0f;
            _light.intensity = _maxIntensity;
        }
    }

    private void ToggleLight()
    {
        if (_light.intensity == _maxIntensity)
        {
            _light.intensity = 0.0f;
        }
        else
        {
            _light.intensity = _maxIntensity;
        }
    }

}
