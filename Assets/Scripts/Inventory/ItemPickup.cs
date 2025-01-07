using System.Collections;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
    [SerializeField] private bool m_IsDestroyAfterPickUp = true;
    private bool m_IsPickedUp = false;
    private BoxCollider2D m_Collider;
    private SpriteRenderer m_SpriteRenderer;

    private void Start()
    {
        m_Collider = GetComponent<BoxCollider2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

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
            // Instead of destroying, disable the collider and sprite renderer so that the interaction manager still runs.
            if (m_Collider != null && m_SpriteRenderer != null)
            {
                m_Collider.enabled = false;
                m_SpriteRenderer.enabled = false;
            }
        }
    }

}
