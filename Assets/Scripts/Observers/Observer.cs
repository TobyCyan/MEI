using UnityEngine;
using UnityEngine.Assertions;

public abstract class Observer : MonoBehaviour
{
    [SerializeField] protected ObserverNotifier _observerNotifier;

    void Awake()
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
