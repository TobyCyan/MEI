using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Attach this script to any interactable that has dialogue on it.
 * The dialogue text list should be customizable to every dialogue interactables' needs.
 * Make sure to attach the dialogue canvas object.
 */
public class DialogueActivator : MonoBehaviour
{
    [SerializeField] private Canvas _dialogueCanvas;
    private DialogueUI _dialogueUI;
    private bool isInteractCDOver = true;

    private void Awake()
    {
        _dialogueUI = _dialogueCanvas.GetComponent<DialogueUI>();
    }

    /**
     * Activates the dialogue with the given dialogue text list.
     * This should be called by an dialogue interactable.
     */
    public IEnumerator ActivateDialogue(List<DialogueInfoStruct> dialogueInfo)
    {
        if (_dialogueUI != null && isInteractCDOver)
        {
            // Activate the dialogue cooldown and stopping the player from moving.
            isInteractCDOver = false;

            yield return _dialogueUI.RunDialogue(dialogueInfo);

            // Wait for some time before can interact again.
            yield return new WaitForSeconds(0.5f);
            isInteractCDOver = true;
        }
    }
}
