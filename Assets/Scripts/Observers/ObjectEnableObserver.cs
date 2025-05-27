public class ObjectEnableObserver : Observer
{
    public override void UpdateSelf()
    {
        gameObject.SetActive(true);
    }
}
