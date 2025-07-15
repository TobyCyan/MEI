using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase-Chasing Direct To Player", menuName = "Enemy Logic/Chase Logic/Chasing Direct To Player")]

public class EnemyChaseDirectToPlayer : EnemyChaseSOBase
{
    private PlayerController _player;
    private HearingEnemy _enemy;

    private Vector3 _targetPos;
    private Vector3 _direction;

    [SerializeField] private float _chaseMoveSpeed = 4f;

    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        if (_enemy.IsWithinAttackingDistance)
        {
            _enemy.EnemyStateMachine.ChangeState(_enemy.AttackState);
        }

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

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
    }

    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _enemy = enemy as HearingEnemy;
    }

    public override void ResetValues()
    {
        base.ResetValues();

        _enemy.SetChasingAnimatorBool(false);
    }
}
