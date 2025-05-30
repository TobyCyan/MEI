using UnityEngine;
using UnityEngine.Assertions;

public class ColliderEnableObserver : Observer
{
    private Collider2D _collider;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider2D>();
        Assert.IsNotNull(_collider, "A Collider Enable Observer "
            + gameObject.name
            + "does not have a collider!");
        _collider.enabled = false;
    }

    public override void UpdateSelf()
    {
        _collider.enabled = true;
    }
}
