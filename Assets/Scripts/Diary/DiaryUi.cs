using TMPro;
using UnityEngine;

public class DiaryUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _contentText;
    [SerializeField] private SfxPlayer _sfxPlayer;
    [SerializeField] private AudioClip _flipPageSfx;

    public void LoadPage(string title, string content)
    {
        _sfxPlayer.PlaySfx(_flipPageSfx);
        _titleText.text = title;
        _contentText.text = content;
    }
}
