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
    private PlayerController _player;

    private void Awake()
    {
        _popUpCanvas = gameObject;
        _image = GetComponentInChildren<Image>();
        _player = FindAnyObjectByType<PlayerController>();
        ClosePopUp();
    }

    public IEnumerator ActivatePopUp(Sprite sprite)
    {
        _player.StopPlayerMovement();
        SetPopUpImage(sprite);
        OpenPopUp();
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        ClosePopUp();
        yield return new WaitForSeconds(0.5f);
        _player.ResumePlayerMovement();
        
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
