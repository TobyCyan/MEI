using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InteractionManager))]
public abstract class Interactable : MonoBehaviour
{
    public abstract IEnumerator Interact();
}
