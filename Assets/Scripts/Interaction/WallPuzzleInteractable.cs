using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPuzzleInteractable : ItemInteractable
{
    [SerializeField] private GameObject m_textOnWall;
    [SerializeField] private Item m_flashlight;
    [SerializeField] private DialogueInteractable m_defaultDialogue;
    [SerializeField] private DialogueInteractable m_flashlightUsedDialogue;
    [SerializeField] private DialogueInteractable m_revealedDialogue;
    [SerializeField] private DialogueInteractable m_wrongItemDialogue;
    private bool m_isRevealed = false;
    private bool m_isWrongItemUsed = false;

    private void Start()
    {
        m_textOnWall.SetActive(false);
    }

    public override IEnumerator Interact()
    {
        if (m_isWrongItemUsed)
        {
            yield return StartCoroutine(m_wrongItemDialogue.Interact());
            m_isWrongItemUsed = false;
            yield break;
        }

        if (m_isRevealed)
        {
            yield return StartCoroutine(m_revealedDialogue.Interact());
        }
        else
        {
            yield return StartCoroutine(m_defaultDialogue.Interact());
        }
    }

    public override IEnumerator UseItem(Item item)
    {
        m_isWrongItemUsed = true;
        if (!m_isRevealed && item.Equals(m_flashlight))
        {
            m_textOnWall.SetActive(true);
            m_isRevealed = true;
            yield return StartCoroutine(m_flashlightUsedDialogue.Interact());
            m_isWrongItemUsed = false;
        }
        yield break;
    }
}
