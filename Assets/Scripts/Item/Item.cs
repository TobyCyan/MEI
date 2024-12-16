using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public class Item : ScriptableObject
{
    new public string name = "default name";
    [TextArea] public string description = "default description";

    /* Sprite that is displayed in the inventory slot. */
    public Sprite icon = null;

    /* Sprite that is displayed when player inspects the item from the inventory. */
    public Sprite detailedSprite = null;

}
