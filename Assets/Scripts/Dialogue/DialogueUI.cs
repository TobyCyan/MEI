using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _dialogueLabel;
    [SerializeField] private TMP_Text _nameLabel;

    private DialogueSpriteManager _dialogueSpriteManager;
    private GameObject _canvas;
    private TextTyping _textTyping;

    private void Awake()
    {
        _dialogueSpriteManager = GetComponent<DialogueSpriteManager>();
        _textTyping = GetComponent<TextTyping>();
        _canvas = gameObject;
        CloseDialogue(_dialogueLabel);
    }

    /**
     * Starts the coroutine to run through each dialogue text in the provided text list.
     * This should be called by an dialogue activator.
     */
    public IEnumerator RunDialogue(List<DialogueInfoStruct> textList)
    {
        OpenDialogue(_dialogueLabel);
        yield return StartCoroutine(RunThroughEachDialogue(textList));
    }

    /**
     * Runs each dialogue text from the list one by one,
     * and only proceeds when the player hits mouse left click or space bar.
     */
    private IEnumerator RunThroughEachDialogue(List<DialogueInfoStruct> textList)
    {

        foreach (DialogueInfoStruct textEmotionPair in textList)
        {
            // Set the dialogue sprite according to the emotion value of the dialogue information struct.
            _dialogueSpriteManager.ActivateDialogueSprite(textEmotionPair.emotion);

            // Set the dialogue name label according to the character value of the dialogue information struct.
            _nameLabel.text = CharacterEnum.CHARACTER_TO_STRING[textEmotionPair.character];

            yield return _textTyping.StartDialogue(textEmotionPair.text, _dialogueLabel);

            // Wait for some time before taking another input to move on in case the player prompted to skip current dialogue text.
            yield return new WaitForSeconds(0.25f);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        }

        CloseDialogue(_dialogueLabel);
    }

    /**
     * Closes the dialogue box by deactivating the canvas.
     */
    public void CloseDialogue(TMP_Text textLabel)
    {
        _textTyping.ClearTextLabel(textLabel);
        _canvas.SetActive(false);
    }

    /**
     * Opens up the dialogue box by activating the canvas.
     */
    public void OpenDialogue(TMP_Text textLabel)
    {
        _textTyping.ClearTextLabel(textLabel);
        _canvas.SetActive(true);
    }
}
