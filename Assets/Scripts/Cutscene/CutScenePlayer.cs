using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class CutScenePlayer : MonoBehaviour
{
    [SerializeField] float m_FreezePlayerPosX = 0.0f;
    [SerializeField] GDTFadeEffect m_FadeEffect;
    PlayableDirector m_PlayableDirector;
    PlayerController m_Player;
    double m_PlayTime = 0.0;

    void Start()
    {
        m_PlayableDirector = GetComponent<PlayableDirector>();
        m_PlayTime = m_PlayableDirector.playableAsset.duration;
        m_Player = PlayerController.Instance;
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        yield return StartCoroutine(ActivateCutSceneFlow());
        GetComponent<BoxCollider2D>().enabled = false;
    }

    void FreezePlayer()
    {
        Vector3 playerPos = m_Player.transform.position;
        m_Player.transform.position = new Vector3(m_FreezePlayerPosX, playerPos.y, playerPos.z);
        m_Player.StopPlayerMovement();
    }

    void ActivateFadeEffect()
    {
        m_FadeEffect.firstColor = Color.clear;
        m_FadeEffect.lastColor = Color.black;
        m_FadeEffect.timeEffect = 1.0f;
        m_FadeEffect.pingPong = true;
        m_FadeEffect.disableWhenFinish = true;
        m_FadeEffect.gameObject.SetActive(true);
    }

    IEnumerator ActivateCutSceneFlow()
    {
        // Freeze the player somewhere.
        FreezePlayer();

        // Fade into the cutscene.
        ActivateFadeEffect();
        m_PlayableDirector.Play();

        // Wait for the cutscene to play out.
        yield return new WaitForSeconds((float)m_PlayTime);

        // Fade out of the cutscene.
        ActivateFadeEffect();
        m_Player.ResumePlayerMovement();
    }
}
