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
    /** Maintain a set of locked interactables IDs to check whether a door has been unlocked when going between scenes. **/
    private HashSet<string> _openedLockedInteractables = new HashSet<string>();

    private HashSet<string> _interactedManagers = new HashSet<string>();

    [SerializeField] private bool _hasTransitionedToDarkScene = false;

    public void AddUnlockedDoor(string doorId)
    {
        _openedLockedInteractables.Add(doorId);
    }

    public void RemoveUnlockedDoor(string unlockedDoorId)
    {
        _openedLockedInteractables.Remove(unlockedDoorId);
    }

    public bool IsDoorUnlocked(string doorId)
    {
        return _openedLockedInteractables.Contains(doorId);
    }

    public void AddInteractedManager(string interactedManagerId)
    {
        _interactedManagers.Add(interactedManagerId);
    }

    public void RemoveInteractedManager(string interactedManagerId)
    {
        _interactedManagers.Remove(interactedManagerId);
    }

    public bool IsManagerInteracted(string interactedManagerId)
    {
        return _interactedManagers.Contains(interactedManagerId);
    }

    public void TransitionToDarkScene()
    {
        _hasTransitionedToDarkScene = true;
    }

    public bool HasTransitionedToDarkScene()
    {
        return _hasTransitionedToDarkScene;
    }

}
