using System.Collections;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;

    public override IEnumerator Interact()
    {
        Pickup();
        yield break;
    }

    void Pickup()
    {
        Debug.Log("Picking up " +  item.name);
        bool isPickedUp = Inventory.Instance.Add(item);

        if (isPickedUp)
        {
            Destroy(gameObject);
        }
    }
}
