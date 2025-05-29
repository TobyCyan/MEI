using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialMannequin : MonoBehaviour
{
    [SerializeField] private int id;

    public bool CheckCombo(int result)
    {
        return id == result;
    }
}
