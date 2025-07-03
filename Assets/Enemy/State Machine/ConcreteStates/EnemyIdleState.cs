using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private Vector3 _targetPos;
    private Vector3 _direction;
    private PlayerController _player;
    private HearingEnemy _enemy;
    [SerializeField] private float _idleMoveSpeed = 1f;

    public EnemyIdleState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _enemy = enemy as HearingEnemy; 
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // Change from Idle State to Chase State
        if (_enemy.IsAggroed && _enemy.IsWalkingSoundHeard)
        {
            _enemy.EnemyStateMachine.ChangeState(_enemy.ChaseState);
        }

        if (_enemy.IsWalkingSoundHeard) 
        {
            _targetPos = _player.transform.position;
            _direction = (_targetPos - _enemy.transform.position).normalized;

            _enemy.MoveEnemy(_direction * _idleMoveSpeed);
            _enemy.SetWalkingAnimatorBool(true);
        }

        if ((_enemy.transform.position - _targetPos).sqrMagnitude < 0.01f)
        {
            _enemy.MoveEnemy(Vector3.zero);
            _enemy.SetWalkingAnimatorBool(false);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
