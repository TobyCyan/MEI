using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StareAtMirrorCutscene : CutScenePlayer
{
    private void Start()
    {
        StartCoroutine(Interact());
    }

    public override IEnumerator Interact()
    {
        yield return StartCoroutine(ActivateCutScene());
    }

    public override IEnumerator ActivateCutScene()
    {
        yield break;
    }
}
