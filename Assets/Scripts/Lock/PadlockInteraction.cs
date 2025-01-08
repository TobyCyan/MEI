using UnityEngine;

public class PadlockInteraction : MonoBehaviour
{
    private Camera _cam;

    private void Awake() => _cam = Camera.main;

    // Update is called once per frame
    private void Update()
    {
        var mousePosition = _cam.ScreenToWorldPoint(Input.mousePosition);
        var hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);

        foreach (var hit in hits)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.TryGetComponent(out PadlockDigitScreen padlockDigitScreen))
                {
                    padlockDigitScreen.AddInput();
                    return; // Stop processing after finding a valid hit
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (hit.collider.TryGetComponent(out Padlock padlock))
                {
                    Debug.Log("correct");
                    padlock.CheckResult();
                    return; // Stop processing after finding a valid hit
                }
            }
        }
    }
}
