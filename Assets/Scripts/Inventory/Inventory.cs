using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    public GameObject inventoryMenu;
    private bool inventoryActivated = false;
    public InventoryManager inventoryManager;   

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            Debug.Log("I is pressed");
            if (!inventoryActivated)
            {
                inventoryMenu.SetActive(true);
                inventoryActivated = true;
                inventoryManager.ListItems();
            }
            else
            {
                inventoryMenu.SetActive(false);
                inventoryActivated = false;
                inventoryManager.DestroyItems();
            }
        }
    }

}
