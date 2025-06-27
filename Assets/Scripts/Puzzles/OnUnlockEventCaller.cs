using UnityEngine;

[RequireComponent(typeof(ObserverNotifier))]
public class OnUnlockEventCaller : MonoBehaviour
{
    private LockedStateReporter _lockedStateReporter;
    private ObserverNotifier _observerNotifier;

    private void Awake()
    {
        _lockedStateReporter = GetComponent<LockedStateReporter>();
        _observerNotifier = GetComponent<ObserverNotifier>();
    }

    private void Start()
    {
        bool isUnlocked = _lockedStateReporter.IsMarked();
        if (isUnlocked)
        {
            _observerNotifier.NotifyObservers();
        }
    }
}
