using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ExitPuzzleScene : MonoBehaviour
{
    [SerializeField] private SceneTransition _sceneTransition;

    void Start()
    {
        Assert.IsNotNull(_sceneTransition, 
            "Scene Transition Is Not Attached To: " +  name + "!");
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            ExitScene();
        }
    }

    private void ExitScene()
    {
        StartCoroutine(_sceneTransition.Interact());
    }
}
