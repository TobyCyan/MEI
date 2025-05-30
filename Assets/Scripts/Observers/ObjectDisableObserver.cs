public class ObjectDisableObserver : Observer
{
    public override void UpdateSelf()
    {
        gameObject.SetActive(false);
    }
}
