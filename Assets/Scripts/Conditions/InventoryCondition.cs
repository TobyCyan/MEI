using UnityEngine;

/**
 * An inventory condition that checks whether an item exists in player's inventory.
 */
public class InventoryCondition : MonoBehaviour, ICondition
{
    [SerializeField] private Item m_Item;

    private Inventory m_Inventory = Inventory.Instance;

    public bool CheckCond()
    {
        return m_Inventory.Contains(m_Item);
    }
}
