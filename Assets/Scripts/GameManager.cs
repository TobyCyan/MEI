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

    private HashSet<string> _interactedStateReporters = new HashSet<string>();

    [SerializeField] private bool _hasTransitionedToDarkScene = false;

    public void AddUnlockedDoor(string id)
    {
        _openedLockedInteractables.Add(id);
    }

    public void RemoveUnlockedDoor(string id)
    {
        _openedLockedInteractables.Remove(id);
    }

    public bool IsDoorUnlocked(string id)
    {
        return _openedLockedInteractables.Contains(id);
    }

    public void AddInteractedReporter(string id)
    {
        _interactedStateReporters.Add(id);
    }

    public void RemoveInteractedReporter(string id)
    {
        _interactedStateReporters.Remove(id);
    }

    public bool IsReporterInteracted(string id)
    {
        return _interactedStateReporters.Contains(id);
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
