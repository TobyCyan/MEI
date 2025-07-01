using UnityEngine;

[RequireComponent(typeof(InteractionManager))]
public class InteractionActivationObserver : Observer
{
    private InteractionManager _interactionManager;

    private void Awake()
    {
        Initialize();
        _interactionManager = GetComponent<InteractionManager>();
    }

    public override void UpdateSelf()
    {
        StartCoroutine(_interactionManager.GoThroughInteractions());
    }
}
