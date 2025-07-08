using UnityEngine;

public class CanvasForwarder : MonoBehaviour
{
    private Canvas _canvas;

    [SerializeField] private int _forwardSortingOrder = 100;
    private readonly int _defaultSortingOrder = 0;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _canvas.overrideSorting = true;
    }

    private void OnEnable()
    {
        _canvas.sortingOrder = _forwardSortingOrder;
    }

    private void OnDisable()
    {
        _canvas.sortingOrder = _defaultSortingOrder;
    }
}
