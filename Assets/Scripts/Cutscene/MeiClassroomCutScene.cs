using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MeiClassroomCutScene : CutScenePlayer
{
    [SerializeField] private List<GameObject> _actors = new List<GameObject>();
    [SerializeField] private MovePosition.Position _freezePlayerPosition;
    [Header("Only Fill This When Player is Frozen at a Custom Position.")]
    [SerializeField] private float _customFreezePlayerPosX = 0.0f;
    [SerializeField] private AudioSource _clappingLaughingAudioSource;
    private float _sfxDuration = 0.0f;

    private void Start()
    {
        GetDirectorComponent();
        GetComponent<BoxCollider2D>().isTrigger = true;
        ToggleActors(false);
        _sfxDuration = _clappingLaughingAudioSource.clip.length;
    }

    public override IEnumerator Interact()
    {
        yield return StartCoroutine(ActivateCutScene());
    }

    public override IEnumerator ActivateCutScene()
    {
        GetComponent<Collider2D>().enabled = false;

        // Initial Zoom In CutScene.
        yield return ActivateCutSceneFlow(_freezePlayerPosition, _customFreezePlayerPosX, false);
        PlayerController.Instance.ResetCamera();

        // Add actors.
        ToggleActors(true);

        // Toggle Clapping & Laughing SFX.
        _clappingLaughingAudioSource.Play();
        yield return new WaitForSeconds(_sfxDuration);

        // Remove actors.
        ToggleActors(false);

        ResetToPlayer();
        yield break;
    }

    private void ToggleActors(bool toggle)
    {
        foreach (GameObject actor in _actors)
        {
            actor.SetActive(toggle);
        }
    }

}
