using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class InteractionStateReporter : MonoBehaviour
{
    protected string _uniqueId;
    protected bool _isInteracted = false;

    protected void Initialize()
    {
        AssignIdAndCheckInteractionState();
    }

    protected void MarkReporter()
    {
        _isInteracted = true;
        GameManager.Instance.AddInteractedReporter(_uniqueId);
    }

    private void AssignIdAndCheckInteractionState()
    {
        // GameObject names in the same scene are unique.
        _uniqueId = SceneManager.GetActiveScene().name + gameObject.name;

        // Check if this manager has been interacted before,
        // which will prevent the interaction from happening.
        _isInteracted = GameManager.Instance.IsReporterInteracted(_uniqueId);
    }
}
