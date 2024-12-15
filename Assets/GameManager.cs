using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance == this)
        {
            Destroy(this);
            DontDestroyOnLoad(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion
}
