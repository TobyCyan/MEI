using UnityEngine;

public abstract class AnimationController : MonoBehaviour
{
    private Animator _animator;
    protected string _upClip;
    protected string _idleClip;
    protected string _isInteractStateBool;
    protected string _isMoveStateBool;

    protected void GetAnimatorComponent()
    {
        _animator = GetComponent<Animator>();
    }

    protected void InitializeFields(string upClip, string idleClip, 
        string isInteractStateBool, string isMoveStateBool)
    {
        _upClip = upClip;
        _idleClip = idleClip;
        _isInteractStateBool = isInteractStateBool;
        _isMoveStateBool = isMoveStateBool;
    }

    /**
     * Activate walk animation in the animator by tweaking the isMoving boolean.
     */
    public void ActivateWalkAnimation(bool isWalking)
    {
        string currentAnimationName = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        if (isWalking && currentAnimationName == _upClip)
        {
            _animator.StopPlayback();
        }
        _animator.SetBool(_isMoveStateBool, isWalking);
    }

    public void ActivateInteractingAnimation()
    {
        _animator.SetBool(_isInteractStateBool, true);
    }

    /**
     * Deactivates the current interaction animation.
     * Then, forces the animator to go back to the idle state.
     * This fixes the problem where the interaction animation is still playing even when the player is able to move again.
     */
    public void DeactivateInteractingAnimation()
    {
        _animator.SetBool(_isInteractStateBool, false);
        GoIdleAnimation();
    }

    public void GoIdleAnimation()
    {
        _animator.Play(_idleClip);
    }
}
