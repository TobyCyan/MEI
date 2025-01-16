using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MeiClassroomCutScene : MonoBehaviour
{
    private CutScenePlayer _cutScenePlayer;
    [SerializeField] private List<GameObject> _actors = new List<GameObject>();
    [SerializeField] private MovePosition.Position _freezePlayerPosition;
    [Header("Only Fill This When Player is Frozen at a Custom Position.")]
    [SerializeField] private float _customFreezePlayerPosX = 0.0f;
    [SerializeField] private PlayableAsset _asset;
    [SerializeField] private AudioSource _clappingLaughingAudioSource;
    private float _sfxDuration = 0.0f;

    private void Start()
    {
        _cutScenePlayer = CutScenePlayer.Instance;
        GetComponent<BoxCollider2D>().isTrigger = true;
        ToggleActors(false);
        _sfxDuration = _clappingLaughingAudioSource.clip.length;
    }

    IEnumerator ActivateCutScene()
    {
        // Initial Zoom In CutScene.
        yield return _cutScenePlayer.ActivateCutSceneFlow(_asset, _freezePlayerPosition, _customFreezePlayerPosX, false);

        // Add actors.
        ToggleActors(true);

        // Toggle Clapping & Laughing SFX.
        _clappingLaughingAudioSource.Play();
        yield return new WaitForSeconds(_sfxDuration);

        // Remove actors.
        ToggleActors(false);

        yield break;
    }

    private void ToggleActors(bool toggle)
    {
        foreach (GameObject actor in _actors)
        {
            actor.SetActive(toggle);
        }
    }

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        yield return StartCoroutine(ActivateCutScene());
        GetComponent<BoxCollider2D>().enabled = false;
    }

}
