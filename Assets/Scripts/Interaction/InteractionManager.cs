using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

/**
 * IMPORTANT: Attach this onto any interactable object and specify the order of interactables.
 * Manager for managing the order of which the interactable scripts will be called.
 */
[RequireComponent(typeof(BoxCollider2D))]
public class InteractionManager : MonoBehaviour
{
    [SerializeField] private GameObject _interactionIcon;
    [SerializeField] private List<Interactable> _interactables = new List<Interactable>();
    [SerializeField] private PlayerState.State _onCompletePlayerState = PlayerState.State.None;

    private void Start()
    {
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
        
        // Add the new player state after completing the interaction.
        if (_onCompletePlayerState != PlayerState.State.None)
        {
            player.AddPlayerState(_onCompletePlayerState);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (!collision.CompareTag("Player"))
        {
            return;
        }

        if (PlayerController.Instance.FocusedInteracable == this)
        {
            PlayerController.Instance.RemoveFocus();
            StartCoroutine(GoThroughInteractions());
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
}
