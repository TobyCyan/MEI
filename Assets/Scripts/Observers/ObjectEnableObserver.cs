/**
 * Enables the game object this is attached to.
 * This component assumes that the game object starts off disabled.
 */
public class ObjectEnableObserver : Observer
{
    private void Start()
    {
        DisableObject();
    }

    public override void UpdateSelf()
    {
        gameObject.SetActive(true);
    }

    protected void DisableObject()
    {
        gameObject.SetActive(false);
    }
}
