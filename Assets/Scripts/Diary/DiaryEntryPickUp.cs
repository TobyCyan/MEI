using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class DiaryEntryPickUp : Interactable
{
    public PickUpDiaryEntry _pickUpDiaryEntry;
    [SerializeField] private bool _isDestroyAfterPickUp = true;
    private bool _isPickedUp = false;

    // Components.
    [SerializeField] private DiaryManager _diaryManager;
    [SerializeField] private SfxPlayer _sfxPlayer;
    private AudioClip _pickUpSfx;
    private BoxCollider2D _collider;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        Assert.IsNotNull(_diaryManager, "Diary Manager Is Not Attached To " + name + "!");
        Assert.IsNotNull(_pickUpDiaryEntry, "A Pick Up Diary Entry Is Not Attached To " + name + "!");
        _collider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _pickUpSfx = Resources.Load<AudioClip>(
            GameConstants.RESOURCEPATH_SFX_UI 
            + "Diary/SFX_Flip_Page"
            );
    }

    private void Start()
    {
        _isPickedUp = _diaryManager.IsEntryFound(_pickUpDiaryEntry.Id);
        if (!_isPickedUp)
        {
            return;
        }

        // Disable the script if already in inventory or picked up before.
        enabled = false;

        // Disable game object if it is set to be destroyed after picking up.
        if (_isDestroyAfterPickUp)
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

    private void Pickup()
    {
        _isPickedUp = _diaryManager.AddEntry(_pickUpDiaryEntry.Id);
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
