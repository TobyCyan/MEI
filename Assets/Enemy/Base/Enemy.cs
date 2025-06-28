using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemyMoveable

{
    public Rigidbody2D RB { get; set; }
    public bool IsFacingRight { get; set; } = true;

    #region State Machine Variables

    public EnemyStateMachine EnemyStateMachine { get; set; }
    public EnemyIdleState IdleState { get; set; }
    public EnemyChaseState ChaseState { get; set; }
    public EnemyAttackState AttackState { get; set; }

    #endregion

    private void Awake()
    {
        EnemyStateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, EnemyStateMachine);
        ChaseState = new EnemyChaseState(this, EnemyStateMachine);
        AttackState = new EnemyAttackState(this, EnemyStateMachine);
    }

    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        EnemyStateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        EnemyStateMachine.CurrentEnemyState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        EnemyStateMachine.CurrentEnemyState.PhysicsUpdate();
    }

    #region Movement Functions
    public void MoveEnemy(Vector2 velocity)
    {
        RB.velocity = velocity;
        CheckForLeftOrRightFacing(velocity);
    }

    public void CheckForLeftOrRightFacing(Vector2 velocity)
    {
        if (IsFacingRight && velocity.x < 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
        }
        else if (!IsFacingRight && velocity.x > 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
        }
    }

    #endregion 

    #region Animation Triggers

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        EnemyStateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
    }
    public enum AnimationTriggerType
    {
        EnemyAlerted,
        PlayFootstepSound
    }
    #endregion
}
