using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiaryUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _contentText;
    [SerializeField] private Image _doodleImage;
    [SerializeField] private SfxPlayer _sfxPlayer;
    private AudioClip _flipPageSfx;

    private void Awake()
    {
        _flipPageSfx = Resources.Load<AudioClip>(
            GameConstants.RESOURCEPATH_SFX_UI
            + "Diary/SFX_Flip_Page"
            );
    }

    public void LoadPage(string title, string content, Sprite doodle)
    {
        _sfxPlayer.PlaySfx(_flipPageSfx);
        _titleText.text = title;
        _contentText.text = content;
        _doodleImage.sprite = doodle;
    }
}
