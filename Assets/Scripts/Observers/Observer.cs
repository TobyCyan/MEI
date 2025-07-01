using UnityEngine;
using UnityEngine.Assertions;

public abstract class Observer : MonoBehaviour
{
    [SerializeField] protected ObserverNotifier _observerNotifier;

    void Awake()
    {
        Initialize();
    }

    // Initializes the observer.
    // Call this explicitly if Awake is defined in child class.
    protected void Initialize()
    {
        Assert.IsNotNull(_observerNotifier, "Observer "
            + gameObject.name
            + " Does Not Have A Notifier!");
        AddObserver();
    }

    private void AddObserver()
    {
        _observerNotifier.AddObserver(this);
    }

    public abstract void UpdateSelf();
}
