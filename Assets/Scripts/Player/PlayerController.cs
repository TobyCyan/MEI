using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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

    [HideInInspector] public InteractionManager FocusedInteracable {  get; private set; }
    [SerializeField] private AudioSource _walkingAudio;
    [SerializeField] private float speed;
    [SerializeField] private Dictionary<PlayerState.State, bool> _playerStates = new Dictionary<PlayerState.State, bool>();
    private bool _isWalking = false;
    private Vector3 _target;
    private bool _isActive = true;

    // Update is called once per frame
    void Update()
    {
        // Pause any controls when player is inactive.
        // If the mouse pointer is over a UI element, don't change target
        if (_isActive && !EventSystem.current.IsPointerOverGameObject())
        {
            UpdateTarget();
        }
        
        MoveToTarget();
        PlayWalkSound();
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
        _target = transform.position;
        ResumePlayerMovement();
    }

    private void UpdateTarget()
    {
        Mouse mouse = Mouse.current;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            // Get the position of the mouse cursor
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(mouse.position.value);
            SetTarget(new Vector3(mousePos.x, _target.y, _target.z));
            SetFocus(GetInteractableAtPosition(mousePos));
        }
    }

    public void SetTarget(Vector3 target)
    {
        _target = target;
    }

    /** <summary>
     * Returns the InteractionManager at the specified position,
     * or null if there is no InteractionManager at that position.
    </summary> */
    private InteractionManager GetInteractableAtPosition(Vector2 position)
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
        FocusedInteracable = interactionManager;
    }

    public void RemoveFocus()
    {
        FocusedInteracable = null;
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
}
