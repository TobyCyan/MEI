using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Padlock : MonoBehaviour
{
    public static event Action<string, int> displayed = delegate { };
    private bool coroutineAllowed;
    private int numberShown;
    void Start()
    {
        coroutineAllowed = true;
        numberShown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnMouseDown()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(displayValue());
        }
    }

    private IEnumerator displayValue()
    {
        coroutineAllowed = false;
        for (int i = 0; i  < 9; i ++)
        {
            yield return new WaitForSeconds(1);
        }
        coroutineAllowed = false;
        numberShown ++;
        if (numberShown > 9)
        {
            numberShown = 0;
        }
        displayed(name, 0);
    }
}
