using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHearingDistanceCheck : MonoBehaviour
{
    public GameObject PlayerTarget { get; set; }
    private HearingEnemy _hearingEnemy;
    private PlayerController _player;

    private void Awake()
    {
        PlayerTarget = GameObject.FindGameObjectWithTag("Player");
        _player = PlayerTarget.GetComponent<PlayerController>();

        _hearingEnemy = GetComponentInParent<HearingEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerTarget && _player.IsWalkingSoundPlaying())
        {
            _hearingEnemy.SetWalkingSoundHeardStatus(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_hearingEnemy.EnemyStateMachine.CurrentEnemyState == _hearingEnemy.IdleState) // Avoid Hearing Check Triggers during other states
        {
            if (collision.gameObject == PlayerTarget && _player.IsWalkingSoundPlaying())
            {
                _hearingEnemy.SetWalkingSoundHeardStatus(true);
            }
            else if (collision.gameObject == PlayerTarget && !_player.IsWalkingSoundPlaying())
            {
                _hearingEnemy.SetWalkingSoundHeardStatus(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerTarget)
        {
            _hearingEnemy.SetWalkingSoundHeardStatus(false);
        }
    }
}
