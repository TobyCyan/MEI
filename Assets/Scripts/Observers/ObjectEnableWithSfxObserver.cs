using UnityEngine;
using UnityEngine.Assertions;

public class ObjectEnableWithSfxObserver : Observer
{
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
