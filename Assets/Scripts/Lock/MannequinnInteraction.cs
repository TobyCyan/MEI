using System.Collections;
using UnityEngine;

public class MannequinnInteraction : MonoBehaviour
{
    [SerializeField] private SpecialMannequin[] _lockValue;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _audioSource2;
    [SerializeField] private AudioClip _accessGrantedSfx;
    [SerializeField] private AudioClip _accessDeniedSfx;
    [SerializeField] private int[] _correctNumberCombination;
    private Camera _cam;
    private int click = 0;
    private SpecialMannequin[] specialMannequins = new SpecialMannequin[2];
    public float swapDuration = 0.5f;

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Vector3 mousePosition = _cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 clickPosition = new Vector2(mousePosition.x, mousePosition.y);

            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

            if (hit.collider == null)
            {
                Debug.Log("No object hit.");
                return;
            }

            Debug.Log("Hit object: " + hit.collider.gameObject.name);

            if (Input.GetMouseButtonDown(0))
            {
                if (click < 2 && hit.collider.TryGetComponent(out SpecialMannequin mannequin))
                {
                    Debug.Log("SpecialMannequin detected: " + mannequin.name);
                    specialMannequins[click] = mannequin;
                    click++;

                    if (click == 2)
                    {
                        swap();
                        click = 0;
                    }
                }
                else
                {
                    Debug.LogWarning("Clicked object is not a SpecialMannequin.");
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (hit.collider.TryGetComponent(out SpecialMannequin mannequin))
                {
                    Debug.Log("Right-clicked on Mannequinn padlock.");
                    CheckResult();
                }
            }
        }
    }

    public void swap()
    {
        if (specialMannequins[0] != null && specialMannequins[1] != null)
        {
            Debug.Log($"Swapping: {specialMannequins[0].name} <-> {specialMannequins[1].name}");

            StartCoroutine(SwapPositions(specialMannequins[0].transform, specialMannequins[1].transform));
            
            int index1 = -1;
            int index2 = -1;

            for (int i = 0; i < _lockValue.Length; i++)
            {
                if (_lockValue[i] == specialMannequins[0]) index1 = i;
                if (_lockValue[i] == specialMannequins[1]) index2 = i;
            }

            if (index1 != -1 && index2 != -1)
            {
                var temp = _lockValue[index1];
                _lockValue[index1] = _lockValue[index2];
                _lockValue[index2] = temp;
            }
            else
            {
                Debug.LogWarning("One or both mannequins were not found in _lockValue array.");
            }

            specialMannequins[0] = null;
            specialMannequins[1] = null;
        }
        else
        {
            Debug.LogWarning("Swap aborted: one or both mannequins are null.");
        }
    }


    private IEnumerator SwapPositions(Transform t1, Transform t2)
    {
        Vector3 startPos1 = t1.position;
        Vector3 startPos2 = t2.position;

        Debug.Log($"StartPos1: {startPos1}, StartPos2: {startPos2}");

        float elapsed = 0f;

        while (elapsed < swapDuration)
        {
            t1.position = Vector3.Lerp(startPos1, startPos2, elapsed / swapDuration);
            t2.position = Vector3.Lerp(startPos2, startPos1, elapsed / swapDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        t1.position = startPos2;
        t2.position = startPos1;

        Debug.Log("Swap completed.");
    }

    public void CheckResult()
    {
        bool allCorrect = true;

        for (int i = 0; i < _lockValue.Length; i++)
        {
            if (!_lockValue[i].CheckCombo(_correctNumberCombination[i]))
            {
                allCorrect = false;
                break;
            }
        }

        if (allCorrect)
        {
            _audioSource.PlayOneShot(_accessGrantedSfx);
        }
        else
        {
            _audioSource2.PlayOneShot(_accessDeniedSfx);
        }
    }

}
