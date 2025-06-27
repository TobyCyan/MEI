using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoverTextManager : MonoBehaviour
{
    public static HoverTextManager Instance { get; private set; }

    [SerializeField] private GameObject hoverTextPanel;
    [SerializeField] private TextMeshProUGUI hoverText;
    [SerializeField] private Canvas canvas;
    [SerializeField] private DialogueUI dialogueUI; // Assign this in the Inspector
    [SerializeField] private TMP_Text dialogueLabel;

    private RectTransform rectTransform;
    private Camera cam;
    private Coroutine dialogueCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        cam = Camera.main;
        if (hoverTextPanel != null)
        {
            rectTransform = hoverTextPanel.GetComponent<RectTransform>();
            hoverTextPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Show hover text and optionally show an inner thought dialogue.
    /// </summary>
    public void ShowText(string message, Transform worldTarget, string innerThought = "")
    {
        if (hoverTextPanel == null || hoverText == null)
            return;

        hoverText.text = message;
        hoverTextPanel.SetActive(true);

        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(cam, worldTarget.position);
        screenPos.y -= 30f;
        rectTransform.position = screenPos;

        // Handle inner thought dialogue
        if (!string.IsNullOrEmpty(innerThought) && dialogueUI != null)
        {
            List<DialogueInfoStruct> dialogue = new List<DialogueInfoStruct>()
            {
                new DialogueInfoStruct
                {
                    character = CharacterEnum.Character.None,
                    emotion = EmotionEnum.Emotion.None,
                    text = innerThought
                }
            };

            if (dialogueCoroutine != null)
                StopCoroutine(dialogueCoroutine);

            dialogueCoroutine = StartCoroutine(dialogueUI.RunDialogue(dialogue));
        }
    }

    /// <summary>
    /// Hide hover text and stop any ongoing dialogue.
    /// </summary>
    public void HideText()
    {
        if (hoverTextPanel != null)
            hoverTextPanel.SetActive(false);

        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
            dialogueCoroutine = null;
        }

        if (dialogueUI != null)
            dialogueUI.CloseDialogue(dialogueLabel);
    }
}
