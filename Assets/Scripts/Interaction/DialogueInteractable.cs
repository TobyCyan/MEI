using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

/**
 * Attach this script to any interactable that has dialogue on it.
 * The dialogue text list should be customizable to every dialogue interactables' needs.
 */
[RequireComponent(typeof(DialogueActivator))]
public class DialogueInteractable : Interactable
{
    [SerializeField] private List<DialogueInfoStruct> _dialogueTextEmotionStructList;
    private DialogueActivator _activator;

    private void Awake()
    {
        _activator = GetComponent<DialogueActivator>();
    }

    public override IEnumerator Interact()
    {
        // Calling activate dialogue using the dialogue _activator.
        yield return StartCoroutine(_activator.ActivateDialogue(_dialogueTextEmotionStructList));
    }

}
