using UnityEngine;

/**
 * Simulates fade out effect when entering the scene.
 * Remember to attach the GDTFadeEffect game object.
 */
public class FadeOut : MonoBehaviour
{
    [SerializeField] private GDTFadeEffect _fadeEffect;
    [SerializeField] private bool _playOnEnter = true;
    [SerializeField] private bool _disableWhenFinish = true;

    // Start is called before the first frame update
    void Start()
    {
        if (_playOnEnter)
        {
            ActivateTransition();
        }
    }

    private void ActivateTransition()
    {
        if (_fadeEffect == null)
        {
            return;
        }
        _fadeEffect.disableWhenFinish = _disableWhenFinish;
        _fadeEffect.firstColor = Color.black;
        _fadeEffect.lastColor = Color.clear;
        _fadeEffect.pingPong = false;
        _fadeEffect.gameObject.SetActive(true);
    }
}
