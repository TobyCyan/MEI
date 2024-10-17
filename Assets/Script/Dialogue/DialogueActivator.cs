using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Attach this script to any interactable that has dialogue on it.
 */
public class DialogueActivator : MonoBehaviour
{
    [SerializeField] private List<string> dialogueTextList;
    [SerializeField] private DialogueUI dialogueUI;

    /**
     * Activates the dialogue with the given dialogue text list.
     * TODO: Link to the interaction script to activate dialogue upon interaction.
     */
    public void ActivateDialogue()
    {
        if (dialogueUI != null)
        {
            dialogueUI.RunDialogue(dialogueTextList);
        }
    }
}
