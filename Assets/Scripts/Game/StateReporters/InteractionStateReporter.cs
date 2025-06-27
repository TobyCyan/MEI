using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class InteractionStateReporter : StateReporter
{
    protected bool _isInteracted = false;

    protected override void Initialize()
    {
        AssignIdAndCheckReporterState();
    }

    protected override void MarkReporter()
    {
        _isInteracted = true;
        GameManager.Instance.AddInteractedReporter(_uniqueId);
    }

    protected override void AssignIdAndCheckReporterState()
    {
        // GameObject names in the same scene are unique.
        _uniqueId = SceneManager.GetActiveScene().name + gameObject.name;

        // Check if this manager has been interacted before,
        // which will prevent the interaction from happening.
        _isInteracted = GameManager.Instance.IsReporterInteracted(_uniqueId);
    }
}
