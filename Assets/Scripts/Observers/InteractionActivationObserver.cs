using UnityEngine;

[RequireComponent(typeof(InteractionManager))]
public class InteractionActivationObserver : Observer
{
    private InteractionManager _interactionManager;

    private void Start()
    {
        _interactionManager = GetComponent<InteractionManager>();
    }

    public override void UpdateSelf()
    {
        StartCoroutine(_interactionManager.GoThroughInteractions());
    }
}
