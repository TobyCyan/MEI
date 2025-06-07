using UnityEngine;
using UnityEngine.Assertions;

public class BGMChanger : MonoBehaviour
{
    [Header("Leave Blank If On Scene Load Audio Clip Changing Is Not Needed.")]
    [SerializeField] private AudioClip _onSceneLoadAudioClip;
    private BGMManager _bgmManager;

    private void Start()
    {
        _bgmManager = BGMManager.Instance;
        Assert.IsNotNull(_bgmManager, "BGM Changer Found that BGM Manager is Null!");
        if (_onSceneLoadAudioClip != null)
        {
            ChangeBgm(_onSceneLoadAudioClip);
        }
    }

    public void ChangeBgm(AudioClip audioClip)
    {
        _bgmManager.PlayBGM(audioClip);
        _bgmManager.UnMuffleMusic();
    }

    public void StopBgm()
    {
        _bgmManager.StopBgm();
    }
}
