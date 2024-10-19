using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private PlayerInput playerInput;
    public int weight = 0;
    private Vector2 boxSize = new Vector2(0.1f, 1f);
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float airWalkSpeed = 3f;
    [SerializeField] bool exit;
    [SerializeField] private bool _isMoving = false;
    [SerializeField] private bool _isRunning = false;
    [SerializeField] TouchingDirection touchingDirection;
    public bool _isFacingRight = true;
    public float jumpImpulse = 8.5f;



    public bool IsMoving
    {
        get { return _isMoving; }
        private set
        {
            _isMoving = value;
            animator.SetBool("isMoving", value);
        }
    }

    public int Weight
    {
        get { return weight; }
        private set { weight = value; }
    }

    public float moveSpeed
    {
        get
        {
            if (IsMoving && !touchingDirection.IsOnWall)
            {
                if (touchingDirection.IsGrounded)
                {
                    if (IsRunning)
                    {
                        return runSpeed;
                    }
                    else
                    {
                        return walkSpeed;
                    }
                }
                else if (!touchingDirection.IsGrounded)
                {
                    return airWalkSpeed;
                }
            }
            return 0;
        }
    }




    public bool IsFacingRight
    {
        get
        {
            return _isFacingRight;
        }
        set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }

    }


    public bool IsRunning
    {
        get { return _isRunning; }
        private set
        {
            _isRunning = value;
            animator?.SetBool("isRunning", value);
        }
    }

    private void Awake()
    {


        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirection = GetComponent<TouchingDirection>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void FixedUpdate()
    {
        float moveVelocity = moveSpeed * Time.deltaTime;
        rb.velocity = new Vector2(moveInput.x * moveVelocity, rb.velocity.y);
        animator.SetFloat("yVelocity", rb.velocity.y);

    }

    public void stopInput()
    {
        playerInput.enabled = false;
    }

    public void startInput()
    {
        playerInput.enabled = true;
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero;
        SetFacingDirection(moveInput);
    }

    public bool GetExit()
    {
        return exit;
    }

    public void SetExit(bool value)
    {
        exit = value;
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirection.IsGrounded)
        {
            animator.SetTrigger("jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    public void OpenInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            CheckInteraction();
        }
    }

    public void CheckInteraction()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, boxSize, 0, Vector2.zero);

        if (hits.Length > 0)
        {
            foreach (RaycastHit2D rc in hits)
            {
                Interactable interactable = rc.transform.GetComponent<Interactable>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
    }

}
