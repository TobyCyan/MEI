using UnityEngine;

public class Padlock : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PadlockDigitScreen[] _lockValue;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _audioSource2;
    [SerializeField] private AudioClip _accessGrantedSfx;
    [SerializeField] private AudioClip _accessDeniedSfx;
    [SerializeField] private int[] _correctCombination = new int[] { 0, 0, 0, 0 };

    public void CheckResult()
    {
        if (_lockValue[0].CheckCombo(_correctCombination[0]) && _lockValue[1].CheckCombo(_correctCombination[1])
            && _lockValue[2].CheckCombo(_correctCombination[2]) && _lockValue[3].CheckCombo(_correctCombination[3]))
        {
            _audioSource.PlayOneShot(_accessGrantedSfx);
        } else
        {
            _audioSource2.PlayOneShot(_accessDeniedSfx);  
        }
    }
}
