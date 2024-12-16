using System.Collections;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;

    public override IEnumerator Interact()
    {
        base.Interact();
        Pickup();
        yield return null;
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

    private void OnMouseDown()
    {
        Pickup();
    }
}
