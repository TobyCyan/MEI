public class MeiAnimationController : AnimationController
{
    private void Start()
    {
        GetAnimatorComponent();
        InitializeFields(GameConstants.CLIP_MEI_UP, GameConstants.CLIP_MEI_IDLE, 
            GameConstants.STATEBOOL_INTERACT, GameConstants.STATEBOOL_MOVE);
    }

}
