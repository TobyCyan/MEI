using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Vector3 _target;
    private bool _isActive { set; get; }
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
        if (!_isActive || _isInteracting)
        {
            return;
        }
        CheckForInteractableClick();
        CheckForMouseInput();      
        MoveToMousePosition();
    }

    private void CheckForMouseInput()
    {
        // Get the position of the mouse cursor
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // If left clicked, update x-coord of _target to x-coord of mousePos
        if (Input.GetMouseButton(0))
        {
            _target = new Vector3(mousePos.x, _target.y, _target.z);
        }
    }

    private void MoveToMousePosition()
    {
        // Move towards _target position
        transform.position = Vector3.MoveTowards(transform.position, _target, Time.deltaTime * speed);
    }

    private void CheckForInteractableClick()
    {
        // Shoot out ray from mouse position and check if there is an interactable.
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity);

        if (hit.collider == null)
        {
            return;
        }

        // Checks for interaction manager.
        // This prevents the raycast from being called if it is not colliding with an interactable.
        InteractionManager interactionManager = hit.collider.gameObject.GetComponent<InteractionManager>();
        bool isInteractableHit = interactionManager != null;
        if (isInteractableHit && Input.GetMouseButton(0))
        {
            _target = new Vector3(mousePos.x, _target.y, _target.z);
            // Move towards the interactable if there's an interactable object and player left clicks on it.
            StartCoroutine(GoToInteractableAndInteract(interactionManager));
        }
    }

    private IEnumerator GoToInteractableAndInteract(InteractionManager manager)
    {
        // Set player status as interacting the moment player decides to move towards the interactable.
        _isInteracting = true;
        while (transform.position != _target)
        {
            // Wait for movement to target.
            MoveToMousePosition();
            yield return null;
        }
        yield return StartCoroutine(manager.GoThroughInteractions());
        _isInteracting = false;
    }
    
    public void StopPlayerMovement()
    {
        _isActive = false;
    }
    
    public void ResumePlayerMovement()
    {
        _isActive = true;
    }

    public bool IsPlayerActive()
    {
        return _isActive;
    }
}
