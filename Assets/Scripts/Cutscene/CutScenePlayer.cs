using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class CutScenePlayer : MonoBehaviour
{
    [SerializeField] private float _freezePlayerPosX = 0.0f;
    [SerializeField] private GDTFadeEffect _fadeEffect;
    private PlayableDirector _playableDirector;
    private PlayerController _player;
    private double _playTime = 0.0;

    void Start()
    {
        _playableDirector = GetComponent<PlayableDirector>();
        _playTime = _playableDirector.playableAsset.duration;
        _player = PlayerController.Instance;
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        yield return StartCoroutine(ActivateCutSceneFlow());
        GetComponent<BoxCollider2D>().enabled = false;
    }

    void FreezePlayer()
    {
        Vector3 playerPos = _player.transform.position;
        _player.transform.position = new Vector3(_freezePlayerPosX, playerPos.y, playerPos.z);
        _player.StopPlayerMovement();
    }

    void ActivateFadeEffect()
    {
        _fadeEffect.firstColor = Color.clear;
        _fadeEffect.lastColor = Color.black;
        _fadeEffect.timeEffect = 1.0f;
        _fadeEffect.pingPong = true;
        _fadeEffect.disableWhenFinish = true;
        _fadeEffect.gameObject.SetActive(true);
    }

    IEnumerator ActivateCutSceneFlow()
    {
        // Freeze the player somewhere.
        FreezePlayer();

        // Fade into the cutscene.
        ActivateFadeEffect();
        _playableDirector.Play();

        // Wait for the cutscene to play out.
        yield return new WaitForSeconds((float) _playTime);

        // Fade out of the cutscene.
        ActivateFadeEffect();
        _player.ResumePlayerMovement();
    }
}
