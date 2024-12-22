using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : ItemInteractable
{
    public override IEnumerator Interact()
    {
        return base.Interact();
    }

    public override IEnumerator UseItem(Item item)
    {
        return base.UseItem(item);
    }
}
