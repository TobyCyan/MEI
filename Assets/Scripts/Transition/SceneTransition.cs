using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Attach this script to any door/ interactables that causes a scene transition.
 * Remember to indicate the scene path and the player pos in the next scene in the inspector.
 */
[RequireComponent(typeof(TransitionInteractable))]
public class SceneTransition : Interactable
{
    [Header("Ensure that the Scene is Added to Build Settings")]
    [SerializeField] private string _scenePath;
    [Header("Y and Z Values are (0.0f, -1.0f)")]
    [SerializeField] private float _newScenePlayerPosX;
    private PlayerController _player;
    private TransitionInteractable _transition;

    private void Start()
    {
        _transition = GetComponent<TransitionInteractable>();
        _player = PlayerController.Instance;
    }

    public override IEnumerator Interact()
    {
        yield return StartCoroutine(TransitionScene());
    }

    /**
     * Transition to scene with the given transition interactable.
     * As such a scene transition does not need a transition interactable added to the Interaction Manager.
     * Be careful since the transitioning into the new scene only happens after all SFX had finished playing.
     */
    private IEnumerator TransitionScene()
    {
        yield return StartCoroutine(_transition.Interact());
        if (_scenePath != "")
        {
            // Explicitly deactivate interacting animation due to abandonment of the rest of execution in Interaction Manager upon scene transition.
            _player.DeactivateInteractingAnimation();

            // Update the player position in the next scene.
            ScenePlayerInfo.scenePlayerPosition = new Vector3(_newScenePlayerPosX, 0.0f, -1.0f);
            SceneManager.LoadScene(_scenePath);
        }
        yield break;
    }
}
