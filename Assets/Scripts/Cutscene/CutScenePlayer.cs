using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class CutScenePlayer : MonoBehaviour
{
    #region Singleton
    public static CutScenePlayer Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
    #endregion

    [SerializeField] private GDTFadeEffect _fadeEffect;
    private PlayableDirector _playableDirector;
    private PlayerController _player;

    void Start()
    {
        _playableDirector = GetComponent<PlayableDirector>();
        _player = PlayerController.Instance;
        SetFadeParameters();
    }


    public void FreezePlayer(float positionX)
    {
        Vector3 playerPos = _player.transform.position;
        _player.transform.position = new Vector3(positionX, playerPos.y, playerPos.z);
        _player.StopPlayerMovement();
    }

    private void SetFadeParameters()
    {
        _fadeEffect.firstColor = Color.clear;
        _fadeEffect.lastColor = Color.black;
        _fadeEffect.pingPong = true;
        _fadeEffect.disableWhenFinish = true;
    }

    public void ActivateFadeEffect()
    {
        _fadeEffect.gameObject.SetActive(true);
    }

    public IEnumerator ActivateCutSceneFlow(PlayableAsset assetToPlay, MovePosition.Position freezePos, float customFreezePosX, bool canPlayerMove)
    {
        float fadeDuration = _fadeEffect.CalculateFadeDuration();

        // Fade into the cutscene.
        SetFadeParameters();
        ActivateFadeEffect();
        yield return new WaitForSeconds(fadeDuration);

        if (!canPlayerMove)
        {
            // Freeze the player somewhere.
            Vector3 freezePosition = MovePosition.GetMovePosX(freezePos, new Vector3(customFreezePosX, 0.0f, 0.0f));
            FreezePlayer(freezePosition.x);
        }
         
        _playableDirector.Play(assetToPlay);
        // Wait for the cutscene to play out.
        yield return new WaitForSeconds((float) assetToPlay.duration);

        // Fade out of the cutscene.
        ActivateFadeEffect();
        yield return new WaitForSeconds(fadeDuration);
        
        _playableDirector.Stop();
        Camera.main.GetComponent<CameraFollow>().ResetCamera();
        _player.ResumePlayerMovement();
    }
}
