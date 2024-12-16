using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsContainer;
    private Inventory inventory;
    private InventorySlot[] slots;

    // Only show the inventory UI after the player first picks up an item
    private bool isShown = false;

    void Start()
    {
        inventory = Inventory.Instance;
        inventory.OnItemChangedCallback += UpdateUI;
        slots = itemsContainer.GetComponentsInChildren<InventorySlot>();
    }

    private void UpdateUI()
    {
        Debug.Log("Updating inventory ui");
        if (!isShown)
        {
            isShown = true;
        }

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
}
