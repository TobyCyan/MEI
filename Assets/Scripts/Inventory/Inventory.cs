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

    [SerializeField] private List<Item> _items = new List<Item>();

    void Update()
    {
        
    }

    public void Add(Item item)
    {
        if (item != null)
        {
            _items.Add(item);
        }
    }

    public void Remove(Item item)
    {
        _items.Remove(item);
    }
}
