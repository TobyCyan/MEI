using System.Collections;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
    [SerializeField] private bool m_IsDestroyAfterPickUp = true;
    private bool m_IsPickedUp = false;

    public override IEnumerator Interact()
    {
        // Stop interaction after item has been picked up, in case forgot to disable repeated interactions.
        if (m_IsPickedUp)
        {
            yield break;
        }

        Pickup();
        yield break;
    }

    void Pickup()
    {
        Debug.Log("Picking up " +  item.name);
        m_IsPickedUp = Inventory.Instance.Add(item);

        // Some objects may not need to be destroyed upon picking up.
        if (m_IsPickedUp && m_IsDestroyAfterPickUp)
        {
            Destroy(gameObject);
        }
    }

}
