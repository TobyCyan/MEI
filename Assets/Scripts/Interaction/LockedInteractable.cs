using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DialogueInteractable))]
public class LockedInteractable : ItemInteractable
{
    [SerializeField] private bool m_IsLocked = true;
    [SerializeField] private DialogueInteractable m_LockedDialogue;
    [SerializeField] private DialogueInteractable m_UnlockedDialogue;
    [SerializeField] private DialogueInteractable m_LockedWithKeyDialogue;

    private SceneTransition m_SceneTransition;

    private void Start()
    {
        m_SceneTransition = GetComponent<SceneTransition>();
    }

    public override IEnumerator Interact()
    {
        if (m_IsLocked)
        {
            // TODO: Add logic here for unlocking the door with key, alongside the dialogue after unlocking, then discarding the key.
            yield return StartCoroutine(m_LockedDialogue.Interact());
        }
        else
        {
            // If unlocked, play dialogue and enter.
            yield return StartCoroutine(m_UnlockedDialogue.Interact());
            yield return StartCoroutine(m_SceneTransition.Interact());
        }
        yield break;
    }


}
