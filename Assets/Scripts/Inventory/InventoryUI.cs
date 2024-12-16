using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private Inventory inventory;

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
    }
}
