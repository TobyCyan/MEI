using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

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
    private SceneTransition _sceneTransition;

    private void Start()
    {
        Initialize();

        // Gets a list of Item Interactables.
        // Then, get the first ItemInteractable in the list and check if it exists.
        // Assumption: There is only one ItemInteractable in _interactables.
        var itemInteractables = _interactables.OfType<ItemInteractable>();
        Assert.IsTrue(itemInteractables.Count() <= 1, $"More than one ItemInteractable on { this.name }");
        _itemInteractable = itemInteractables.FirstOrDefault();
        CanUseItem = _itemInteractable != null;

        var sceneTransitions = _interactables.OfType<SceneTransition>();
        Assert.IsTrue(sceneTransitions.Count() <= 1, $"More than one ItemInteractable on {this.name}");
        _sceneTransition = sceneTransitions.FirstOrDefault();
        _interactables.Remove(_sceneTransition);

        Transform interactionIconTransform = transform.Find("InteractIcon");

        if (interactionIconTransform != null)
        {
            _interactionIcon = interactionIconTransform.gameObject;
            _interactionIcon.SetActive(false);
        }

        _observerNotifier = GetComponent<ObserverNotifier>();

        if (_isInteracted)
        {
            NotifyObservers();
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

        // Interactions won't happen again if not allowed to.
        if (!_isAllowRepeatedInteractions)
        {
            _isInteracted = true;
            MarkReporter();
            CloseInterableIcon();
        }

        // Add the new player state after completing the interaction.
        if (_onCompletePlayerState != PlayerState.State.None)
        {
            player.AddPlayerState(_onCompletePlayerState);
        }

        // Scene transition should be executed before resetting or triggering events.
        if (_sceneTransition != null)
        {
            yield return StartCoroutine(_sceneTransition.Interact());
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
}
