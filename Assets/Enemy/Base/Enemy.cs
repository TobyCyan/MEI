using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemyMoveable, ITriggerCheckable

{
    public Rigidbody2D RB { get; set; }
    public bool IsFacingRight { get; set; } = true;
    public bool IsAggroed { get; set; }
    public bool IsWithinAttackingDistance { get; set; }
    [SerializeField] private Animator _animator;

    #region State Machine Variables

    public EnemyStateMachine EnemyStateMachine { get; set; }
    public EnemyIdleState IdleState { get; set; }
    public EnemyChaseState ChaseState { get; set; }
    public EnemyAttackState AttackState { get; set; }

    #endregion

    #region ScriptableObject Variables

    [SerializeField] private EnemyIdleSOBase EnemyIdleBase;
    [SerializeField] private EnemyChaseSOBase EnemyChaseBase;
    [SerializeField] private EnemyAttackSOBase EnemyAttackBase;

    public EnemyIdleSOBase EnemyIdleBaseInstance { get; set; }
    public EnemyChaseSOBase EnemyChaseBaseInstance { get; set; }
    public EnemyAttackSOBase EnemyAttackBaseInstance { get; set; }

    #endregion

    protected virtual void Awake()
    {
        EnemyIdleBaseInstance = Instantiate(EnemyIdleBase);
        EnemyChaseBaseInstance = Instantiate(EnemyChaseBase);
        EnemyAttackBaseInstance = Instantiate(EnemyAttackBase);

        EnemyStateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, EnemyStateMachine);
        ChaseState = new EnemyChaseState(this, EnemyStateMachine);
        AttackState = new EnemyAttackState(this, EnemyStateMachine);
    }

    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();

        EnemyIdleBaseInstance.Initialize(gameObject, this);
        EnemyChaseBaseInstance.Initialize(gameObject, this);
        EnemyAttackBaseInstance.Initialize(gameObject, this);

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

    #region Trigger Check Functions

    public void SetAggroStatus(bool isAggroed)
    {
        IsAggroed = isAggroed;
    }
    
    public void SetAttackingDistanceBool(bool isWithinAttackingDistance)
    {
        IsWithinAttackingDistance = isWithinAttackingDistance;
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

    #region Animator Bool Setters

    public void SetWalkingAnimatorBool(bool isWalking)
    {
        _animator.SetBool("IsWalking", isWalking);
    }

    public void SetChasingAnimatorBool(bool isChasing)
    {
        _animator.SetBool("IsChasing", isChasing);
    }

    #endregion
}
