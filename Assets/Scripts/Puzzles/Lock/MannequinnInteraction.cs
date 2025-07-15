using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.SceneManagement;

public class MannequinInteraction : MonoBehaviour
{
    #region Singleton
    public static MannequinInteraction Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

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

    [Header("Dialogue")]
    [SerializeField] private List<DialogueInfoStruct> _dialogueInfo; // Set this in Inspector for the specific dialogue
    [SerializeField] private InteractionManager Interactor;
    [SerializeField] private NotificationManager _notificationManager;

    //[SerializeField] private SceneTransition scenePlayer;

    private bool _hasTriggered = false;
    private int _clickCount = 0;
    private bool allFiveEquipped = false;
    private SpecialMannequin[] _selectedMannequins = new SpecialMannequin[2];

    private void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            _inventory.Add(item);
        }
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1)) return;

        Vector3 mouseWorldPos = _cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 clickPosition = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero, Mathf.Infinity);

        if (hit.collider == null) return;

        GameObject clickedObject = hit.collider.gameObject;

        if (Input.GetMouseButtonDown(0))
        {
            HandleLeftClick(clickedObject);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            HandleRightClick(clickedObject);
        }
    }

    private void HandleLeftClick(GameObject clickedObject)
    {
        if (!clickedObject.TryGetComponent(out SpecialMannequin mannequin)) return;

        if (AllMannequinsEquipped())
        {
            if (_clickCount < 2)
            {
                _selectedMannequins[_clickCount] = mannequin;
                _clickCount++;

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
        if (AllMannequinsEquipped())
        {
            CheckResult();
        }
    }

    private void SwapSelectedMannequins()
    {
        var m1 = _selectedMannequins[0];
        var m2 = _selectedMannequins[1];

        if (m1 == null || m2 == null) return;

        StartCoroutine(SwapPositions(m1.transform, m2.transform));

        int index1 = System.Array.IndexOf(_lockValue, m1);
        int index2 = System.Array.IndexOf(_lockValue, m2);

        if (index1 != -1 && index2 != -1)
        {
            (_lockValue[index1], _lockValue[index2]) = (_lockValue[index2], _lockValue[index1]);
        }

        _selectedMannequins[0] = null;
        _selectedMannequins[1] = null;
    }

    private IEnumerator SwapPositions(Transform t1, Transform t2)
    {
        float elapsed = 0f;
        float duration = swapDuration;

        float startX1 = t1.position.x;
        float startX2 = t2.position.x;

        Vector3 fixedPos1 = t1.position;
        Vector3 fixedPos2 = t2.position;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            // Interpolate only X-axis
            t1.position = new Vector3(Mathf.Lerp(startX1, startX2, t), fixedPos1.y, fixedPos1.z);
            t2.position = new Vector3(Mathf.Lerp(startX2, startX1, t), fixedPos2.y, fixedPos2.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Final positions (X swapped, YZ unchanged)
        t1.position = new Vector3(startX2, fixedPos1.y, fixedPos1.z);
        t2.position = new Vector3(startX1, fixedPos2.y, fixedPos2.z);
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
        //if (scenePlayer != null)
        //{
        //    Debug.Log("transitioning");
        //    scenePlayer.Interact();
        //    Debug.Log("done");
        //}
        SceneManager.LoadScene(17);
    }

    private bool AllMannequinsEquipped()
    {
        return allFiveEquipped;
    }

    public void DoneEquipped()
    {
        foreach (SpecialMannequin m in _lockValue)
        {
            if (!m.IsEquipped())
            {
                return;
            }
        }

        foreach (SpecialMannequin m in _lockValue)
        {
            m.AllEquippedSetter();
            m.Rise();
        }
        allFiveEquipped = true;
        if (_hasTriggered) return;
        StartCoroutine(Interactor.GoThroughInteractions());
        StartCoroutine(ShowNotificationWithDelay());
    }

    private IEnumerator ShowNotificationWithDelay()
    {
        yield return new WaitForSeconds(7f);

        _notificationManager.ShowNotification("Left-click on two mannequins to swap their positions. Right-click to check the arrangement.");
        Debug.Log("notify");
    }

}
