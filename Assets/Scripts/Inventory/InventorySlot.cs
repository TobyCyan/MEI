using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image icon;
    private InventoryUI _inventoryCanvas;
    private Item _item;
    private Button _button;
    private bool _hasItem = false;

    private Vector2 _startPosition;
    [SerializeField] private Color defaultIconColor;
    [SerializeField] private Color overInteractableIconColor;

    private void Awake()
    {
        _inventoryCanvas = FindAnyObjectByType<InventoryUI>();
        _button = GetComponent<Button>();
    }

    public void AddItem(Item item)
    {
        _item = item;
        icon.sprite = _item.icon;
        icon.enabled = true;
        _button.onClick.AddListener(ButtonCallback);
        _hasItem = true;
    }

    public void RemoveItem()
    {
        _item = null;
        icon.sprite = null;
        icon.enabled = false;
        if (_hasItem)
        {
            _button.onClick.RemoveListener(ButtonCallback);
        }
        _hasItem = false;
    }

    private void ButtonCallback()
    {
        _inventoryCanvas.InspectItem(_item);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _startPosition = icon.transform.position;
        icon.transform.SetParent(transform.root);
        icon.transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        icon.transform.position = mousePos;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        if (PlayerController.GetInteractableAtPosition(mouseWorldPos) == null)
        {
            icon.color = overInteractableIconColor;
        }
        else
        {
            icon.color = defaultIconColor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        icon.transform.SetParent(transform);
        icon.transform.position = _startPosition;
        icon.color = defaultIconColor;
    }
}
