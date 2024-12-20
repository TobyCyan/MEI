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
            Debug.LogWarning("More than one instance of Inventory");
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

    public bool Add(Item item)
    {
        if (items.Count >= size || item == null)
        {
            return false;
        }

        items.Add(item);
        OnItemChangedCallback?.Invoke();
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);

        OnItemChangedCallback?.Invoke();
    }

    public bool Contains(Item item)
    {
        return items.Contains(item);
    }
}
