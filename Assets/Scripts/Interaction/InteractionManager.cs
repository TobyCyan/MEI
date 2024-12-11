using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * IMPORTANT: Attach this onto any interactable object and specify the order of interactables.
 * Manager for managing the order of which the interactable scripts will be called.
 */
public class InteractionManager : MonoBehaviour
{
    [SerializeField] private List<Interactable> _interactables = new List<Interactable>();
    private PlayerController _player;

    private void Awake()
    {
        _player = FindAnyObjectByType<PlayerController>();
    }

    public IEnumerator GoThroughInteractions()
    {
        _player.StopPlayerMovement();
        foreach (Interactable interactable in _interactables)
        {
            yield return StartCoroutine(interactable.Interact());
            yield return new WaitForSeconds(0.3f);
        }
        _player.ResumePlayerMovement();
        yield break;
    } 
}
