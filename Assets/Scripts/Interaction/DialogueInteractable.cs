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
    [SerializeField] private bool CanCharacterMove = false;
    private DialogueActivator _activator;

    private void Awake()
    {
        _activator = GetComponent<DialogueActivator>();
    }

    public override IEnumerator Interact()
    {
        // Calling activate dialogue using the dialogue _activator.
        //Debug.Log("Interactables entered");
        //yield return StartCoroutine(_activator.ActivateDialogue(_dialogueTextEmotionStructList));
        if (CanCharacterMove)
        {
            yield return StartCoroutine(_activator.ActivateDialogue(_dialogueTextEmotionStructList));

        }
        else
        {
            PlayerController.Instance.StopPlayerMovement();
            yield return StartCoroutine(_activator.ActivateDialogue(_dialogueTextEmotionStructList));
            PlayerController.Instance.ResumePlayerMovement();

        }
    }

}
