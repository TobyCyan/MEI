using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Attach this script to any interactable that has dialogue on it.
 * The dialogue text list should be customizable to every dialogue interactables' needs.
 */
[RequireComponent(typeof(DialogueActivator))]
public class DialogueInteractable : Interactable
{
    [SerializeField] private List<DialogueInfoStruct> _dialogueTextEmotionStructList;
    private DialogueActivator _activator;
    private PlayerController _player;

    private void Awake()
    {
        _activator = GetComponent<DialogueActivator>();
        _player = PlayerController.Instance;
    }

    public override IEnumerator Interact()
    {
        // Calling activate dialogue using the dialogue _activator.
        if (_player != null)
        {
            yield return StartCoroutine(_activator.ActivateDialogue(_dialogueTextEmotionStructList));
        }
    }

}
