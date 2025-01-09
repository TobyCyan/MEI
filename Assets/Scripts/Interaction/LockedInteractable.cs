using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DialogueInteractable))]
public class LockedInteractable : ItemInteractable
{
    private bool m_IsLocked = true;
    [SerializeField] private DialogueInteractable m_LockedDialogue;
    [SerializeField] private DialogueInteractable m_UnlockedDialogue;
    [SerializeField] private Item m_UnlockItem;

    private SceneTransition m_SceneTransition;

    private void Start()
    {
        m_SceneTransition = GetComponent<SceneTransition>();
    }

    public override IEnumerator Interact()
    {
        if (m_IsLocked)
        {
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

    public override IEnumerator UseItem(Item item)
    {
        if (m_IsLocked && item.Equals(m_UnlockItem))
        {
            m_IsLocked = false;
            Inventory.Instance.Remove(item);
        }
        yield break;
    }
}
