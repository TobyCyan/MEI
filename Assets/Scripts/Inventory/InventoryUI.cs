using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private GameObject inspectPanel;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private Image itemDetailedImage;

    private Inventory inventory;
    private InventorySlot[] slots;
    private bool _isOpen = false;

    void Start()
    {
        inspectPanel.SetActive(false);

        inventory = Inventory.Instance;
        inventory.OnItemChangedCallback += UpdateUi;
        slots = itemsContainer.GetComponentsInChildren<InventorySlot>();
        UpdateUi();
    }

    void OnDisable()
    {
        inventory.OnItemChangedCallback -= UpdateUi;
    }

    private void UpdateUi()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].RemoveItem();
            }
        }
    }

    public void ToggleUi()
    {
        if (_isOpen)
        {
            itemsContainer.DOLocalMove(new Vector3(-520, 0), 0.5f).SetEase(Ease.OutQuint);
        }
        else
        {
            itemsContainer.DOLocalMove(new Vector3(0, 0), 0.5f).SetEase(Ease.OutQuint);
        }
        _isOpen = !_isOpen;
    }

    public void InspectItem(Item item)
    {
        inspectPanel.SetActive(true);
        itemNameText.text = item.name;
        itemDescriptionText.text = item.description;
        itemDetailedImage.sprite = item.detailedSprite;
    }

    public void CloseInspect()
    {
        inspectPanel.SetActive(false);
    }
}
