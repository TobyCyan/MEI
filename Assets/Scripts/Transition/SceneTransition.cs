using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Attach this script to any door/ interactables that causes a scene transition.
 * Remember to indicate the scene path and the player pos in the next scene in the inspector.
 */
public class SceneTransition : Interactable
{
    [Header("Ensure that the Scene is Added to Build Settings")]
    [SerializeField] private string _scenePath;
    [Header("Y and Z Values are (-2.6f, -1.0f)")]
    [SerializeField] private float _newScenePlayerPosX;

    public override IEnumerator Interact()
    {
        Debug.Log("Scene");
        yield return StartCoroutine(TransitionScene());
    }

    private IEnumerator TransitionScene()
    {
        if (_scenePath != "")
        {
            // Update the player position in the next scene.
            ScenePlayerInfo.scenePlayerPosition = new Vector3(_newScenePlayerPosX, -2.6f, -1.0f);
            SceneManager.LoadScene(_scenePath);
        }
        yield break;
    }
}
