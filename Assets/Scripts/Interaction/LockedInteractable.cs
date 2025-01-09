using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DialogueInteractable))]
public class LockedInteractable : ItemInteractable
{ 
    private bool _isLocked = true;
    private bool _isFirstTimeEnter = true;
    [SerializeField] private DialogueInteractable _lockedDialogue;
    [SerializeField] private DialogueInteractable _unlockedDialogue;
    [SerializeField] private Item _unlockItem;

    private SceneTransition _sceneTransition;

    private void Start()
    {
        _sceneTransition = GetComponent<SceneTransition>();

        // Check with the game manager to see if this door has been unlocked before.
        // Only locked interactables are expected to be locked and have this behavior.
        if (GameManager.Instance.isDoorUnlocked(this))
        {
            _isLocked = false;
        }
    }

    public override IEnumerator Interact()
    {
        // Door is locked.
        if (_isLocked)
        {
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
            _isLocked = false;
            Inventory.Instance.Remove(item);
        }
        yield break;
    }
}
