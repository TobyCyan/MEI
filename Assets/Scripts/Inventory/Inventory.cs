using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryMenu;
    private bool _inventoryActivated = false;
    private PlayerController _player;
    private InventoryManager _inventoryManager;

    private void Start()
    {
        _player = FindAnyObjectByType<PlayerController>();
        _inventoryManager = GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            Debug.Log("I is pressed");
            if (!_inventoryActivated && _player.IsPlayerActive())
            {
                _inventoryMenu.SetActive(true);
                _inventoryActivated = true;
                _inventoryManager.ListItems();
            }
            else
            {
                _inventoryMenu.SetActive(false);
                _inventoryActivated = false;
                _inventoryManager.DestroyItems();
            }
        }
    }

}
