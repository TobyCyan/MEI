using System.Collections;

public class StareAtMirrorCutsceneObserver : CutscenePlayerObserver
{
    private void Start()
    {
        StartCoroutine(PlayCutscene());
    }

    public override IEnumerator PlayCutscene()
    {
        FreezePlayer();
        yield return PlayAssetAndWait();
        UnfreezePlayer();
    }
}
