using System.Collections.Generic;
using System.Linq;
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
    /** Maintain a set of locked interactables IDs to check whether a door has been unlocked when going between scenes. **/
    private HashSet<string> _openedLockedInteractables = new HashSet<string>();

    private HashSet<string> _interactedManagers = new HashSet<string>();

    public void AddUnlockedDoor(string doorID)
    {
        _openedLockedInteractables.Add(doorID);
    }

    public void RemoveUnlockedDoor(string unlockedDoorID)
    {
        _openedLockedInteractables.Remove(unlockedDoorID);
    }

    public bool IsDoorUnlocked(string doorID)
    {
        return _openedLockedInteractables.Contains(doorID);
    }

    public void AddInteractedManager(string interactedManagerID)
    {
        _interactedManagers.Add(interactedManagerID);
    }

    public void RemoveInteractedManager(string interactedManagerID)
    {
        _interactedManagers.Remove(interactedManagerID);
    }

    public bool IsManagerInteracted(string interactedManagerID)
    {
        return _interactedManagers.Contains(interactedManagerID);
    }
}
