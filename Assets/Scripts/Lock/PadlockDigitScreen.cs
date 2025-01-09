using TMPro;
using UnityEngine;

public class PadlockDigitScreen : PadlockScreen
{
    [SerializeField] private TMP_Text _keypadDisplayText;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _lockSpinnedSfx;
    private int _currentInput = 0;

    public override void AddInput()
    {
        _audioSource.PlayOneShot(_lockSpinnedSfx);
        _currentInput++;
        if (_currentInput > 9)
        {
            _currentInput = 0;
        }
        _keypadDisplayText.text = _currentInput.ToString();
    }

    public override bool CheckCombo(int result)
    {
        return _currentInput == result;
    }

    public override bool CheckCombo(string result)
    {
        return false;
    }
}
