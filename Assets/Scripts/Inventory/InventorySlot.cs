using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    private Item _item;

    public void AddItem(Item item)
    {
        _item = item;
        icon.sprite = _item.icon;
        icon.enabled = true;
    }

    public void RemoveItem()
    {
        _item = null;
        icon.sprite = null;
        icon.enabled = false;
    }
}
