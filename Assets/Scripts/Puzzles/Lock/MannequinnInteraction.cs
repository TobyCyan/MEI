using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinnInteraction : MonoBehaviour
{
    [Header("Mannequin Setup")]
    [SerializeField] private SpecialMannequin[] _lockValue;
    [SerializeField] private int[] _correctNumberCombination;

    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _audioSource2;
    [SerializeField] private AudioClip _accessGrantedSfx;
    [SerializeField] private AudioClip _accessDeniedSfx;

    [Header("Swap")]
    public float swapDuration = 0.5f;

    [SerializeField] private Camera _cam;
    [SerializeField] private InventoryUI _inventoryUI;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Item item;
    private int _clickCount = 0;
    private bool allEquipped = false;
    private SpecialMannequin[] _selectedMannequins = new SpecialMannequin[2];

    private void Awake()
    {
        for (int i = 0; i < 6; i ++)
        {
            _inventory.Add(item);
        }
    }

    private void Update()
    {
        // Only run if left or right mouse button is clicked
        if (!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1)) return;

        // Get mouse world position
        Vector3 mouseWorldPos = _cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 clickPosition = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        Debug.Log($"Mouse Screen Pos: {Input.mousePosition} | World Pos: {mouseWorldPos}"); 
        Debug.DrawRay(clickPosition, Vector3.forward * 0.1f, Color.red, 2f);


        // Fire the raycast from mouse position into 2D space
        RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero, Mathf.Infinity);

        Debug.DrawRay(clickPosition, Vector3.forward * 0.1f, Color.red, 2f); // Optional, for debug

        if (hit.collider == null)
        {
            Debug.Log(" No object hit by raycast.");
            return;
        }

        GameObject clickedObject = hit.collider.gameObject;
        Debug.Log(" Raycast hit: " + clickedObject.name);

        // Handle based on mouse button
        if (Input.GetMouseButtonDown(0))
        {
            HandleLeftClick(clickedObject);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            HandleRightClick(clickedObject);
        }
    }


    private Vector2 GetMouseClickPosition()
    {
        Vector3 mouseWorldPos = _cam.ScreenToWorldPoint(Input.mousePosition);
        return new Vector2(mouseWorldPos.x, mouseWorldPos.y);
    }

    private void HandleLeftClick(GameObject clickedObject)
    {
        if (!clickedObject.TryGetComponent(out SpecialMannequin mannequin)) return;

        if (AllMannequinsEquid())
        {
            if (_clickCount < 2)
            {
                _selectedMannequins[_clickCount] = mannequin;
                _clickCount++;

                Debug.Log("Selected: " + mannequin.name);

                if (_clickCount == 2)
                {
                    SwapSelectedMannequins();
                    _clickCount = 0;
                }
            }
        }
        else
        {
            if (!_inventoryUI.isOpenGetter())
            {
                _inventoryUI.ToggleUi();
            }
        }
    }

    private void HandleRightClick(GameObject clickedObject)
    {
        if (AllMannequinsEquid())
        {
            Debug.Log("Right-clicked on Mannequinn padlock.");
            CheckResult();
        }
    }

    private void SwapSelectedMannequins()
    {
        var m1 = _selectedMannequins[0];
        var m2 = _selectedMannequins[1];

        if (m1 == null || m2 == null)
        {
            Debug.LogWarning("Swap aborted: One or both mannequins are null.");
            return;
        }

        Debug.Log($"Swapping: {m1.name} <-> {m2.name}");
        StartCoroutine(SwapPositions(m1.transform, m2.transform));

        int index1 = System.Array.IndexOf(_lockValue, m1);
        int index2 = System.Array.IndexOf(_lockValue, m2);

        if (index1 != -1 && index2 != -1)
        {
            (_lockValue[index1], _lockValue[index2]) = (_lockValue[index2], _lockValue[index1]);
        }
        else
        {
            Debug.LogWarning("One or both mannequins not found in _lockValue array.");
        }

        _selectedMannequins[0] = null;
        _selectedMannequins[1] = null;
    }

    private IEnumerator SwapPositions(Transform t1, Transform t2)
    {
        Vector3 startPos1 = t1.position;
        Vector3 startPos2 = t2.position;
        float elapsed = 0f;

        while (elapsed < swapDuration)
        {
            float t = elapsed / swapDuration;
            t1.position = Vector3.Lerp(startPos1, startPos2, t);
            t2.position = Vector3.Lerp(startPos2, startPos1, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        t1.position = startPos2;
        t2.position = startPos1;

        Debug.Log("Swap completed.");
    }

    private void CheckResult()
    {
        for (int i = 0; i < _lockValue.Length; i++)
        {
            if (!_lockValue[i].CheckCombo(_correctNumberCombination[i]))
            {
                _audioSource2.PlayOneShot(_accessDeniedSfx);
                return;
            }
        }

        _audioSource.PlayOneShot(_accessGrantedSfx);
    }
    private bool AllMannequinsEquid()
    {
        if (allEquipped)
        {
            return true;
        }
        foreach (SpecialMannequin m in _lockValue)
        {
            if (m == null || !m.IsEquipped())
                return false;
        }
        foreach (SpecialMannequin m in _lockValue)
        {
            m.allEquippedSetter();

        }
        allEquipped = true;
        return true;
    }

    public bool AllEquippedGetter()
    {
        return allEquipped;
    }
}

