using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public abstract class Interactable : MonoBehaviour
{
    public abstract void Interact();

    [SerializeField] private GameObject Interaction;

    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void Start()
    {
        if (Interaction != null)
        {
            Interaction.SetActive(false);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            if (Interaction != null)
            {
                OpenInterableIcon();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            if (Interaction != null)
            {
                CloseInterableIcon();
            }
        }
    }

    public void OpenInterableIcon()
    {
        if (Interaction != null)
        {
            Interaction.SetActive(true);
        }
    }

    public void CloseInterableIcon()
    {
        if (Interaction != null)
        {
            Interaction.SetActive(false);
        }
    }
}
