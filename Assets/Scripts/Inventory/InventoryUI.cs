using DG.Tweening;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsContainer;
    private Inventory inventory;
    private InventorySlot[] slots;
    private bool _isOpen = false;

    void Start()
    {
        inventory = Inventory.Instance;
        inventory.OnItemChangedCallback += UpdateUI;
        slots = itemsContainer.GetComponentsInChildren<InventorySlot>();
    }

    private void UpdateUI()
    {
        Debug.Log("Updating inventory ui");

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].RemoveItem();
            }
        }
    }

    public void HandleClick()
    {
        if (_isOpen)
        {
            itemsContainer.DOLocalMove(new Vector3(-520, 0), 0.5f).SetEase(Ease.OutQuint);
        }
        else
        {
            itemsContainer.DOLocalMove(new Vector3(0, 0), 0.5f).SetEase(Ease.OutQuint);
        }
        _isOpen = !_isOpen;
    }
}
