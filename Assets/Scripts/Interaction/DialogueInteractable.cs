using UnityEngine;
using System.Collections;

/**
 * Attach this script to any interactable that has dialogue on it.
 * Make sure to attach the Dialogue Activator too.
 */
public class DialogueInteractable : Interactable
{
    [SerializeField] private bool _isInteracted = false;
    private DialogueActivator _activator;
    private PlayerController _player;

    private void Start()
    {
        _activator = GetComponent<DialogueActivator>();
        _player = FindAnyObjectByType<PlayerController>();
    }

    public override IEnumerator Interact()
    {
        // Calling activate dialogue using the dialogue _activator.
        if (!_isInteracted && _player != null && _activator != null)
        {
            yield return StartCoroutine(_activator.ActivateDialogue());
            // Dialogue won't pop up anymore after interaction.
            _isInteracted = true;
        }
    }
}
