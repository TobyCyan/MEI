using UnityEngine;
using UnityEngine.Assertions;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;
    private AudioSource _audioSource;
    private AudioLowPassFilter _filter;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        _audioSource = GetComponent<AudioSource>();
        _filter = GetComponent<AudioLowPassFilter>();
        if (_audioSource.clip != null)
        {
            _audioSource.Play();
        }
    }

    public void PlayBGM(AudioClip audioClip)
    {
        Assert.IsNotNull(_audioSource, "Audio Source is Null!");
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }

    public void MuffleMusic()
    {
        _filter.cutoffFrequency = 400;
        _filter.lowpassResonanceQ = 3;
    }

    public void UnMuffleMusic()
    {
        _filter.cutoffFrequency = 2000;
        _filter.lowpassResonanceQ = 1;
    }

    public void StopBgm()
    {
        _audioSource.Stop();
    }
}
