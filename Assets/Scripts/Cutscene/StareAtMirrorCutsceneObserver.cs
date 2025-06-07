using System.Collections;

public class StareAtMirrorCutsceneObserver : CutscenePlayerObserver
{
    private void Start()
    {
        GetDirectorComponent();
    }

    public override IEnumerator PlayCutscene()
    {
        FreezePlayer();
        yield return PlayAssetAndWait();
        ResetToPlayer();
    }
}
