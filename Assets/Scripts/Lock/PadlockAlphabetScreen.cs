using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PadlockAlphabetScreen : PadlockScreen
{
    [SerializeField] private TMP_Text _keypadDisplayText;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _lockSpinnedSfx;
    private char[] alphabetLists = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
    private int _currentInput = 0;
    private string _currentAlpha = "A";

    public override void AddInput()
    {
        _audioSource.PlayOneShot(_lockSpinnedSfx);
        _currentInput++;
        if (_currentInput >= 26)
        {
            _currentInput = 0;
        }
        _currentAlpha = alphabetLists[_currentInput].ToString();
        _keypadDisplayText.text = alphabetLists[_currentInput].ToString();
    }

    public override bool CheckCombo(string result)
    {
        return _currentAlpha.Equals(result);
    }

    public override bool CheckCombo(int result)
    {
        return false;
    }
}
