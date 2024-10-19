using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TouchingDirection : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D touchingCol;
    private Animator animator;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.5f;
    public float ceilingDistance = 0.05f;
    public ContactFilter2D castFilter;
    public int switches = 1;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];
    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _isOnWall;

    public bool IsGrounded
    {
        get { return _isGrounded; }
        private set
        {
            _isGrounded = value;
            animator.SetBool("isGrounded", value);
        }
    }

    public bool IsOnWall
    {
        get { return _isOnWall; }
        private set
        {
            _isOnWall = value;
            animator.SetBool("isOnWall", value);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingCol = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        IsOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
    }
}

