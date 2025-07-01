using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private Vector3 _targetPos;
    private Vector3 _direction;
    private PlayerController _player = PlayerController.Instance;
    [field: SerializeField] private float _idleMoveSpeed = 1f;

    public EnemyIdleState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
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

        if (_player.IsWalkingSoundPlaying()) // still need to add trigger checks
        {
            _targetPos = _player.transform.position;
            _direction = (_targetPos - enemy.transform.position).normalized;

            enemy.MoveEnemy(_direction * _idleMoveSpeed);
        }

        if ((enemy.transform.position - _targetPos).sqrMagnitude < 0.01f)
        {
            enemy.MoveEnemy(Vector3.zero);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
