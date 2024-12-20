using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
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

    [SerializeField] private AudioSource _walkingAudio;
    [SerializeField] private float speed;
    private bool _isWalking = false;
    private Vector3 _target;
    private bool _isActive;
    private bool _isInteracting = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = ScenePlayerInfo.scenePlayerPosition;
        _target = transform.position;
        _isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Pause any controls when player is inactive.
        if (!_isActive)
        {
            return;
        }

        // If the mouse pointer is over a UI element, don't change target
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            UpdateTarget();
        }
        
        MoveToTarget();
        PlayWalkSound();
    }

    private void UpdateTarget()
    {
        Mouse mouse = Mouse.current;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            // Get the position of the mouse cursor
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(mouse.position.value);
            _target = new Vector3(mousePos.x, _target.y, _target.z);
            _isInteracting = CheckForInteractable(mousePos);
            Debug.Log($"{_target} {_isInteracting}");
        }
    }

    private bool CheckForInteractable(Vector2 position)
    {
        // Shoot out ray from mouse position and check if there is an interactable.
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity);

        if (hit.collider == null)
        {
            return false;
        }

        // Checks for interaction manager.
        InteractionManager interactionManager = hit.collider.gameObject.GetComponent<InteractionManager>();
        return interactionManager != null;
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

    //private void CheckForInteractableClick()
    //{
    //    // Shoot out ray from mouse position and check if there is an interactable.
    //    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity);

    //    if (hit.collider == null)
    //    {
    //        return;
    //    }

    //    // Checks for interaction manager.
    //    // This prevents the raycast from being called if it is not colliding with an interactable.
    //    InteractionManager interactionManager = hit.collider.gameObject.GetComponent<InteractionManager>();
    //    bool isInteractableHit = interactionManager != null;
    //    if (isInteractableHit && Input.GetMouseButton(0))
    //    {
    //        _target = new Vector3(mousePos.x, _target.y, _target.z);
    //        // Move towards the interactable if there's an interactable object and player left clicks on it.
    //        StartCoroutine(GoToInteractableAndInteract(interactionManager));
    //    }
    //}

    //private IEnumerator GoToInteractableAndInteract(InteractionManager manager)
    //{
    //    // Set player status as interacting the moment player decides to move towards the interactable.
    //    _isInteracting = true;
    //    while (transform.position != _target)
    //    {
    //        // Wait for movement to target.
    //        MoveToTarget();
    //        yield return null;
    //    }
    //    yield return StartCoroutine(manager.GoThroughInteractions());
    //    _isInteracting = false;
    //}

    public void StopPlayerMovement()
    {
        _isActive = false;
        _target = transform.position;
    }
    
    public void ResumePlayerMovement()
    {
        _isActive = true;
    }
}
