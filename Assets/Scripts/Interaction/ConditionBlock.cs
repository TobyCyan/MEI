using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Conditional blocker to check if the player has already satisfied the given conditions.
 * Conditions could involve checking for specific items in the inventory and/ or adding a new "state" to the player singleton.
 * All conditions implement the ICondition interface.
 */
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(DialogueActivator))]
public class ConditionBlock : MonoBehaviour
{
    [SerializeField] private ICondition[] m_Conditions;

    [Header("To specify conditions to check for, just add the ICondition components to this game object.")]
    [SerializeField] private bool m_IsSatisfied = false;
    [SerializeField] private List<DialogueInfoStruct> m_DialogueTextEmotionStructList;
    [SerializeField] private DialogueActivator m_Activator;
    [SerializeField] private GDTFadeEffect m_FadeEffect;

    private float m_HalfWidth;

    private void Awake()
    {
        // Might remove this if there are too many condition blocks in the game.
        // Re-Checking if all conditions are satisfied may be better for performance in this case.
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        m_Activator = GetComponent<DialogueActivator>();
        m_HalfWidth = GetComponent<BoxCollider2D>().size.x / 2;
        m_Conditions = GetComponents<ICondition>();
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

        /**
         * Check if conditions are satisfied.
         * If satisfied, set the field as true.
         * If not satisfied, trigger a dialogue and move player backwards
         */
        bool areConditionsSatisfied = CheckConditions();
        
        if (areConditionsSatisfied)
        {
            m_IsSatisfied = true;
        }
        else
        {
            player.StopPlayerMovement();
            yield return StartCoroutine(m_Activator.ActivateDialogue(m_DialogueTextEmotionStructList));
            MovePlayerBack(player);
            player.ResumePlayerMovement();
        }
    }

    private bool CheckConditions()
    {
        foreach (ICondition condition in m_Conditions)
        {
            bool isSatisfied = condition.CheckCond();
            if (!isSatisfied)
            {
                return false;
            }
        }
        return true;
    }

    /** <summary>
        Checks if the player entered the condition block from the left or right.
        Then moves the player accordingly.
        </summary>
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

        // Reset the target position so that player does not keep moving towards the previous target.
        player.SetTarget(player.transform.position);
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
