public class DarkSceneTransitionObserver : Observer
{
    public override void UpdateSelf()
    {
        GameManager.Instance.TransitionToDarkScene();
    }
}
