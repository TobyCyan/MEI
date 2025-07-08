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

    [HideInInspector] public InteractionManager FocusedInteractable { get; private set; }
    [HideInInspector] public Item UsedItem { get; private set; }

    [SerializeField] private AudioSource _walkingAudio;
    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private Dictionary<PlayerState.State, bool> _playerStates = new();
    [SerializeField] private bool _isFacingRight = true;
    [SerializeField] private float destinationDistanceLeft = 0.000f;
    private bool _isWalking = false;
    private Vector3 _target;
    private Vector3 _destination;
    private bool _isActive = true;
    private readonly float _epsilon = 0.01f;
    private bool _isInteracting = false;
    private bool _isOnMission = false;
    private Transform targetInteractableTransform;


    // Components
    private SpriteRenderer _spriteRenderer;
    private Camera _camera;
    private MeiAnimationController _animationController;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _camera = Camera.main;
        _animationController = GetComponent<MeiAnimationController>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoadedCallback;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoadedCallback;
    }

    private void SceneLoadedCallback(Scene scene, LoadSceneMode mode)
    {
        transform.position = ScenePlayerInfo.scenePlayerPosition;
        SetCamera(Camera.main);
        ResetTarget();

        if (ScenePlayerInfo.shouldResumePlayerMovement)
            ResumePlayerMovement();
        else
            StopPlayerMovement();
    }

    private void Update()
    {
        if(_isOnMission && targetInteractableTransform != null)
{
            float distance = Mathf.Abs(transform.position.x - targetInteractableTransform.position.x);
            //Debug.Log(distance);
            if (distance <= destinationDistanceLeft)
            {
                Debug.Log("entered");
                HoverTextManager.Instance.ShowButton(targetInteractableTransform);
                _isOnMission = false;
            }
        }


        if (!_isOnMission && _isActive && !EventSystem.current.IsPointerOverGameObject())
        {
            if (HandleWorldClickBlocking()) return;
            UpdateTarget();
            UpdateIsFacingRight();
            FlipSprite();
        }

        MoveToTarget();
        PlayWalkSound();
        _animationController.ActivateWalkAnimation(_isWalking);
    }


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
            Debug.Log("Click handled by IClickable. Block player movement.");
            return true;
        }

        return false;
    }

    private void UpdateTarget()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        Vector3 playerPos = transform.position;
        Vector2 mouseScreenPos = Mouse.current.position.value;
        Vector3 worldPosOnPlane = GetWorldPositionOnPlane(mouseScreenPos, playerPos.z);
        Vector3 targetPos = new Vector3(worldPosOnPlane.x, playerPos.y, playerPos.z);

        UsedItem = null;
        SetTarget(targetPos);
        SetFocus(GetInteractableAtPosition(worldPosOnPlane));
    }

    Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = _camera.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        xy.Raycast(ray, out float distance);
        return ray.GetPoint(distance);
    }

    private void UpdateIsFacingRight()
    {
        if (!_isWalking || _isInteracting) return;

        float xDiff = _target.x - transform.position.x;
        if (Mathf.Abs(xDiff) < _epsilon) return;

        _isFacingRight = xDiff > 0;
    }


    private void FlipSprite()
    {
        _spriteRenderer.flipX = _isFacingRight;
    }

    private void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target, Time.deltaTime * _speed);
        _isWalking = Mathf.Abs(transform.position.x - _target.x) >= _epsilon;
    }

    private void PlayWalkSound()
    {
        if (_walkingAudio == null) return;

        if (_isWalking && !_walkingAudio.isPlaying)
            _walkingAudio.Play();
        else if (!_isWalking && _walkingAudio.isPlaying)
            _walkingAudio.Stop();
    }

    public void SetTarget(Vector3 target)
    {
        _target = target;
    }

    public void ResetTarget()
    {
        SetTarget(transform.position);
    }

    public static InteractionManager GetInteractableAtPosition(Vector2 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
        return hit.collider?.GetComponent<InteractionManager>();
    }

    public void SetCamera(Camera camera)
    {
        _camera = camera;
    }

    public void ResetCamera()
    {
        _camera?.GetComponent<CameraFollow>()?.ResetCamera();
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
    }

    public void RemoveFocus()
    {
        FocusedInteractable = null;
    }

    public void UseItemOn(Item item, InteractionManager interactionManager)
    {
        if (interactionManager != null && interactionManager.CanUseItem)
        {
            Vector2 pos = interactionManager.transform.position;
            _target = new Vector3(pos.x, _target.y, _target.z);
            UsedItem = item;
            SetFocus(interactionManager);
        }
    }

    public bool IsUsingItem() => UsedItem != null;

    public void StopUsingItem() => UsedItem = null;

    public bool IsContainState(PlayerState.State state) => _playerStates.ContainsKey(state);

    public void AddPlayerState(PlayerState.State state)
    {
        if (!IsContainState(state))
            _playerStates.Add(state, true);
    }

    public void ActivateInteractingAnimation()
    {
        _isInteracting = true;
        _animationController.ActivateInteractingAnimation();
    }

    public void DeactivateInteractingAnimation()
    {
        _isInteracting = false;
        _animationController.DeactivateInteractingAnimation();
    }


    public void SetFacingDirectionToRight() => SetFacingDirection(true);

    public void isActiveSetTrue() => _isActive = true;
    public void isActiveSetFalse() => _isActive = false;


    private void SetFacingDirection(bool isRight)
    {
        _isFacingRight = isRight;
        FlipSprite();
    }

    /// <summary>
    /// Simulates a click on a target position, causing the player to walk to it with animation.
    /// </summary>

    public bool SimulateClickToMove(Transform targetTransform)
    {
        Vector3 playerPos = transform.position;
        _destination = targetTransform.position;
        targetInteractableTransform = targetTransform;

        _isOnMission = true;
        UsedItem = null;
        SetTarget(new Vector3(_destination.x, playerPos.y, playerPos.z));
        SetFocus(GetInteractableAtPosition(_destination));
        UpdateIsFacingRight();
        float xDiff = _target.x - transform.position.x;
        if (Mathf.Abs(xDiff) > _epsilon)
        {
            _isFacingRight = xDiff > 0;
        }
        Debug.Log(_isFacingRight);
        FlipSprite();

        return true;
    }

    //public bool SimulateClickToMove(Transform targetTransform)
    //{
    //    Debug.Log("simulated");
    //    Vector3 playerPos = transform.position;
    //    _destination = targetTransform.position;
    //    Vector3 targetPos = new Vector3(_destination.x, playerPos.y, playerPos.z);

    //    UsedItem = null;
    //    SetTarget(targetPos);
    //    SetFocus(GetInteractableAtPosition(_destination));
    
    //    UpdateIsFacingRight();
    //    Debug.Log(_isFacingRight);
    //    FlipSprite();
    //    return true;
    //}

<<<<<<< HEAD
    public void ResetTarget()
    {
        SetTarget(transform.position);
    }

    #region Functions Needed for CryingMei

    public bool IsWalkingSoundPlaying()
    {
        return _walkingAudio.isPlaying;
    }

    #endregion

=======
>>>>>>> main
}
