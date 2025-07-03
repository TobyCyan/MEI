using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHearingDistanceCheck : MonoBehaviour
{
    public GameObject PlayerTarget { get; set; }
    private HearingEnemy _enemy;
    private PlayerController _player;

    private void Awake()
    {
        PlayerTarget = GameObject.FindGameObjectWithTag("Player");
        _player = PlayerTarget.GetComponent<PlayerController>();

        _enemy = GetComponentInParent<HearingEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerTarget && _player.IsWalkingSoundPlaying())
        {
            _enemy.SetWalkingSoundHeardStatus(true);
            Debug.Log("Player walking sound heard by enemy");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_enemy.EnemyStateMachine.CurrentEnemyState == _enemy.IdleState) // Avoid Hearing Check Triggers during other states
        {
            if (collision.gameObject == PlayerTarget && _player.IsWalkingSoundPlaying())
            {
                _enemy.SetWalkingSoundHeardStatus(true);
                Debug.Log("Player walking sound still heard by enemy while in hearing range");
            }
            else if (collision.gameObject == PlayerTarget && !_player.IsWalkingSoundPlaying())
            {
                _enemy.SetWalkingSoundHeardStatus(false);
                Debug.Log("Player walking sound not heard by enemy anymore, but still in hearing range");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerTarget)
        {
            _enemy.SetWalkingSoundHeardStatus(false);
            Debug.Log("Player exited hearing range, enemy no longer hears walking sound");
        }
    }
}
