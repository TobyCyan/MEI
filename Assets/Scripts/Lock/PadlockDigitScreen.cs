using TMPro;
using UnityEngine;

public class PadlockDigitScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text _keypadDisplayText;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _lockSpinnedSfx;
    private int _currentInput;

    public void AddInput()
    {
        _audioSource.PlayOneShot(_lockSpinnedSfx);
        _currentInput++;
        if (_currentInput > 9)
        {
            _currentInput = 0;
        }
        _keypadDisplayText.text = _currentInput.ToString();
    }

    public bool CheckCombo(int result)
    {
        return _currentInput == result;
    }
}
