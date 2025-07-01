using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class ToggleSceneLighting : MonoBehaviour
{
    [Header("The Color Specified in Light2D Component Will Be the Normal Lighting.")]
    [SerializeField] private Color _darkSceneColor;
    [SerializeField] private float _darkSceneIntensity = 0.6f;
    [SerializeField] private bool _isOnDuringDark = true;
    private Color _normalSceneColor;

    private void Start()
    {
        ToggleSceneLightSettings();
    }

    public void ToggleSceneLightSettings()
    {
        bool isSceneDark = GameManager.Instance.HasTransitionedToDarkScene();
        Light2D light = GetComponent<Light2D>();
        _normalSceneColor = light.color;

        if (!isSceneDark)
        {
            light.color = _normalSceneColor;
            return;
        }

        // Scene is dark.
        if (!_isOnDuringDark)
        {
            light.intensity = 0.0f;
            return;
        }

        // Light is on during dark.
        light.color = _darkSceneColor;
        light.intensity = _darkSceneIntensity;
    }

    public float GetDarkSceneIntensity()
    {
        return _darkSceneIntensity;
    }

}
