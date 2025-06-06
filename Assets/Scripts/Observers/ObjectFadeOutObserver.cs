using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ObjectFadeOutObserver : Observer
{
    [SerializeField] private float _fadeOutSpeed = 1.5f;
    [SerializeField] private bool _isFadeOut = true;

    public override void UpdateSelf()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color initialColor = renderer.color;
        Color finalColor = Color.clear;
        while (elapsedTime < 1.0f)
        {
            elapsedTime += _fadeOutSpeed * Time.deltaTime;
            if (_isFadeOut)
            {
                renderer.color = Color.Lerp(initialColor, finalColor, elapsedTime);
            }
            else
            {
                renderer.color = Color.Lerp(finalColor, initialColor, elapsedTime);
            }
            yield return null;
        }
    }
}
