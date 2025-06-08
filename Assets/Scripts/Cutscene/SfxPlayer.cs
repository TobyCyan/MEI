using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SfxPlayer : MonoBehaviour
{
    [Header("Must Assign The Audio Clip.")]
    [SerializeField] private AudioClip _clip;
    private AudioSource _audioSrc;

    private void Start()
    {
        _audioSrc = GetComponent<AudioSource>();
    }

    public void PlaySfx()
    {
        _audioSrc.clip = _clip;
        _audioSrc.Play();
    }

    public void StopSfx()
    {
        _audioSrc.Stop();
    }
}
