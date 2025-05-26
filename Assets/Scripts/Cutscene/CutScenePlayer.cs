using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class CutScenePlayer : Interactable
{
    [SerializeField] private GDTFadeEffect _fadeEffect;

    protected void FreezePlayer(PlayerController player, float positionX)
    {
        Debug.Log("Freezing player");
        Vector3 playerPos = player.transform.position;
        player.transform.position = new Vector3(positionX, playerPos.y, playerPos.z);
        player.StopPlayerMovement();
    }

    protected void SetFadeParametersWithPingPong()
    {
        _fadeEffect.firstColor = Color.clear;
        _fadeEffect.lastColor = Color.black;
        _fadeEffect.pingPong = true;
        _fadeEffect.disableWhenFinish = true;
    }

    protected void SetFadeParametersWithoutPingPong()
    {
        _fadeEffect.firstColor = Color.clear;
        _fadeEffect.lastColor = Color.black;
        _fadeEffect.pingPong = false;
        _fadeEffect.disableWhenFinish = true;
    }

    public void ActivateFadeEffect()
    {
        _fadeEffect.gameObject.SetActive(true);
    }

    protected void ResetToPlayer()
    {
        Debug.Log("Reset to player");
        PlayerController.Instance.ResetCamera();
        PlayerController.Instance.ResumePlayerMovement();
    }

    public IEnumerator ActivateCutSceneFlow(PlayableDirector director, PlayableAsset asset, MovePosition.Position freezePos, float customFreezePosX, bool canPlayerMove)
    {
        float fadeDuration = _fadeEffect.CalculateFadeDuration();

        // Fade into the cutscene.
        SetFadeParametersWithPingPong();
        ActivateFadeEffect();

        if (!canPlayerMove)
        {
            // Freeze the player somewhere.
            Vector3 freezePosition = MovePosition.GetMovePosX(freezePos, new Vector3(customFreezePosX, 0.0f, 0.0f));
            FreezePlayer(PlayerController.Instance, freezePosition.x);
        }

        yield return new WaitForSeconds(fadeDuration);

        director.Play(asset);
        // Wait for the cutscene to play out.
        yield return new WaitForSeconds((float) asset.duration);

        // Fade out of the cutscene.
        SetFadeParametersWithoutPingPong();
        ActivateFadeEffect();
        fadeDuration = _fadeEffect.CalculateFadeDuration();
        yield return new WaitForSeconds(fadeDuration);

        director.Stop();
    }

}
