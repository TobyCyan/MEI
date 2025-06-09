using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[RequireComponent(typeof(MeiAnimationController))]
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
    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private Dictionary<PlayerState.State, bool> _playerStates = new();
    [SerializeField] private bool _isFacingRight = true;
    private bool _isWalking = false;
    private Vector3 _target;
    private bool _isActive = true;
    private readonly float _epsilon = 0.01f;

    // Components.
    private SpriteRenderer _spriteRenderer;
    private Camera _camera;
    private MeiAnimationController _animationController;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _camera = Camera.main;
        _animationController = GetComponent<MeiAnimationController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Pause any controls when player is inactive.
        // If the mouse pointer is over a UI element, don't change target
        if (_isActive && !EventSystem.current.IsPointerOverGameObject())
        {
            UpdateTarget();
            UpdateIsFacingRight();
            FlipSprite();
        }

        MoveToTarget();
        PlayWalkSound();
        _animationController.ActivateWalkAnimation(_isWalking);
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
        ResetTarget();
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
        if (!_isWalking)
        {
            return;
        }

        float xPosDifference = _target.x - transform.position.x;
        if (Mathf.Abs(xPosDifference) < _epsilon)
        {
            return;
        }
        _isFacingRight = xPosDifference > 0;
    }

    public void SetFacingDirectionToRight()
    {
        SetFacingDirection(true);
    }

    private void SetFacingDirection(bool isRight)
    {
        _isFacingRight = isRight;
        FlipSprite();
    }


    private void FlipSprite()
    {
        _spriteRenderer.flipX = _isFacingRight;
    }

    public void SetTarget(Vector3 target)
    {
        _target = target;
    }

    public void ActivateInteractingAnimation()
    {
        _animationController.ActivateInteractingAnimation();
    }

    public void DeactivateInteractingAnimation()
    {
        _animationController.DeactivateInteractingAnimation();
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
        transform.position = Vector3.MoveTowards(transform.position, _target, Time.deltaTime * _speed);

        // Once target is reached, stop walking audio.
        if (Mathf.Abs(transform.position.x - _target.x) < _epsilon)
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
        ResetTarget();
        RemoveFocus();
    }
    
    public void ResumePlayerMovement()
    {
        _isActive = true;
        RemoveFocus();
    }

    public void SetFocus(InteractionManager interactionManager)
    {
        FocusedInteractable = interactionManager;
        print("interactable: " + FocusedInteractable?.name);
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

    public void ResetCamera()
    {
        _camera.GetComponent<CameraFollow>().ResetCamera();
    }

    public void ResetTarget()
    {
        SetTarget(transform.position);
    }
}
