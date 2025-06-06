using System.Collections;
using UnityEngine;

public class StareAtMirrorCutscene : CutScenePlayer
{
    [SerializeField] private Vector3 _finalCamPos;
    [SerializeField] private CameraFollow _camera;

    private void Start()
    {
        GetDirectorComponent();
    }

    public override IEnumerator Interact()
    {
        yield return StartCoroutine(ActivateCutScene());
    }

    public override IEnumerator ActivateCutScene()
    {
        yield return ActivateCutSceneFlow(MovePosition.Position.PLAYER_POSITION, 0.0f, false);
        _camera.FreezeCameraToPos(_finalCamPos);
    }
}
