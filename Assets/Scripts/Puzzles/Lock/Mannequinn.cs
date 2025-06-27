using UnityEngine;

public class Mannequinn: MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private SpecialMannequin[] _lockValue;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _audioSource2;
    [SerializeField] private AudioClip _accessGrantedSfx;
    [SerializeField] private AudioClip _accessDeniedSfx;
    [SerializeField] private int[] _correctNumberCombination = new int[] { 0, 0, 0, 0 };
 

    public void CheckResult()
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
    }
}

