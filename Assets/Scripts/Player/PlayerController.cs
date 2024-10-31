using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Vector2 _target;
    private bool _isActive = true;

    // Start is called before the first frame update
    void Start()
    {
        _target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Pause any controls when player is inactive.
        if (!_isActive)
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
            _target = new Vector2(mousePos.x, transform.position.y);
        }

        MoveToMousePosition();
    }

    private void MoveToMousePosition()
    {
        // Move towards _target position
        transform.position = Vector2.MoveTowards(transform.position, _target, Time.deltaTime * speed);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        // Checks for interactable objects upon collision stay.
        // This prevents the raycast from being called if it is not colliding with an interactable.
        Interactable interactable = collider.gameObject.GetComponent<Interactable>();
        if (interactable == null)
        {
            return;
        }

        // Shoot out ray from mouse position and check if there is an interactable.
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hit = Physics2D.RaycastAll(mousePos, Vector2.zero, Mathf.Infinity);
        bool isInteractableHit = hit.Length > 0 && hit.Any(obj => obj.collider.GetComponent<Interactable>() != null);
/*        Debug.Log("is hit? " + (bool) hit);
        Debug.Log("is it interactable? " + isInteractableHit);*/
        if (isInteractableHit && Input.GetMouseButton(0))
        {
            // Interact if there's an interactable object and player left clicks on it.
            interactable.Interact();
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
