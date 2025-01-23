using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ToggleSceneLighting : MonoBehaviour
{
    [Header("The Color Specified in Light2D Component Will Be the Normal Lighting.")]
    [SerializeField] private Color _darkSceneColor;
    private Color _normalSceneColor;

    private void Start()
    {
        bool isSceneDark = GameManager.Instance.HasTransitionedToDarkScene();
        Light2D light = GetComponent<Light2D>();
        _normalSceneColor = light.color;

        if (isSceneDark)
        {
            light.color = _darkSceneColor;
        }
        else
        {
            light.color = _normalSceneColor;
        }
    }

}
