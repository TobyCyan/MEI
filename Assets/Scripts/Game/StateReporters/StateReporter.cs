using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateReporter : MonoBehaviour
{
    protected string _uniqueId;

    protected abstract void Initialize();
    protected abstract void MarkReporter();
    protected abstract void AssignIdAndCheckReporterState();
}
