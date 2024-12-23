using UnityEngine;

/** <summary>
    An inventory condition that checks whether an item exists in player's inventory.
    </summary>
 */
public class InventoryCondition : MonoBehaviour, ICondition
{
    [SerializeField] private Item m_Item;

    private Inventory m_Inventory;
    
    void Start()
    {
        m_Inventory = Inventory.Instance;
    }

    public bool CheckCond()
    {
        if (m_Item == null)
        {
            return true;
        }

        return m_Inventory.Contains(m_Item);
    }
}
