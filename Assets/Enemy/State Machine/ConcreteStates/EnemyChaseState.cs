using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    private Vector3 _targetPos;
    private Vector3 _direction;
    private PlayerController _player;
    private HearingEnemy _enemy;
    [SerializeField] private float _chaseMoveSpeed = 4f;

    public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
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

        _targetPos = _player.transform.position;
        _direction = (_targetPos - _enemy.transform.position).normalized;

        _enemy.MoveEnemy(_direction * _chaseMoveSpeed);
        _enemy.SetChasingAnimatorBool(true); 

        if ((_enemy.transform.position - _targetPos).sqrMagnitude < 0.01f)
        {
            // GameObject.Destroy(_player.gameObject); 
            // to be determined how to destroy;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
