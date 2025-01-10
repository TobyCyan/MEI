using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            return;
        }

        Instance = this;
    }
    #endregion

    public delegate void OnItemChanged();
    public event OnItemChanged OnItemChangedCallback;

    // The inventory has a fixed size for now. This should be changed if the player is required to have
    // more than 8 items at any point of time.
    public int size = 8;
    public List<Item> items = new List<Item>();
    private List<Item> _pickedUpItems = new List<Item>();

    /** <summary>
        Adds an item to the current player inventory AND the list of picked up items for tracking.
        </summary>
    */
    public bool Add(Item item)
    {
        if (items.Count >= size || item == null)
        {
            return false;
        }

        items.Add(item);
        _pickedUpItems.Add(item);
        OnItemChangedCallback?.Invoke();
        return true;
    }

    /** <summary>
        Removes an item from the current player inventory only.
        </summary>
    */
    public void Remove(Item item)
    {
        items.Remove(item);

        OnItemChangedCallback?.Invoke();
    }

    /** <summary>
        Checks if the current player inventory has the item.
        If not, which means it may have been used, then check the list of picked up items instead.
        If both lists do not contain the item, it means that this item was never picked up before.
        </summary>
    */
    public bool Contains(Item item)
    {
        return items.Contains(item) || _pickedUpItems.Contains(item);
    }
}
