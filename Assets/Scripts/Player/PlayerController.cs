using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Vector3 _target;
    private bool _isActive = true;
    private bool _isInteracting = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = ScenePlayerInfo.scenePlayerPosition;
        _target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Pause any controls when player is inactive.
        if (!_isActive || _isInteracting)
        {
            return;
        }

        CheckForMouseInput();
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

        MoveToMousePosition();
    }

    private void MoveToMousePosition()
    {
        // Move towards _target position
        transform.position = Vector3.MoveTowards(transform.position, _target, Time.deltaTime * speed);
    }

    private IEnumerator OnTriggerStay2D(Collider2D collider)
    {
        // Checks for interactable objects upon collision stay.
        // This prevents the raycast from being called if it is not colliding with an interactable.
        Interactable[] interactables = collider.gameObject.GetComponents<Interactable>();
        if (_isInteracting || interactables.Length == 0)
        {
            // Use yield break to terminate the coroutine entirely.
            yield break;
        }

        // Shoot out ray from mouse position and check if there is an interactable.
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hit = Physics2D.RaycastAll(mousePos, Vector2.zero, Mathf.Infinity);
        bool isInteractableHit = hit.Length > 0 && hit.Any(obj => obj.collider.GetComponent<Interactable>() != null);

        if (isInteractableHit && Input.GetMouseButton(0))
        {
            _isInteracting = true;
            // Interact if there's an interactable object and player left clicks on it.
            foreach (Interactable interactable in interactables)
            {
                yield return StartCoroutine(interactable.Interact());
            }
            _isInteracting = false;
        }
    }
    
    public void StopPlayerMovement()
    {
        _isActive = false;
    }
    
    public void ResumePlayerMovement()
    {
        _isActive = true;
    }
}
