using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Attach this script to any interactable that has dialogue on it.
 * The dialogue text list should be customizable to every dialogue interactables' needs.
 */
public class DialogueInteractable : Interactable
{
    [SerializeField] private bool _isInteracted = false;
    [SerializeField] private List<DialogueInfoStruct> _dialogueTextEmotionStructList;
    private DialogueActivator _activator;
    private PlayerController _player;

    private void Awake()
    {
        _activator = GetComponent<DialogueActivator>();
        _player = FindAnyObjectByType<PlayerController>();
    }

    public override IEnumerator Interact()
    {
        // Calling activate dialogue using the dialogue _activator.
        if (!_isInteracted && _player != null)
        {
            yield return StartCoroutine(_activator.ActivateDialogue(_dialogueTextEmotionStructList));
            // Dialogue won't pop up anymore after interaction.
            _isInteracted = true;
        }
    }

}
