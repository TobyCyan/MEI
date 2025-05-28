using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * IMPORTANT: Attach this onto any interactable object and specify the order of interactables.
 * Manager for managing the order of which the interactable scripts will be called.
 */
[RequireComponent(typeof(BoxCollider2D))]
public class InteractionManager : MonoBehaviour
{
    [HideInInspector] public bool CanUseItem { get; private set; }
    [SerializeField] private GameObject _interactionIcon;
    [SerializeField] private List<Interactable> _interactables = new();
    [SerializeField] private PlayerState.State _onCompletePlayerState = PlayerState.State.None;
    [SerializeField] private bool _isAllowRepeatedInteractions = true;
    [SerializeField] private bool _isInteracted = false;
    [SerializeField] private bool _shouldPlayerLookUp = true;
    [SerializeField] private bool _shouldPlayerBeActiveAfter = true;
    private readonly List<Observer> _observers = new();

    /** Unique IDs Saved Are SceneName + the Given Unique ID. **/
    private string _uniqueID;

    private ItemInteractable _itemInteractable;

    private void Start()
    {
        // GameObject names in the same scene are unique.
        _uniqueID = SceneManager.GetActiveScene().name + gameObject.name;

        // Check if this manager has been interacted before, which will prevent the interaction from happening.
        _isInteracted = GameManager.Instance.IsManagerInteracted(_uniqueID);

        // Gets a list of Item Interactables.
        // Then, get the first ItemInteractable in the list and check if it exists.
        // Assumption: There is only one ItemInteractable in _interactables.
        var itemInteractables = _interactables.Where(interactable => IsItemInteractable(interactable)).ToList();
        if (itemInteractables.Count > 0)
        {
            _itemInteractable = itemInteractables.First() as ItemInteractable;
            CanUseItem = _itemInteractable != null;
        }
        else if (itemInteractables.Count > 1)
        {
            Debug.LogError($"More than one ItemInteractable on {this.name}");
        }

        if (_interactionIcon != null)
        {
            _interactionIcon.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        PlayerController.Instance.ResumePlayerMovement();
    }

    public IEnumerator GoThroughInteractions()
    {
        if (_isInteracted)
        {
            yield break;
        }

        // Makes player idle and transition into interacting animation.
        var player = PlayerController.Instance;
        player.StopPlayerMovement();
        if (_shouldPlayerLookUp)
        {
            player.ActivateInteractingAnimation();
        }

        // Go through the interactables.
        foreach (Interactable interactable in _interactables)
        {
            yield return StartCoroutine(interactable.Interact());
            yield return new WaitForSeconds(0.1f);
        }

        // Go back to idle.
        player.DeactivateInteractingAnimation();

        // Wait a little more to ensure the interaction to ensure the interacting animation is fully deactivated.
        yield return new WaitForSeconds(0.1f);

        if (_shouldPlayerBeActiveAfter)
        {
            player.ResumePlayerMovement();
        }

        // Add the new player state after completing the interaction.
        if (_onCompletePlayerState != PlayerState.State.None)
        {
            player.AddPlayerState(_onCompletePlayerState);
        }

        // Interactions won't happen again if not allowed to.
        if (!_isAllowRepeatedInteractions)
        {
            _isInteracted = true;
            GameManager.Instance.AddInteractedManager(_uniqueID);
        }

        NotifyObservers();
    }

    private void NotifyObservers()
    {
        foreach (var observer in _observers)
        {
            observer.UpdateSelf();
        }
    }

    private IEnumerator UseItem(Item item)
    {
        yield return _itemInteractable.UseItem(item);
        yield return GoThroughInteractions();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        PlayerController player = PlayerController.Instance;

        // TODO Right now the player always interacts at the edge of the interactable collider 2D.
        // Make player move towards the center first, then interact.

        if (player.FocusedInteractable == this)
        {
            PlayerController.Instance.RemoveFocus();
            if (CanUseItem && player.IsUsingItem())
            {
                Item usedItem = player.UsedItem;
                player.StopUsingItem();
                StartCoroutine(UseItem(usedItem));
            }
            else
            {
                StartCoroutine(GoThroughInteractions());
            }
        }
        else if (_interactionIcon != null)
        {
            OpenInterableIcon();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            if (_interactionIcon != null)
            {
                CloseInterableIcon();
            }
        }
    }

    public void OpenInterableIcon()
    {
        if (_interactionIcon != null)
        {
            _interactionIcon.SetActive(true);
        }
    }

    public void CloseInterableIcon()
    {
        if (_interactionIcon != null)
        {
            _interactionIcon.SetActive(false);
        }
    }

    private bool IsItemInteractable(Interactable interactable)
    {
        return interactable.GetType().IsSubclassOf(typeof(ItemInteractable));
    }

    public void AddObserver(Observer observer)
    {
        _observers.Add(observer);
    }
}
