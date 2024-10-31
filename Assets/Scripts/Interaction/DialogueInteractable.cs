/**
 * Attach this script to any interactable that has dialogue on it.
 * Make sure to attach the Dialogue Activator too.
 */
public class DialogueInteractable : Interactable
{
    private DialogueActivator _activator;
    private PlayerController player;

    private void Start()
    {
        _activator = GetComponent<DialogueActivator>();
        player = FindAnyObjectByType<PlayerController>();
    }

    public override void Interact()
    {
        
        // Calling activate dialogue using the dialogue _activator.
        if (player != null && _activator != null)
        {
            StartCoroutine(_activator.ActivateDialogue());
        }
    }
}
