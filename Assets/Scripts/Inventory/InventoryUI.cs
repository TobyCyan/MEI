using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private Inventory inventory;

    // Only show the inventory UI after the player first picks up an item
    private bool isShown = false;

    void Start()
    {
        inventory = Inventory.Instance;
        inventory.OnItemChangedCallback += UpdateUI;
    }

    
    void Update()
    {
        
    }

    private void UpdateUI()
    {
        Debug.Log("Updating inventory ui");
        if (!isShown)
        {
            isShown = true;

        }
    }
}
