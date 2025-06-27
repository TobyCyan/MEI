public class InteractionStateReporter : StateReporter
{
    protected bool _isInteracted = false;

    private void Awake()
    {
        Initialize();
    }

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
        _uniqueId = GetReporterToReportId();

        // Check if this manager has been interacted before,
        // which will prevent the interaction from happening.
        _isInteracted = IsMarked();
    }

    public override bool IsMarked()
    {
        return GameManager.Instance.IsReporterInteracted(_uniqueId);
    }
}
