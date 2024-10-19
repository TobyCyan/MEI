using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Vector2 target;

    // Start is called before the first frame update
    void Start()
    {
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the position of the mouse cursor
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // If left clicked, update x-coord of target to x-coord of mousePos
        if (Input.GetMouseButton(0))
        {
            target = new Vector2(mousePos.x, transform.position.y);
        }

        // Move towards target position
        transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * speed);
    }
}
