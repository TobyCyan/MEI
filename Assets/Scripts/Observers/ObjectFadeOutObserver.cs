using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ObjectFadeOutObserver : Observer
{
    [SerializeField] private float _fadeOutSpeed = 1.5f;
    [SerializeField] private bool _isFadeOut = true;
    private SpriteRenderer _renderer;
    private Color _initialColor;
    private Color _finalColor;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        // Set color params.
        if (_isFadeOut)
        {
            _initialColor = _renderer.color;
            _finalColor = Color.clear;
        }
        else
        {
            _initialColor = Color.clear;
            _finalColor = _renderer.color;
            _renderer.color = _initialColor;
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
            _renderer.color = Color.Lerp(_initialColor, _finalColor, elapsedTime);
            yield return null;
        }
    }
}
