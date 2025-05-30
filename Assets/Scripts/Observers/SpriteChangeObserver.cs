using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteChangeObserver : Observer
{
    [SerializeField] private Sprite _newSprite;

    public override void UpdateSelf()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = _newSprite;
    }
}
