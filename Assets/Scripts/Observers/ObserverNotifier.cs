using System.Collections.Generic;
using UnityEngine;

public class ObserverNotifier : MonoBehaviour
{
    private readonly List<Observer> _observers = new();

    public void NotifyObservers()
    {
        foreach (var observer in _observers)
        {
            observer.UpdateSelf();
        }
    }

    public void AddObserver(Observer observer)
    {
        _observers.Add(observer);
    }
}
