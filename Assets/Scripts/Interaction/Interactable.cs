using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Interactable : MonoBehaviour
{
    public abstract void Interact();

    [SerializeField] private GameObject _interaction;

    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void Start()
    {
        if (_interaction != null)
        {
            _interaction.SetActive(false);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            if (_interaction != null)
            {
                OpenInterableIcon();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            if (_interaction != null)
            {
                CloseInterableIcon();
            }
        }
    }

    public void OpenInterableIcon()
    {
        if (_interaction != null)
        {
            _interaction.SetActive(true);
        }
    }

    public void CloseInterableIcon()
    {
        if (_interaction != null)
        {
            _interaction.SetActive(false);
        }
    }
}
