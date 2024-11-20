using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Attach this script to the PopUpCanvas.
 */
public class PopUpActivator : MonoBehaviour
{
    private GameObject _popUpCanvas;
    private Image _image;

    private void Awake()
    {
        _popUpCanvas = gameObject;
        _image = GetComponentInChildren<Image>();
        ClosePopUp();
    }

    public IEnumerator ActivatePopUp(Sprite sprite)
    {
        SetPopUpImage(sprite);
        OpenPopUp();
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        ClosePopUp();     
    }

    private void SetPopUpImage(Sprite image)
    {
        _image.sprite = image;
    }

    private void OpenPopUp()
    {
        _popUpCanvas.SetActive(true);
    }

    private void ClosePopUp()
    {
        _popUpCanvas.SetActive(false);
    }
}
