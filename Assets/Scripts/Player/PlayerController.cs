using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEditor.Animations;
using System;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    #region Singleton
    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
    #endregion

    [HideInInspector] public InteractionManager FocusedInteractable {  get; private set; }
    [HideInInspector] public Item UsedItem { get; private set; }
    [SerializeField] private AudioSource _walkingAudio;
    [SerializeField] private float speed;
    [SerializeField] private Dictionary<PlayerState.State, bool> _playerStates = new();
    [SerializeField] private bool _isFacingRight = true;
    [SerializeField] private float arrivalThreshold = 0.0f;
    private bool _isWalking = false;
    private Vector3 _target;
    private bool _isActive = true;                                           
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Camera _camera;
    private Vector3 destination;
    private bool isOnMission = false;


    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Mathf.Abs(destination.x - transform.position.x);
     
        if (isOnMission && distance > 0f && distance <= 0.5f)
        //if (isOnMission)
        {
            isOnMission = false;
            //_isActive = false;
            Debug.Log("printed");
            HoverTextManager.Instance.ShowButton();
        }
        // Pause any controls when player is inactive.
        // If the mouse pointer is over a UI element, don't change target
        if (_isActive && !EventSystem.current.IsPointerOverGameObject())
        {
            if (HandleWorldClickBlocking())
                return; // Click was handled by a non-move interaction

            UpdateTarget();
            UpdateIsFacingRight();
            FlipSprite();
        }
        
        MoveToTarget();
        PlayWalkSound();
        ActivateWalkAnimation();
    }

    //private IEnumerator DelayedInteraction()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    Debug.Log("printed");
    //    HoverTextManager.Instance.ShowButton();
    //}

    private bool HandleWorldClickBlocking()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return false;

        Vector3 mouseScreenPos = Mouse.current.position.value;
        Vector3 worldPos = GetWorldPositionOnPlane(mouseScreenPos, transform.position.z);
        Vector2 worldPos2D = new Vector2(worldPos.x, worldPos.y);

        Collider2D hit = Physics2D.OverlapPoint(worldPos2D);
        if (hit == null) return false;

        var clickable = hit.GetComponent<IClickable>();
        if (clickable != null && clickable.HandleClick())
        {
            Debug.Log("? Click handled by IClickable. Block player movement.");
            return true;
        }

        return false;
    }


    /**
     * Activate walk animation in the animator by tweaking the isMoving boolean.
     */
    private void ActivateWalkAnimation()
    {
        string currentAnimationName = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        if (_isWalking && currentAnimationName == "Mei Up")
        {
            Debug.Log("Stop playback");
            _animator.StopPlayback();
        }

        _animator.SetBool("isMoving", _isWalking);
    }

    public void ActivateInteractingAnimation()
    {
        _animator.SetBool("isInteracting", true);
    }

    /**
     * Deactivates the current interaction animation.
     * Then, forces the animator to go back to the idle state.
     * This fixes the problem where the interaction animation is still playing even when the player is able to move again.
     */
    public void DeactivateInteractingAnimation()
    {
        _animator.SetBool("isInteracting", false);
        _animator.Play("Mei Idle");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoadedCallback;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoadedCallback;
    }

    /** The callback for whenever the player loads into a new scene.
     * This callback will update the player's position in the new scene.
     * This callback also resets the camera to prevent expensive calls to Camera.main every frame.
     */
    private void SceneLoadedCallback(Scene scene, LoadSceneMode mode)
    {
        transform.position = ScenePlayerInfo.scenePlayerPosition;
        SetCamera(Camera.main);
        _target = transform.position;
        ResumePlayerMovement();
    }

    private void UpdateTarget()
    {
        Mouse mouse = Mouse.current;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            // Vector2 mousePos = _camera.ScreenToWorldPoint(mouse.position.value);

            Vector3 playerPos = transform.position;
            Vector2 mouseScreenPos = mouse.position.value;
            // Get position in world space in perspective projection.
            Vector3 worldPosOnPlane = GetWorldPositionOnPlane(mouseScreenPos, playerPos.z);
            Vector3 targetPos = new Vector3(worldPosOnPlane.x, playerPos.y, playerPos.z);
            UsedItem = null;
            SetTarget(targetPos);
            SetFocus(GetInteractableAtPosition(worldPosOnPlane));
        }
    }

    /**
     * Get World Position using Perspective Projection by Tomer-Barkan.
     * https://discussions.unity.com/t/camera-screentoworldpoint-in-perspective/85521
     */
    Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        // Cast ray from clicked position to the z plane and obtain the point of intersection.
        Ray ray = _camera.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    /**
     * Checks whether the target position is to the right or left of the player.
     * Updates the property accordingly.
     */
    private void UpdateIsFacingRight()
    {
        float xPosDifference = _target.x - transform.position.x;
        if (xPosDifference == 0)
        {
            return;
        }
        _isFacingRight = xPosDifference > 0;
    }

    private void FlipSprite()
    {
        _spriteRenderer.flipX = _isFacingRight;
    }

    public void SetTarget(Vector3 target)
    {
        _target = target;
    }

    /** <summary>
     Returns the InteractionManager at the specified position,
     or null if there is no InteractionManager at that position.
    </summary> */
    public static InteractionManager GetInteractableAtPosition(Vector2 position)
    {
        // Shoot out ray from mouse position and check if there is an interactable.
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity);

        if (hit.collider == null)
        {
            return null;
        }

        return hit.collider.gameObject.GetComponent<InteractionManager>();
    }

    private void MoveToTarget()
    {
        // Move towards _target position
        transform.position = Vector3.MoveTowards(transform.position, _target, Time.deltaTime * speed);

        // Once target is reached, stop walking audio.
        if (transform.position.x == _target.x)
        {
            _isWalking = false;
        } 
        else
        {
            _isWalking = true;
        }
    }

    private void PlayWalkSound()
    {
        if (_isWalking && !_walkingAudio.isPlaying)
        {
            _walkingAudio.Play();
        }
        else if (!_isWalking && _walkingAudio.isPlaying)
        {
            _walkingAudio.Stop();
        }
    }

    public void StopPlayerMovement()
    {
        _isActive = false;
        _target = transform.position;
    }
    
    public void ResumePlayerMovement()
    {
        _isActive = true;
        RemoveFocus();
    }

    public void SetFocus(InteractionManager interactionManager)
    {
        FocusedInteractable = interactionManager;
    }

    public void RemoveFocus()
    {
        FocusedInteractable = null;
    }

    public void UseItemOn(Item item, InteractionManager interactionManager)
    {
        if (interactionManager != null && interactionManager.CanUseItem)
        {
            Vector2 interactionManagerPos = interactionManager.transform.position;
            _target = new Vector3(interactionManagerPos.x, _target.y, _target.z);
            UsedItem = item;
            SetFocus(interactionManager);
        }
    }

    public bool IsUsingItem()
    {
        return UsedItem != null;
    }

    public void StopUsingItem()
    {
        UsedItem = null;
    }

    public bool IsContainState(PlayerState.State state)
    {
        return _playerStates.ContainsKey(state);
    }

    public void AddPlayerState(PlayerState.State state)
    {
        if (!IsContainState(state))
        {
            _playerStates.Add(state, true);
        }
    }

    /** <summary>
        Used to set the player camera every scene load.
        </summary>
    */
    public void SetCamera(Camera camera)
    {
        _camera = camera;
    }

    /// <summary>
    /// Simulates a click on a target position, causing the player to walk to it with all animations and sound.
    /// </summary>
    public bool SimulateClickToMove(Transform targetTransform)
    {

        Vector3 playerPos = transform.position;
        destination = targetTransform.position;

        // Create a target vector that only updates the X position, keeping Y and Z the same
        Vector3 targetPos = new Vector3(destination.x, playerPos.y, playerPos.z);

        UsedItem = null;
        isOnMission = true;
        Debug.Log("simulated");
        SetTarget(targetPos);
        SetFocus(GetInteractableAtPosition(destination)); // optional if clicking on interactable

        UpdateIsFacingRight();
        FlipSprite();
        return true;
    }

    public void isActiveSetTrue()
    {
        _isActive = true;
    }

    public void isActiveSetFalse()
    {
        _isActive = false;
    }
}
