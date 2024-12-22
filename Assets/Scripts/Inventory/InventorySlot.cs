using DG.Tweening;
using System.Collections;
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

    [SerializeField] private Image draggedItem;
    [SerializeField] private Color defaultIconColor;
    [SerializeField] private Color overNonInteractableColor;

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
        draggedItem.sprite = icon.sprite;
        draggedItem.rectTransform.localScale = icon.rectTransform.localScale;
        draggedItem.gameObject.SetActive(true);
        icon.enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        draggedItem.transform.position = mousePos;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        InteractionManager interactableAtMousePos = PlayerController.GetInteractableAtPosition(mouseWorldPos);
        if (interactableAtMousePos != null && interactableAtMousePos.CanUseItem)
        {
            draggedItem.color = defaultIconColor;
        }
        else
        {
            draggedItem.color = overNonInteractableColor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        StartCoroutine(DraggedItemDisableAnim());
        icon.enabled = true;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        PlayerController.Instance.UseItemOn(_item, PlayerController.GetInteractableAtPosition(mouseWorldPos));
    }

    private IEnumerator DraggedItemDisableAnim()
    {
        Vector3 initScale = draggedItem.rectTransform.localScale;
        draggedItem.rectTransform.DOScale(initScale + new Vector3(0.25f, 0.25f), 0.25f);
        draggedItem.DOFade(0, 0.25f);
        yield return new WaitForSeconds(0.25f);

        draggedItem.gameObject.SetActive(false);
    }
}
