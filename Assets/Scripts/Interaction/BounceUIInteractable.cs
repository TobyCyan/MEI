using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class BounceUIInteractable : Interactable
{
    [SerializeField] private Bounce _bounce;

    private void Awake()
    {
        Assert.IsNotNull(_bounce, "Bounce Is Not Attached To: " + name + "!");
    }

    public override IEnumerator Interact()
    {
        Assert.IsTrue(_bounce.IsDoOnce(), 
            "Bounce Would Risk Running Into Infinite CallStack: " + name + "!");
        _bounce.ResetBounce();
        yield return new WaitUntil(() => _bounce.HasBounced());
    }
}
