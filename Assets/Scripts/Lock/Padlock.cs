using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Padlock : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] PadlockDigitScreen[] lockValue;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSource2;
    [SerializeField] private AudioClip accessGrantedSfx;
    [SerializeField] private AudioClip accessDeniedSfx;
    [SerializeField] int[] correctCombination = new int[] { 0, 0, 0, 0 };

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void checkResult()
    {
        if (lockValue[0].CheckCombo(correctCombination[0]) && lockValue[1].CheckCombo(correctCombination[1])
            && lockValue[2].CheckCombo(correctCombination[2]) && lockValue[3].CheckCombo(correctCombination[3]))
        {
            audioSource.PlayOneShot(accessGrantedSfx);
        } else
        {
            audioSource2.PlayOneShot(accessDeniedSfx);  
        }

    }
}
