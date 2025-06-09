using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFadeOutObserver : Observer
{
    [SerializeField] private float _fadeOutSpeed = 1.5f;
    [SerializeField] private bool _isFadeOut = true;
    private Light2D _light2d;
    private float _initialIntensity;
    private float _finalIntensity;

    // Start is called before the first frame update
    void Start()
    {
        _light2d = GetComponent<Light2D>();
        
        if (_isFadeOut)
        {
            _initialIntensity = _light2d.intensity;
            _finalIntensity = 0.0f;
        }
        else
        {
            _initialIntensity = 0.0f;
            _finalIntensity = _light2d.intensity;
            _light2d.intensity = _initialIntensity;
        }
    }

    public override void UpdateSelf()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < 1.0f)
        {
            elapsedTime += _fadeOutSpeed * Time.deltaTime;
            _light2d.intensity = Mathf.Lerp(_initialIntensity, _finalIntensity, elapsedTime);
            yield return null;
        }
    }
}
