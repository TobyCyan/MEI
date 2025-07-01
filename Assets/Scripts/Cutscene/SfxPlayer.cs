using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SfxPlayer : MonoBehaviour
{
    private AudioSource _audioSrc;

    private void Awake()
    {
        _audioSrc = GetComponent<AudioSource>();
    }

    public void PlaySfx(AudioClip audioClip)
    {
        _audioSrc.clip = audioClip;
        _audioSrc.Play();
    }

    public void StopSfx()
    {
        _audioSrc.Stop();
    }
}
