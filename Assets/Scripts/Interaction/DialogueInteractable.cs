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

    private void Awake()
    {
        _activator = GetComponent<DialogueActivator>();
    }

    public override IEnumerator Interact()
    {
        PlayerController.Instance.StopPlayerMovement();
        yield return StartCoroutine(_activator.ActivateDialogue(_dialogueTextEmotionStructList));
        PlayerController.Instance.ResumePlayerMovement();
    }
}
