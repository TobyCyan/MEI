using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item Item;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Pickup()
    {
        if (!InventoryManager.IsInventoryFull())
        {
            InventoryManager.Add(Item);
            Destroy(gameObject);
        }        
    }

    private void OnMouseDown()
    {
        Pickup();
    }
}
