using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoverTextTrigger : MonoBehaviour
{
    [SerializeField] private string notificationMessage = "You have obtained a book.";
    [SerializeField] private string innerThought = "[Category A....]";
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private TMP_Text dialogueLabel;
    [SerializeField] private int bookType;

    /// <summary>
    /// Called by HoverTextManager to determine which item to add to inventory.
    /// </summary>
    public int bookTypeGetter()
    {
        return bookType;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("interacted");
            HoverTextManager.Instance.ShowText(transform, this, innerThought);
        }
    }

    /// <summary>
    /// Called when the Inspect button is clicked.
    /// </summary>
    public void Interact()
    {
        NotificationManager.Instance.ShowNotification(notificationMessage);
        gameObject.SetActive(false); // Optional: deactivate after use
    }
}
