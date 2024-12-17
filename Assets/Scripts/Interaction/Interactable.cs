using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(InteractionManager))]
public abstract class Interactable : MonoBehaviour
{
    public virtual IEnumerator Interact()
    {
        Debug.Log("Interacting with " + gameObject.name);
        yield return null;
    }

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
