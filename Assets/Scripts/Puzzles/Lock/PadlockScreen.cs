using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PadlockScreen : MonoBehaviour
{
    public abstract void AddInput();
    public abstract bool CheckCombo(int result);

    public abstract bool CheckCombo(String result);
}
