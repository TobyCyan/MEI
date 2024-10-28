using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public static List<Item> Items = new List<Item>();

    public Transform ItemContent;
    public GameObject InventoryItem;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Add(Item item)
    {
        Items.Add(item);
    }

    public static void Remove(Item item)
    {
        Items.Remove(item);
    }

    public static bool IsInventoryFull()
    {
        return Items.Count >= 10;
    }

    public void ListItems()
    {
        foreach (var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemIcon = obj.transform.Find("ItemImage").GetComponent<Image>();

            itemIcon.sprite = item.icon;
        }
    }

    public void DestroyItems()
    {
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);   
        }
    }
}
