using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Automatically trigger the given dialogue when the player enters.
 */
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(DialogueActivator))]
public class AutoDialogueTrigger : InteractionStateReporter
{
    [SerializeField] private List<DialogueInfoStruct> _dialogueTextEmotionStructList;
    private DialogueActivator _activator;

    private void Start()
    {
        Initialize();

        _activator = GetComponent<DialogueActivator>();
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = PlayerController.Instance;
        if (!_isInteracted && player != null)
        {
            player.StopPlayerMovement();
            yield return StartCoroutine(_activator.ActivateDialogue(_dialogueTextEmotionStructList));
            player.ResumePlayerMovement();
            MarkReporter();
        }
    }


}
