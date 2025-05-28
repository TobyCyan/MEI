using UnityEngine;

[RequireComponent(typeof(ObserverAdder))]
public abstract class Observer : MonoBehaviour
{
    public abstract void UpdateSelf();
}
