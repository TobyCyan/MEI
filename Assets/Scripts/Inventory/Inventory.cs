using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    public GameObject InventoryMenu;
    private bool inventoryActivated = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            Debug.Log("I is pressed.");
            if (!inventoryActivated)
            {
                InventoryMenu.SetActive(true);
                inventoryActivated = true;
            }
            else
            {
                InventoryMenu.SetActive(false);
                inventoryActivated = false;
            }
        }
    }

}
