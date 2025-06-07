using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoverTextTrigger : MonoBehaviour
{
    [SerializeField] private string message = "Press E to inspect";
    [SerializeField] private string InnerThought = "[Category A....]";
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private TMP_Text dialogueLabel;
    private Boolean isClicked = false;
    private Boolean isInteractable = false;


    private void Update()
    {
        if (isInteractable)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
        }
    }
    private List<DialogueInfoStruct> CreateInnerThought(string thoughtText)
    {
        return new List<DialogueInfoStruct>()
    {
        new DialogueInfoStruct
        {
            character = CharacterEnum.Character.None, // Hide name
            emotion = EmotionEnum.Emotion.None,       // Hide portrait
            text = thoughtText
        }
    };
    }

    private void OnMouseOver()
    {
        if (!isClicked)
        {
            if (Input.GetMouseButtonDown(1))
            {
                isClicked = true;
                isInteractable = true;
                string innerThought = InnerThought;  
                List<DialogueInfoStruct> innerThoughtDialogue = CreateInnerThought(innerThought);
                StartCoroutine(dialogueUI.RunDialogue(innerThoughtDialogue));
                HoverTextManager.Instance.ShowText(message, transform);
            }
        }
    }

    private void OnMouseExit()
    {
        isClicked = false;
        isInteractable = false;
        HoverTextManager.Instance.HideText();
        dialogueUI.CloseDialogue(dialogueLabel); // Overload version with no parameter needed

    }

    public void Interact()
    {
        // Show camera message
        NotificationManager.Instance.ShowNotification("You have obtained a book.");
    }
}
