using System.Collections;
using UnityEngine;
using TMPro;

public class TextTyping : MonoBehaviour
{
    [SerializeField] float typingSpeed = 12.0f;

    /** 
     * Starts the dialogue coroutine to type the dialogue in a type writer style.
     */
    public Coroutine StartDialogue(string dialogue, TMP_Text textLabel)
    {
        return StartCoroutine(TypeDialogue(dialogue, textLabel));
    }

    /**
     * Types the dialogue in a type writer style based on the typing speed given.
     */
    private IEnumerator TypeDialogue(string dialogue, TMP_Text textLabel)
    {
        ClearTextLabel(textLabel);
        yield return new WaitForSeconds(0.2f);
        int index = 0;
        float time = 0.0f;
        int dialogueLength = dialogue.Length;

        while (index < dialogue.Length)
        {
            string textSubString = dialogue.Substring(0, index);
            textLabel.text = textSubString;

            // If player prompts to skip current dialogue text, break out and show full text immediately.
            bool isSkipDialogue = CheckForSkipDialogueInput();
            if (isSkipDialogue)
            {
                break;
            }

            time += typingSpeed * Time.deltaTime;
            index = Mathf.FloorToInt(time);
            index = Mathf.Clamp(index, 0, dialogueLength);
            yield return null;
        }

        textLabel.text = dialogue;
    }

    /**
     * Clears the text label box.
     */
    public void ClearTextLabel(TMP_Text textLabel)
    {
        textLabel.text = string.Empty;
    }

    /**
     * Checks for player input to skip the current dialogue text.
     */
    private bool CheckForSkipDialogueInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            return true;
        }

        return false;
    }

}
