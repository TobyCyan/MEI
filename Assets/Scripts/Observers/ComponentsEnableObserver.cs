using System.Collections.Generic;
using UnityEngine;

public class ComponentsEnableObserver : Observer
{
    [SerializeField] private List<MonoBehaviour> _components = new();

    public override void UpdateSelf()
    {
        foreach (var component in _components)
        {
            component.enabled = true;
        }
    }
}
