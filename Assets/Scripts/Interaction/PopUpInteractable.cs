using System.Collections;
using UnityEngine;

/**
 * Attach this script to any interactables that has a pop up image.
 * Remember to attach the correct sprite to be shown in the pop up.
 * Remember to attach the pop up canvas (which has the PopUpActivator component).
 */
public class PopUpInteractable : Interactable
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private PopUpActivator _activator;

    public override IEnumerator Interact()
    {
        if (_activator != null && _sprite != null)
        {
            yield return StartCoroutine(_activator.ActivatePopUp(_sprite));
        }
    }
}
