using System.Collections;
using System.Linq;
using UnityEngine;

/** <summary>
     This class represents an interactable that items can be used on.
     Interactables that require item(s) should inherit this class and override the <c>UseItem</c> method
     AND the <c>Interact</c> method. The <c>Interact</c> method is called right after the <c>UseItem</c> method.
     <br />Each gameObject can have at most one <c>ItemRequiredInteractable</c>.
    </summary>
*/
public abstract class ItemInteractable : Interactable
{
    /** <summary>
         Use an item on this interactable.
        </summary>
    */
    public virtual IEnumerator UseItem(Item item)
    {
        // TODO: Add a check to see if this is the correct item used.
        Debug.Log($"Using {item.name} on {this.name}");
        Inventory.Instance.Remove(item);
        yield break;
    }
}
