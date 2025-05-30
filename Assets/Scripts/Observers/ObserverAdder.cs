using UnityEngine;
using UnityEngine.Assertions;

public class ObserverAdder : MonoBehaviour
{
    [SerializeField] protected InteractionManager _interactionManager;

    void Awake()
    {
        Assert.IsNotNull(_interactionManager, "Observer Adder " 
            + gameObject.name 
            + " Does Not Have An Interaction Manager!");
        AddObservers();
    }

    private void AddObservers()
    {
        foreach (var observer in GetComponents<Observer>())
        {
            _interactionManager.AddObserver(observer);
        }
    }
}
