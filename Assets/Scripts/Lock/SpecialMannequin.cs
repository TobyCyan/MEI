using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialMannequin : ItemInteractable, IClickable
{
    [SerializeField] private int id;
    [SerializeField] private Sprite completeMannequin;
    [SerializeField] private Sprite halfMannequin;
    [SerializeField] private Item requiredItem;

    private bool isEquipped = false;
    private bool allEquipped = false;

    public bool CheckCombo(int result)
    {
        return id == result;
    }

    public void SwapSprite()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && spriteRenderer.sprite == halfMannequin)
        {
            spriteRenderer.sprite = completeMannequin;
            isEquipped = true;
        }
    }

    public bool IsEquipped()
    {
        return isEquipped;
    }

    // Handle item usage
    public override IEnumerator UseItem(Item item)
    {
        if (isEquipped)
        {
            Debug.Log($"{name} is already equipped.");
            yield break;
        }

        if (item == requiredItem)
        {
            Debug.Log($" Correct item used on {name}");
            SwapSprite();
            Inventory.Instance.Remove(item);
        }
        else
        {
            Debug.Log($" Wrong item used on {name}");
        }

        yield break;
    }

    // Optional interaction logic
    public override IEnumerator Interact()
    {
        Debug.Log($" Interacting with {name}");

        // You can add additional behavior here:
        // - show dialogue
        // - emit particle effect
        // - toggle between half/completed state, etc.

        yield return base.Interact();
    }

    public void allEquippedSetter()
    {
        allEquipped = true;
    }

    public bool HandleClick()
    {
        if (allEquipped)
        {
            return true;
        } 
        return false;

    }
}