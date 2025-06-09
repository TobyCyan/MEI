using UnityEngine;

[RequireComponent(typeof(SceneTransition))]
public class SetScenePathObserver : Observer
{
    [SerializeField] private string _newScenePath;

    public override void UpdateSelf()
    {
        SceneTransition sceneTransition = GetComponent<SceneTransition>();
        sceneTransition.SetScenePath(_newScenePath);
    }
}
