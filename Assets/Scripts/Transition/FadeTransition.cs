using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTransition : MonoBehaviour
{
    [SerializeField] private GameObject _fadeEffect;

    public void ActivateTransition()
    {
        _fadeEffect.SetActive(true);
    }
}
