using UnityEngine;
using UnityEngine.Assertions;

/**
 * Enables the game object this is attached to, with SFX played.
 * This component assumes that the game object starts off disabled.
 */
public class ObjectEnableWithSfxObserver : ObjectEnableObserver
{
    private void Start()
    {
        DisableObject();
    }

    public override void UpdateSelf()
    {
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        AudioSource audioSrc = GetComponent<AudioSource>();
        Assert.IsNotNull(audioSrc, "No Audio Source on: " + gameObject.name);
        audioSrc.Play();
    }
}
