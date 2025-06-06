using System.Collections;
using UnityEngine;

public class MeiMoveBackCutscene : CutscenePlayerObserver
{
    // Start is called before the first frame update
    void Start()
    {
        GetDirectorComponent();
    }

    public override IEnumerator PlayCutscene()
    {
        FreezePlayer();
        yield return PlayAssetAndWait();
        UnfreezePlayer();
    }
}
