using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * IMPORTANT: Attach this onto any interactable object and specify the order of interactables.
 * Manager for managing the order of which the interactable scripts will be called.
 */
[RequireComponent(typeof(BoxCollider2D))]
public class InteractionManager : MonoBehaviour
{
    [HideInInspector] public bool CanUseItem { get; private set; }
    [SerializeField] private GameObject _interactionIcon;
    [SerializeField] private List<Interactable> _interactables = new List<Interactable>();
    private ItemInteractable _itemInteractable;

    private void Start()
    {
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
        var player = PlayerController.Instance;
        player.StopPlayerMovement();

        foreach (Interactable interactable in _interactables)
        {
            yield return StartCoroutine(interactable.Interact());
            yield return new WaitForSeconds(0.3f);
        }

        player.ResumePlayerMovement();
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
        if (player.FocusedInteracable == this)
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
}
