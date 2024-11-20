using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Automatically trigger the given dialogue when the player enters.
 * Remember to attach the DialogueActivator.
 */
[RequireComponent(typeof(BoxCollider2D))]
public class AutoDialogueTrigger : MonoBehaviour
{
    [SerializeField] private List<DialogueInfoStruct> _dialogueTextEmotionStructList;
    private DialogueActivator _activator;
    private bool _hasInteracted;

    private void Start()
    {
        _activator = GetComponent<DialogueActivator>();
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (!_hasInteracted && player != null)
        {
            player.StopPlayerMovement();
            yield return StartCoroutine(_activator.ActivateDialogue(_dialogueTextEmotionStructList));
            player.ResumePlayerMovement();
            _hasInteracted = true;
        }
    }


}
