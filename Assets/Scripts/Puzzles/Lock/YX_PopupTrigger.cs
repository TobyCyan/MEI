using UnityEngine;

public class YX_PopupTrigger : MonoBehaviour
{
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryTriggerPopup();
        }
    }

    private void TryTriggerPopup()
    {
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 clickPosition = new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);

        RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);
        if (hit.collider == null)
        {
            Debug.Log("No object hit.");
            return;
        }

        Debug.Log("Hit object: " + hit.collider.name);

        if (hit.collider.TryGetComponent<IPopupDescribable>(out var describable))
        {
            Debug.Log("Calling showPopUp() on: " + hit.collider.name);
            describable.ShowPopUp();
        }
    }
}
