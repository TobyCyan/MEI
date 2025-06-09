using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(DialogueInteractable))]
[RequireComponent(typeof(SfxPlayer))]
public class LockedInteractable : ItemInteractable
{ 
    private bool _isLocked = true;
    private bool _isFirstTimeEnter = true;
    [SerializeField] private DialogueInteractable _lockedDialogue;
    [SerializeField] private AudioClip _lockedAudioClip;
    [Header("Leave The Followings Empty If This Object Cannot Be Unlocked.")]
    [SerializeField] private DialogueInteractable _unlockedDialogue;
    [SerializeField] private AudioClip _unlockedAudioClip;
    [SerializeField] private Item _unlockItem;
    [Header("Dependent Door Can Be Nothing.")]
    [SerializeField] private LockedInteractable _dependentLockedInteractable;

    /** Unique IDs Saved Are SceneName + the Given Unique ID. **/
    private string _uniqueID;

    // Other components.
    [SerializeField] private SceneTransition _sceneTransition;
    private SfxPlayer _sfxPlayer;

    private void Start()
    {
        // GameObject names in the same scene are unique.
        _uniqueID = SceneManager.GetActiveScene().name + gameObject.name;

        // Check with the game manager to see if this door or its dependent door have been unlocked before.
        // E.g. if the front door is unlocked, then the back door should be unlocked since the front and back door are dependent on each other.
        // Only locked interactables are expected to be locked and have this behavior.
        bool isDependentDoorUnlocked = _dependentLockedInteractable ? _dependentLockedInteractable.IsUnlocked() : false;
        if (IsUnlocked() || isDependentDoorUnlocked)
        {
            _isLocked = _isFirstTimeEnter = false;
        }
        _sfxPlayer = GetComponent<SfxPlayer>();
    }

    public override IEnumerator Interact()
    {
        // Door is locked.
        if (_isLocked)
        {
            _sfxPlayer.PlaySfx(_lockedAudioClip);
            yield return StartCoroutine(_lockedDialogue.Interact());
            yield break;
        }

        // Door is unlocked.
        if (_isFirstTimeEnter)
        {
            // If unlocked, play dialogue and enter the first time.
            yield return StartCoroutine(EnterFirstTime());
        }
        else
        {
            // If entered before, just transition.
            yield return StartCoroutine(_sceneTransition.Interact());
        }

        yield break;
    }

    private IEnumerator EnterFirstTime()
    {
        yield return StartCoroutine(_unlockedDialogue.Interact());
        yield return StartCoroutine(_sceneTransition.Interact());
        _isFirstTimeEnter = false;
    }

    public override IEnumerator UseItem(Item item)
    {
        if (_isLocked && item.Equals(_unlockItem))
        {
            _sfxPlayer.PlaySfx(_unlockedAudioClip);
            _isLocked = false;
            GameManager.Instance.AddUnlockedDoor(_uniqueID);
            Inventory.Instance.Remove(item);
        }
        yield break;
    }

    public bool IsUnlocked()
    {
        return GameManager.Instance.IsDoorUnlocked(_uniqueID);
    }
}
