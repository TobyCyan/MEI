
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerThoughtTrigger : MonoBehaviour
{
    //[SerializeField] private DialogueActivator _dialogueActivator; // Reference to the DialogueActivator
    [SerializeField] private List<DialogueInfoStruct> _dialogueInfo; // Set this in Inspector for the specific dialogue
    [SerializeField] private DialogueInteractable Interactor;
    private bool _hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collided");
        if (_hasTriggered) return;  // Prevent repeated triggers
        if (!collision.CompareTag("Player")) return;

        _hasTriggered = true;

        //if (_dialogueActivator != null && _dialogueInfo != null && _dialogueInfo.Count > 0)
        //{
        //    StartCoroutine(_dialogueActivator.ActivateDialogue(_dialogueInfo));
        //}
        
        StartCoroutine(Interactor.Interact());
    }
}
