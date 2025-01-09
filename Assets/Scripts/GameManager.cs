using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
    #endregion
    /** Maintain a list of locked interactables to check whether a door has been unlocked when going between scenes. **/
    private List<LockedInteractable> _openedLockedInteractables = new List<LockedInteractable>();

    public void AddUnlockedDoor(LockedInteractable door)
    {
        _openedLockedInteractables.Add(door);
    }

    public void RemoveUnlockedDoor(LockedInteractable unlockedDoor)
    {
        _openedLockedInteractables.Remove(unlockedDoor);
    }

    public bool isDoorUnlocked(LockedInteractable door)
    {
        return _openedLockedInteractables.Contains(door);
    }
}
