using TMPro;
using UnityEngine;

public class DiaryUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _contentText;
    [SerializeField] private SfxPlayer _sfxPlayer;
    private AudioClip _flipPageSfx;

    private void Awake()
    {
        _flipPageSfx = Resources.Load<AudioClip>(
            GameConstants.RESOURCEPATH_SFX_UI
            + "Diary/SFX_Flip_Page"
            );
    }

    public void LoadPage(string title, string content)
    {
        _sfxPlayer.PlaySfx(_flipPageSfx);
        _titleText.text = title;
        _contentText.text = content;
    }
}
