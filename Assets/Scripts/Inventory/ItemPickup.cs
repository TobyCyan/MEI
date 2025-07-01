using System.Collections;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
    [SerializeField] private bool _isDestroyAfterPickUp = true;
    private bool _isPickedUp = false;

    // Components.
    [SerializeField] private SfxPlayer _sfxPlayer;
    private AudioClip _pickUpSfx;
    private BoxCollider2D _collider;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _pickUpSfx = Resources.Load<AudioClip>(
            GameConstants.RESOURCEPATH_SFX_UI
            + "PopUp/SFX_PickUp"
            );

        _isPickedUp = Inventory.Instance.Contains(item);
        if (!_isPickedUp)
        {
            return;
        }

        // Disable the script if already in inventory or picked up before.
        enabled = false;

        // Disable game object if it is set to be destroyed after picking up.
        if (!_isDestroyAfterPickUp)
        {
            return;
        }
        // gameObject.SetActive(false);
        if (_spriteRenderer != null)
        {
            _spriteRenderer.enabled = false;
        }

        _collider.enabled = false;
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
        _sfxPlayer.PlaySfx(_pickUpSfx);

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
