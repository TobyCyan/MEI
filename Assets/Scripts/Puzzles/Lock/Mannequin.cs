using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mannequin : ItemInteractable, IClickable
{
    public bool HandleClick()
    {
        // Do nothing, allow movement
        return true;
    }

    public override IEnumerator Interact()
    {
        // Do nothing
        yield break;
    }

    public override IEnumerator UseItem(Item item)
    {
        // Optionally call base method or do nothing
        yield return base.UseItem(item); // or just 'yield break;' if you want to skip base behavior
    }

    public void Rise()
    {
        return;
    }
}