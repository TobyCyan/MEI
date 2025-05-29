using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ObjectFadeOutObserver : Observer
{
    [SerializeField] private float _fadeOutSpeed = 1.5f;

    public override void UpdateSelf()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color initialColor = Color.white;
        Color finalColor = Color.clear;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        while (elapsedTime < 1.0f)
        {
            elapsedTime += _fadeOutSpeed * Time.deltaTime;
            renderer.color = Color.Lerp(initialColor, finalColor, elapsedTime);
            yield return null;
        }
    }
}
