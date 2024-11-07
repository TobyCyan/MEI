using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Attach this script to any interactable that will perform a fade in/ fade out transition.
 */
public class TransitionInteractable : Interactable
{
    [SerializeField] private GDTFadeEffect _fadeEffect;
    [SerializeField] private float _fadeDuration;

    public override IEnumerator Interact()
    {
        yield return StartCoroutine(ActivateTransition());
    }

    public IEnumerator ActivateTransition()
    {
        _fadeEffect.timeEffect = _fadeDuration;
        _fadeEffect.gameObject.SetActive(true);
        yield return null;
    }
}
