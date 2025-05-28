using System;
using System.Collections;
using UnityEngine;

public class MeiMoveBackCutscene : CutscenePlayerObserver
{
    [SerializeField] private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        GetDirectorComponent();
    }

    public override IEnumerator PlayCutscene()
    {
        FreezePlayer();
        //_animator.SetTrigger(GameConstants.TRIGGER_MOVE_BACK);
        yield return PlayAssetAndWait();
        UnfreezePlayer();
    }
}
