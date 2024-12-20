using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Conditional blocker to check if the player has already satisfied the given conditions.
 * Conditions could involve checking for specific items in the inventory and/ or adding a new "state" to the player singleton.
 */
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(DialogueActivator))]
public class ConditionBlock : MonoBehaviour
{
    [SerializeField] private List<string> m_Conditions;
    [SerializeField] private bool m_IsSatisfied = false;
    [SerializeField] private List<DialogueInfoStruct> m_DialogueTextEmotionStructList;
    [SerializeField] private DialogueActivator m_Activator;
    [SerializeField] private GDTFadeEffect m_FadeEffect;

    private float m_HalfWidth;

    private void Start()
    {
        m_Activator = GetComponent<DialogueActivator>();
        m_HalfWidth = GetComponent<BoxCollider2D>().size.x / 2;
    }

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if (m_IsSatisfied)
        {
            yield break;
        }

        PlayerController player = collision.GetComponent<PlayerController>();
        if (player == null)
        {
            yield break;
        }

        // TODO Check if item exists.
        // If satisfied, set the field as true.
        // If not satisfied, trigger a dialogue and move player backwards.
        if (false)
        {

        }
        else
        {
            player.StopPlayerMovement();
            yield return StartCoroutine(m_Activator.ActivateDialogue(m_DialogueTextEmotionStructList));
            MovePlayerBack(player);
            player.ResumePlayerMovement();
        }
    }

    /**
     * Checks if the player entered the condition block from the left or right.
     * Then moves the player accordingly.
     */
    private void MovePlayerBack(PlayerController player)
    {
        bool isPlayerLeft = (player.transform.position.x - gameObject.transform.position.x) <= 0;
        Vector2 playerPos = player.transform.position;
        PerformTransition();
        if (isPlayerLeft)
        {
            player.transform.position = new Vector2(playerPos.x - m_HalfWidth, playerPos.y);
        }
        else
        {
            player.transform.position = new Vector2(playerPos.x + m_HalfWidth, playerPos.y);
        }
    }

    /**
     * Performs transition to smoothen the process of moving the player away.
     */
    private void PerformTransition()
    {
        m_FadeEffect.pingPong = true;
        m_FadeEffect.firstColor = Color.clear;
        m_FadeEffect.lastColor = Color.black;
        m_FadeEffect.disableWhenFinish = true;

        m_FadeEffect.gameObject.SetActive(true);
    }
}
