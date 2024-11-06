using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class Openable : Interactable
{
    // Start is called before the first frame update

    public Sprite open;
    public Sprite closed;

    private SpriteRenderer sr;
    private bool isOpen;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = closed;
        CloseInterableIcon();
    }

    public override IEnumerator Interact()
    {
        if (isOpen)
        {
            sr.sprite = closed;
        }
        else
        {
            sr.sprite = open;
        }

        isOpen = !isOpen;
        yield return null;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
