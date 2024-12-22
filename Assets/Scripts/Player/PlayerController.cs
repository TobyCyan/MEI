using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    [HideInInspector] public Item UsedItem { get; private set; }
    [SerializeField] private AudioSource _walkingAudio;
    [SerializeField] private float speed;
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
            _target = new Vector3(mousePos.x, _target.y, _target.z);
            UsedItem = null;
            SetFocus(GetInteractableAtPosition(mousePos));
        }
    }

    /** <summary>
     * Returns the InteractionManager at the specified position,
     * or null if there is no InteractionManager at that position.
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
        FocusedInteracable = interactionManager;
    }

    public void RemoveFocus()
    {
        FocusedInteracable = null;
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
}
