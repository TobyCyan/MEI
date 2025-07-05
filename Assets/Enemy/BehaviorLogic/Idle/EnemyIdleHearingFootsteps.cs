using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Idle-Hearing Footsteps", menuName = "Enemy Logic/Idle Logic/Hearing Footsteps")]

public class EnemyIdleHearingFootsteps : EnemyIdleSOBase
{
    private PlayerController _player;
    private HearingEnemy _hearingEnemy;

    private Vector3 _targetPos;
    private Vector3 _direction;

    [SerializeField] private float _idleMoveSpeed = 1f;

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

        if (_hearingEnemy.IsWalkingSoundHeard)
        {
            _targetPos = _player.transform.position;
            _direction = (_targetPos - _hearingEnemy.transform.position).normalized;

            _hearingEnemy.MoveEnemy(_direction * _idleMoveSpeed);
            _hearingEnemy.SetWalkingAnimatorBool(true);
        }

        if ((_hearingEnemy.transform.position - _targetPos).sqrMagnitude < 0.01f)
        {
            _hearingEnemy.MoveEnemy(Vector3.zero);
            _hearingEnemy.SetWalkingAnimatorBool(false);
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
        _hearingEnemy = enemy as HearingEnemy;
    }

    public override void ResetValues()
    {
        base.ResetValues();

        _hearingEnemy.SetWalkingAnimatorBool(false);
    }
}
