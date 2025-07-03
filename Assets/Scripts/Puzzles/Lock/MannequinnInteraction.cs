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
    private bool _inputEnabled = true;
    private SpecialMannequin[] _selectedMannequins = new SpecialMannequin[2];
    private Plane _interactionPlane = new Plane(Vector3.forward, 0f);

    private void Awake()
    {
        PlayerController.Instance.RemoveFocus();

        for (int i = 0; i < 6; i++)
        {
            _inventory.Add(item);
        }
    }

    private void Update()
    {
        if (!_inputEnabled) return;

        // Handle left click
        if (Input.GetMouseButtonDown(0))
        {
            ProcessClick(true);
        }
        // Handle right click
        else if (Input.GetMouseButtonDown(1))
        {
            ProcessClick(false);
        }
    }

    private void ProcessClick(bool isLeftClick)
    {
        // Capture mouse position immediately on click
        Vector3 clickScreenPos = Input.mousePosition;
        Vector3 worldPosition = GetWorldPosition(clickScreenPos);
        CheckObjectAtPosition(worldPosition, isLeftClick);
    }

    private Vector3 GetWorldPosition(Vector3 screenPosition)
    {
        Ray ray = _cam.ScreenPointToRay(screenPosition);
        float distance;
        if (_interactionPlane.Raycast(ray, out distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            Debug.DrawRay(hitPoint, Vector3.up * 0.5f, Color.green, 1f);
            return hitPoint;
        }

        // Fallback if plane raycast fails
        return _cam.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y,
            Mathf.Abs(_cam.transform.position.z)));
    }

    private void CheckObjectAtPosition(Vector3 worldPosition, bool isLeftClick)
    {
        Vector2 checkPosition = new Vector2(worldPosition.x, worldPosition.y);

        // Visualize the exact detection point
        Debug.DrawRay(checkPosition, Vector3.forward * 0.5f, Color.cyan, 2f);

        RaycastHit2D hit = Physics2D.Raycast(checkPosition, Vector2.zero, Mathf.Infinity);

        if (hit.collider == null)
        {
            Debug.Log("No object detected at click position");
            return;
        }

        GameObject clickedObject = hit.collider.gameObject;
        Debug.Log($"Detected: {clickedObject.name}");

        if (isLeftClick)
        {
            HandleLeftClick(clickedObject);
        }
        else
        {
            HandleRightClick(clickedObject);
        }
    }

    private void HandleLeftClick(GameObject clickedObject)
    {
        if (!clickedObject.TryGetComponent(out SpecialMannequin mannequin)) return;

        if (clickedObject.TryGetComponent(out InteractionManager interactionManager))
        {
            PlayerController.Instance.SetFocus(interactionManager);
            PlayerController.Instance.SetTarget(interactionManager.transform.position);
        }

        if (AllMannequinsEquid())
        {
            if (_clickCount < 2)
            {
                _selectedMannequins[_clickCount] = mannequin;
                _clickCount++;

                Debug.Log($"Selected: {mannequin.name} ({_clickCount}/2)");

                if (_clickCount == 2)
                {
                    SwapSelectedMannequins();
                }
            }
        }
        else if (!_inventoryUI.isOpenGetter())
        {
            _inventoryUI.ToggleUi();
        }
    }

    private void HandleRightClick(GameObject clickedObject)
    {
        if (AllMannequinsEquid() && clickedObject.GetComponent<SpecialMannequin>() != null)
        {
            Debug.Log("Checking combination...");
            CheckResult();
        }
    }

    private void SwapSelectedMannequins()
    {
        SpecialMannequin m1 = _selectedMannequins[0];
        SpecialMannequin m2 = _selectedMannequins[1];

        // Validate selection
        if (m1 == null || m2 == null)
        {
            Debug.LogWarning("Swap failed: One or both mannequins are null");
            ResetSelection();
            return;
        }

        if (m1 == m2)
        {
            Debug.LogWarning("Swap failed: Selected the same mannequin twice");
            ResetSelection();
            return;
        }

        Debug.Log($"Swapping positions: {m1.name} <-> {m2.name}");
        StartCoroutine(PerformSwap(m1.transform, m2.transform));

        // Update array references
        int index1 = System.Array.IndexOf(_lockValue, m1);
        int index2 = System.Array.IndexOf(_lockValue, m2);

        if (index1 != -1 && index2 != -1)
        {
            // Swap positions in array
            (_lockValue[index1], _lockValue[index2]) = (_lockValue[index2], _lockValue[index1]);
        }
        else
        {
            Debug.LogWarning("One or both mannequins not found in registry");
        }
    }

    private IEnumerator PerformSwap(Transform t1, Transform t2)
    {
        _inputEnabled = false;
        Vector3 startPos1 = t1.position;
        Vector3 startPos2 = t2.position;
        float elapsed = 0f;

        while (elapsed < swapDuration)
        {
            float progress = elapsed / swapDuration;
            t1.position = Vector3.Lerp(startPos1, startPos2, progress);
            t2.position = Vector3.Lerp(startPos2, startPos1, progress);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure final positions are exact
        t1.position = startPos2;
        t2.position = startPos1;

        Debug.Log("Swap completed successfully");
        ResetSelection();
        _inputEnabled = true;
    }

    private void ResetSelection()
    {
        _clickCount = 0;
        _selectedMannequins[0] = null;
        _selectedMannequins[1] = null;
    }

    private void CheckResult()
    {
        for (int i = 0; i < _lockValue.Length; i++)
        {
            if (!_lockValue[i].CheckCombo(_correctNumberCombination[i]))
            {
                Debug.Log("Incorrect combination");
                _audioSource2.PlayOneShot(_accessDeniedSfx);
                return;
            }
        }

        Debug.Log("Correct combination!");
        _audioSource.PlayOneShot(_accessGrantedSfx);
    }

    private bool AllMannequinsEquid()
    {
        if (allEquipped) return true;

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