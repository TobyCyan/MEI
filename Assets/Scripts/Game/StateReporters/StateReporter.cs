using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class StateReporter : MonoBehaviour
{
    protected enum ReporterToReport
    {
        SELF,
        CUSTOM,
    }

    protected string _uniqueId;
    [SerializeField] protected ReporterToReport _reporterToReport;
    [SerializeField] protected string _customReporterName;
    [SerializeField] protected string _customReporterScene;

    protected abstract void Initialize();
    protected abstract void MarkReporter();
    protected abstract void AssignIdAndCheckReporterState();
    public abstract bool IsMarked();

    protected string GetReporterToReportId()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        switch (_reporterToReport)
        {
            case ReporterToReport.SELF:
                return sceneName + gameObject.name;

            case ReporterToReport.CUSTOM:
                return _customReporterScene + _customReporterName;

            default:
                return sceneName + "Invalid";
        }
    }
}
