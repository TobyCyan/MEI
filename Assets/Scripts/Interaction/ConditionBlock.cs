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
public class ConditionBlock : InteractionStateReporter
{
    private ICondition[] _conditions;

    [Header("To specify conditions to check for, just add the ICondition components to this game object.")]
    [SerializeField] private bool _isSatisfied = false;
    [SerializeField] private List<DialogueInfoStruct> _dialogueTextEmotionStructList;
    [SerializeField] private DialogueActivator _activator;
    [SerializeField] private GDTFadeEffect _fadeEffect;

    private float _halfWidth;

    private void Awake()
    {
        Initialize();
        _isSatisfied = _isInteracted;
    }

    private void Start()
    {
        _activator = GetComponent<DialogueActivator>();
        _halfWidth = GetComponent<BoxCollider2D>().size.x / 2;
        _conditions = GetComponents<ICondition>();
    }

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if (_isSatisfied)
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
            _isSatisfied = _isInteracted = true;
            MarkReporter();
        }
        else
        {
            player.StopPlayerMovement();
            yield return StartCoroutine(_activator.ActivateDialogue(_dialogueTextEmotionStructList));
            MovePlayerBack(player);
            player.ResumePlayerMovement();
        }
    }

    private bool CheckConditions()
    {
        foreach (ICondition condition in _conditions)
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
        Vector3 playerPos = player.transform.position;
        bool isPlayerLeft = (playerPos.x - gameObject.transform.position.x) <= 0;

        PerformTransition();

        if (isPlayerLeft)
        {
            player.transform.position = new Vector3(playerPos.x - _halfWidth, playerPos.y, playerPos.z);
        }
        else
        {
            player.transform.position = new Vector3(playerPos.x + _halfWidth, playerPos.y, playerPos.z);
        }

        // Reset the target position so that player does not keep moving towards the previous target.
        player.SetTarget(player.transform.position);
    }

    /**
     * Performs transition to smoothen the process of moving the player away.
     */
    private void PerformTransition()
    {
        _fadeEffect.pingPong = true;
        _fadeEffect.firstColor = Color.clear;
        _fadeEffect.lastColor = Color.black;
        _fadeEffect.disableWhenFinish = true;

        _fadeEffect.gameObject.SetActive(true);
    }
}
