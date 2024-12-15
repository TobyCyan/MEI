using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Attach this script to any interactable that will perform a fade in/ fade out transition.
 */
public class TransitionInteractable : Interactable
{
    [SerializeField] private GDTFadeEffect _fadeEffect;
    [SerializeField] private bool _isPingPong = true;
    [SerializeField] private AudioSource _startAudio;
    [SerializeField] private AudioSource _endAudio;
    [SerializeField] private float _pauseDuration = 0.75f;
    [SerializeField] private bool _disableWhenFinish = true;

    private float _fadeDuration = 0.0f;
    private float _startAudioDuration = 0.0f;
    private float _endAudioDuration = 0.0f;

    private void Start()
    {
        if (_startAudio != null)
        {
            _startAudioDuration = _startAudio.clip.length;
        }

        if (_endAudio != null)
        {
            _endAudioDuration = _endAudio.clip.length;
        }

        _fadeDuration = _pauseDuration + 0.5f;
    }

    public override IEnumerator Interact()
    {
        yield return StartCoroutine(ActivateTransition());
    }

    /**
     * Activates fade effect and waits until the whole transition is completed.
     */
    public IEnumerator ActivateTransition()
    {
        _fadeEffect.pingPongDelay = _fadeDuration;
        _fadeEffect.pingPong = _isPingPong;
        _fadeEffect.disableWhenFinish = _disableWhenFinish;
       
        _fadeEffect.gameObject.SetActive(true);
        if (_startAudio != null )
        {
            _startAudio.Play();
        }

        float currTime = 0.0f;
        float playLength = _startAudioDuration + _pauseDuration;
        while (currTime < playLength)
        {
            currTime += Time.deltaTime;
            yield return null;
        }

        if (_endAudio != null)
        {
            _endAudio.Play();
        }

        currTime = 0.0f;
        while (currTime < _endAudioDuration)
        {
            currTime += Time.deltaTime;
            yield return null;
        }

        yield break;
    }
}
