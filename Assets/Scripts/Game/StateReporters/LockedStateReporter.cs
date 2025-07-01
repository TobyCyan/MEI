public class LockedStateReporter : StateReporter
{
    protected bool _isLocked = true;

    protected override void Initialize()
    {
        AssignIdAndCheckReporterState();
    }

    protected override void MarkReporter()
    {
        _isLocked = true;
        GameManager.Instance.AddUnlockedReporter(_uniqueId);
    }

    protected override void AssignIdAndCheckReporterState()
    {
        // GameObject names in the same scene are unique.
        _uniqueId = GetReporterToReportId();

        // Check if this manager has been interacted before,
        // which will prevent the interaction from happening.
        _isLocked = IsMarked();
    }

    public override bool IsMarked()
    {
        return GameManager.Instance.IsReporterUnlocked(_uniqueId);
    }
}
