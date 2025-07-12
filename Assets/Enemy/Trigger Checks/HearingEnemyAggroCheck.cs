using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearingEnemyAggroCheck : MonoBehaviour
{
    public GameObject PlayerTarget { get; set; }
    private HearingEnemy _hearingEnemy;

    private void Awake()
    {
        PlayerTarget = GameObject.FindGameObjectWithTag("Player");

        _hearingEnemy = GetComponentInParent<HearingEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerTarget && _hearingEnemy.IsWalkingSoundHeard)
        {
            _hearingEnemy.SetAggroStatus(true);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_hearingEnemy.EnemyStateMachine.CurrentEnemyState == _hearingEnemy.IdleState)
        {
            if (collision.gameObject == PlayerTarget && _hearingEnemy.IsWalkingSoundHeard)
            {
                _hearingEnemy.SetAggroStatus(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerTarget && !_hearingEnemy.IsWalkingSoundHeard)
        {
            _hearingEnemy.SetAggroStatus(false);
        }

    }
}
