using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textLabel;

    private GameObject _canvas;
    private TextTyping _textTyping;

    private void Start()
    {
        _textTyping = GetComponent<TextTyping>();
        _canvas = this.gameObject;
        CloseDialogue(textLabel);
        // Testing call, delete later.
        // RunDialogue();
    }

    /**
     * Starts the coroutine to run through each dialogue text in the provided text list.
     */
    public void RunDialogue(List<string> textList)
    {
        OpenDialogue(textLabel);
        StartCoroutine(RunThroughEachDialogue(textList));
    }

    /**
     * Runs each dialogue text from the list one by one,
     * and only proceeds when the player hits mouse left click or space bar.
     */
    private IEnumerator RunThroughEachDialogue(List<string> textList)
    {

        foreach (string text in textList)
        {
            yield return _textTyping.StartDialogue(text, textLabel);

            // Wait for some time before taking another input to move on in case the player prompted to skip current dialogue text.
            yield return new WaitForSeconds(0.25f);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        }

        CloseDialogue(textLabel);
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
