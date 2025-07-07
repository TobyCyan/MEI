using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HoverTextManager : MonoBehaviour
{
    public static HoverTextManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private RectTransform inspectPanel; // Panel containing both buttons
    [SerializeField] private Canvas canvas;
    [SerializeField] private TMP_Text dialogueLabel;
    [SerializeField] private DialogueUI dialogueUI;

    [Header("Item References by Book Type")]
    [SerializeField] private Item bookType1Item;
    [SerializeField] private Item bookType2Item;
    [SerializeField] private Item bookType3Item;

    private Camera cam;
    private Coroutine dialogueCoroutine;
    private HoverTextTrigger interactedTrigger = null;
    private string InnerThought;
    private bool occupied = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        cam = Camera.main;

        if (inspectPanel != null)
            inspectPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// Displays the inspect panel once, fixed to the clicked object's position.
    /// </summary>
    public void ShowText(Transform worldTarget, HoverTextTrigger clickedTrigger, string innerThought = "")
    {
        if (interactedTrigger != null || occupied)
        {
            return;
        }
        Debug.Log("showText");
        interactedTrigger = clickedTrigger;
        Debug.Log(interactedTrigger.name);
        InnerThought = innerThought;
        PlayerController.Instance.isActiveSetFalse();
        PlayerController.Instance.SimulateClickToMove(clickedTrigger.transform);
    }

    public void ShowButton(Transform destination)
    {
        if (occupied) return;

        occupied = true;
        StartCoroutine(ShowPanelWithDelay(destination));
    }

    private IEnumerator ShowPanelWithDelay(Transform destination)
    {
        yield return new WaitForSeconds(0.7f);  // Wait for camera to stop

        inspectPanel.gameObject.SetActive(true);

        Vector3 screenPos = Camera.main.WorldToScreenPoint(destination.position);
        screenPos.y -= 150.0f; // Adjust as needed
        inspectPanel.transform.position = screenPos;

        if (!string.IsNullOrEmpty(InnerThought) && dialogueUI != null)
        {
            List<DialogueInfoStruct> dialogue = new List<DialogueInfoStruct>
        {
            new DialogueInfoStruct
            {
                character = CharacterEnum.Character.None,
                emotion = EmotionEnum.Emotion.None,
                text = InnerThought
            }
        };

            if (dialogueCoroutine != null)
                StopCoroutine(dialogueCoroutine);

            dialogueCoroutine = StartCoroutine(dialogueUI.RunDialogue(dialogue));
        }
    }

    /// <summary>
    /// Hides the inspect panel and dialogue.
    /// </summary>
    public void HideText()
    {
        if (inspectPanel != null)
            inspectPanel.gameObject.SetActive(false);

        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
            dialogueCoroutine = null;
        }

        if (dialogueUI != null)
            dialogueUI.CloseDialogue(dialogueLabel);

        occupied = false;
    }

    /// <summary>
    /// Called when the Inspect button is clicked.
    /// </summary>
    public void Inspect()
    {
        if (interactedTrigger == null)
            return;

        NotificationManager.Instance.ShowNotification("You have obtained a book.");

        switch (interactedTrigger.bookTypeGetter())
        {
            case 1:
                if (bookType1Item != null)
                    Inventory.Instance.Add(bookType1Item);
                break;
            case 2:
                if (bookType2Item != null)
                    Inventory.Instance.Add(bookType2Item);
                break;
            case 3:
                if (bookType3Item != null)
                    Inventory.Instance.Add(bookType3Item);
                break;
        }

        interactedTrigger = null;
        HideText();
        PlayerController.Instance.isActiveSetTrue();
    }

    /// <summary>
    /// Called when the Close button is clicked.
    /// </summary>
    public void closeInspect()
    {
        interactedTrigger = null;
        HideText();
        PlayerController.Instance.isActiveSetTrue();
    }
}
