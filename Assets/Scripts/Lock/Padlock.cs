using UnityEngine;

public class Padlock : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PadlockScreen[] _lockValue;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _audioSource2;
    [SerializeField] private AudioClip _accessGrantedSfx;
    [SerializeField] private AudioClip _accessDeniedSfx;
    [SerializeField] private bool isAlphaLock = false;
    [SerializeField] private int[] _correctNumberCombination = new int[] { 0, 0, 0, 0 };
    [SerializeField] private string[] _correctAlphabetCombination = new string[] { "A", "A", "A", "A" };

    public void CheckResult()
    {
        if (!isAlphaLock)
        {
            if (_lockValue[0].CheckCombo(_correctNumberCombination[0]) && _lockValue[1].CheckCombo(_correctNumberCombination[1])
           && _lockValue[2].CheckCombo(_correctNumberCombination[2]) && _lockValue[3].CheckCombo(_correctNumberCombination[3]))
            {
                _audioSource.PlayOneShot(_accessGrantedSfx);
            }
            else
            {
                _audioSource2.PlayOneShot(_accessDeniedSfx);
            }
        } else
        {
            if (_lockValue[0].CheckCombo(_correctAlphabetCombination[0]) && _lockValue[1].CheckCombo(_correctAlphabetCombination[1])
           && _lockValue[2].CheckCombo(_correctAlphabetCombination[2]) && _lockValue[3].CheckCombo(_correctAlphabetCombination[3]))
            {
                _audioSource.PlayOneShot(_accessGrantedSfx);
            }
            else
            {
                _audioSource2.PlayOneShot(_accessDeniedSfx);
            }
        }
        
    }
}
