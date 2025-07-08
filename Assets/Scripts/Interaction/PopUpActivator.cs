using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/**
 * Attach this script to the PopUpCanvas.
 */
public class PopUpActivator : MonoBehaviour
{
    private GameObject _popUpCanvas;
    [SerializeField] private Image _image;
    private bool _isExpanded = false;

    private void Awake()
    {
        _popUpCanvas = gameObject;
        ClosePopUp();
    }

    public IEnumerator ActivatePopUp(Sprite sprite)
    {
        SetPopUpImage(sprite);
        OpenPopUp();
        yield return new WaitUntil(() => !_isExpanded);  
    }

    private void SetPopUpImage(Sprite image)
    {
        _image.sprite = image;
    }

    private void OpenPopUp()
    {
        _popUpCanvas.SetActive(true);
        _isExpanded = true;
    }

    public void ClosePopUp()
    {
        _popUpCanvas.SetActive(false);
        _isExpanded = false;
    }
}
