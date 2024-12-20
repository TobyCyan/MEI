using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InteractionManager))]
public abstract class Interactable : MonoBehaviour
{
    public virtual IEnumerator Interact()
    {
        Debug.Log("Interacting with " + gameObject.name);
        yield return null;
    }
}
