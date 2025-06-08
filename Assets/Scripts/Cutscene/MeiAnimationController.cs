using UnityEngine;

public class MeiAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    /**
     * Activate walk animation in the animator by tweaking the isMoving boolean.
     */
    public void ActivateWalkAnimation(bool isWalking)
    {
        string currentAnimationName = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        if (isWalking && currentAnimationName == GameConstants.CLIP_MEI_UP)
        {
            _animator.StopPlayback();
        }
        _animator.SetBool(GameConstants.STATEBOOL_MOVE, isWalking);
    }

    public void ActivateInteractingAnimation()
    {
        _animator.SetBool(GameConstants.STATEBOOL_INTERACT, true);
    }

    /**
     * Deactivates the current interaction animation.
     * Then, forces the animator to go back to the idle state.
     * This fixes the problem where the interaction animation is still playing even when the player is able to move again.
     */
    public void DeactivateInteractingAnimation()
    {
        _animator.SetBool(GameConstants.STATEBOOL_INTERACT, false);
        GoIdleAnimation();
    }

    public void GoIdleAnimation()
    {
        _animator.Play(GameConstants.CLIP_MEI_IDLE);
    }
}
