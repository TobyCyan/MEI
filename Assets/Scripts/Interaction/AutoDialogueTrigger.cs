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
    private DialogueActivator _activator;
    private bool _hasInteracted;

    private void Start()
    {
        _activator = GetComponent<DialogueActivator>();
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        bool isPlayerEnter = collision.CompareTag("Player");
        if (!_hasInteracted && isPlayerEnter)
        {
            yield return StartCoroutine(_activator.ActivateDialogue());
            _hasInteracted = true;
        }
    }


}
