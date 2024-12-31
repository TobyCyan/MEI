using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PadlockDigitScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text keypadDisplayText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip lockSpinnedSfx;
    private int currentInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddInput()
    {
        audioSource.PlayOneShot(lockSpinnedSfx);
        currentInput++;
        if (currentInput > 9)
        {
            currentInput = 0;
        }
        keypadDisplayText.text = currentInput.ToString();
    }

    public Boolean CheckCombo(int result)
    {
        return currentInput == result;
    }
}
