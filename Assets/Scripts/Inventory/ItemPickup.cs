using System.Collections;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
    [SerializeField] private bool _isDestroyAfterPickUp = true;
    private bool _isPickedUp = false;
    private BoxCollider2D _collider;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        // Disable the game object if already in inventory or picked up before, and is set to be destroyed after picking up.
        if (Inventory.Instance.Contains(item) && _isDestroyAfterPickUp)
        {
            gameObject.SetActive(false);
        }
    }

    public override IEnumerator Interact()
    {
        // Stop interaction after item has been picked up, in case forgot to disable repeated interactions.
        if (_isPickedUp)
        {
            yield break;
        }

        Pickup();
        yield break;
    }

    void Pickup()
    {
        _isPickedUp = Inventory.Instance.Add(item);

        // Some objects may not need to be destroyed upon picking up.
        if (_isPickedUp && _isDestroyAfterPickUp)
        {
            // Instead of destroying, disable the collider and sprite renderer so that the interaction manager still runs.
            if (_collider != null && _spriteRenderer != null)
            {
                _collider.enabled = false;
                _spriteRenderer.enabled = false;
            }
        }
    }

}
