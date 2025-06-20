using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

/**
 * Attach this script to any interactable that will perform a fade in/ fade out transition.
 */
[RequireComponent(typeof(SfxPlayer))]
public class TransitionInteractable : Interactable
{
    [SerializeField] private GDTFadeEffect _fadeEffect;
    [SerializeField] private bool _isPingPong = true;
    [SerializeField] private AudioClip _startAudioClip;
    [SerializeField] private AudioClip _endAudioClip;
    [SerializeField] private float _pauseDuration = 0.75f;
    [SerializeField] private bool _disableWhenFinish = true;
    [SerializeField] private bool _isInvisibleTransitionObject = false;

    private float _fadeDuration = 0.0f;
    private float _startAudioDuration = 0.0f;
    private float _endAudioDuration = 0.0f;
    private SpriteRenderer _spriteRenderer;
    private SfxPlayer _sfxPlayer;

    private void Awake()
    {
        _startAudioClip = Resources.Load<AudioClip>(
            GameConstants.RESOURCEPATH_SFX_ENVIRONMENT 
            + "SFX_Open_Door"
            );
        _endAudioClip = Resources.Load<AudioClip>(
            GameConstants.RESOURCEPATH_SFX_ENVIRONMENT
            + "SFX_Close_Door"
            );
    }

    private void Start()
    {
        Assert.IsNotNull(_fadeEffect, "Transition Interactable " 
            + gameObject.name 
            + " Does Not Have A Fade Effect Object!");
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _sfxPlayer = GetComponent<SfxPlayer>();
        if (_isInvisibleTransitionObject)
        {
            _spriteRenderer.enabled = false;
        }

        if (_startAudioClip != null)
        {
            _startAudioDuration = _startAudioClip.length;
        }

        if (_endAudioClip != null)
        {
            _endAudioDuration = _endAudioClip.length;
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
        _fadeEffect.firstColor = Color.clear;
        _fadeEffect.lastColor = Color.black;
       
        _fadeEffect.gameObject.SetActive(true);
        if (_startAudioClip != null )
        {
            _sfxPlayer.PlaySfx(_startAudioClip);
        }

        float currTime = 0.0f;
        float playLength = _startAudioDuration + _pauseDuration;
        while (currTime < playLength)
        {
            currTime += Time.deltaTime;
            yield return null;
        }

        if (_endAudioClip != null)
        {
            _sfxPlayer.PlaySfx(_endAudioClip);
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
