using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * IMPORTANT: Attach this onto any interactable object and specify the order of interactables.
 * Manager for managing the order of which the interactable scripts will be called.
 */
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(ObserverNotifier))]
public class InteractionManager : InteractionStateReporter
{
    [HideInInspector] public bool CanUseItem { get; private set; }
    [SerializeField] private GameObject _interactionIcon;
    [SerializeField] private List<Interactable> _interactables = new();
    [SerializeField] private PlayerState.State _onCompletePlayerState = PlayerState.State.None;
    [SerializeField] private bool _isAllowRepeatedInteractions = true;
    [SerializeField] private bool _shouldPlayerLookUp = true;
    [SerializeField] private bool _shouldPlayerBeActiveAfter = true;
    [SerializeField] private bool _shouldCameraReset = true;
    private ObserverNotifier _observerNotifier;

    private ItemInteractable _itemInteractable;

    private void Start()
    {
        Initialize();

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

        Transform interactionIconTransform = transform.Find("InteractIcon");

        if (interactionIconTransform != null)
        {
            _interactionIcon = interactionIconTransform.gameObject;
            _interactionIcon.SetActive(false);
        }

        _observerNotifier = GetComponent<ObserverNotifier>();
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

        // Interactions won't happen again if not allowed to.
        if (!_isAllowRepeatedInteractions)
        {
            _isInteracted = true;
            MarkReporter();
            CloseInterableIcon();
        }

        // Go back to idle.
        player.DeactivateInteractingAnimation();

        // Wait a little more to ensure the interaction to ensure the interacting animation is fully deactivated.
        yield return new WaitForSeconds(0.1f);

        if (_shouldPlayerBeActiveAfter)
        {
            player.ResumePlayerMovement();
        }

        if (_shouldCameraReset)
        {
            player.ResetCamera();
        }

        // Add the new player state after completing the interaction.
        if (_onCompletePlayerState != PlayerState.State.None)
        {
            player.AddPlayerState(_onCompletePlayerState);
        }

        NotifyObservers();
    }

    private void NotifyObservers()
    {
        _observerNotifier.NotifyObservers();
    }

    private IEnumerator UseItem(Item item)
    {
        yield return _itemInteractable.UseItem(item);
        yield return GoThroughInteractions();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || _isInteracted)
        {
            return;
        }

        if (_interactionIcon != null)
        {
            OpenInterableIcon();
        }

        PlayerController player = PlayerController.Instance;

        if (player.FocusedInteractable != this)
        {
            return;
        }

        Vector3 playerPos = player.transform.position;
        Vector3 centerPoint = new(transform.position.x, playerPos.y, playerPos.z);

        // Makes player move towards the center first, then interact.
        player.SetTarget(centerPoint);

        float centerPointX = centerPoint.x;
        float epsilon = 0.01f;
        if (Mathf.Abs(playerPos.x - centerPointX) > epsilon)
        {
            return;
        }

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
}
