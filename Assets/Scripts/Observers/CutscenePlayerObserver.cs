using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public abstract class CutscenePlayerObserver : Observer
{
    [SerializeField] protected PlayableAsset _asset;
    protected PlayableDirector _director;

    protected void GetDirectorComponent()
    {
        _director = GetComponent<PlayableDirector>();
    }

    public override void UpdateSelf()
    {
        StartCoroutine(PlayCutscene());
    }

    public abstract IEnumerator PlayCutscene();

    protected void FreezePlayer()
    {
        PlayerController.Instance.StopPlayerMovement();
    }

    protected void UnfreezePlayer()
    {
        PlayerController.Instance.ResumePlayerMovement();
    }

    protected void ResetPlayerTarget()
    {
        PlayerController.Instance.ResetTarget();
    }

    protected IEnumerator PlayAssetAndWait()
    {
        _director.Play(_asset);
        yield return new WaitForSeconds((float) _asset.duration);
    }
}
