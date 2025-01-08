using System.Collections;
using UnityEngine;

/** <summary>
    Moves a game object in the scene as part of an interaction sequence.
    The object can be moved to either the player's current position or a custom set position.
    Main use is to move a cutscene trigger to the player's position.
    </summary>
*/
public class MoveObjectInteractable : Interactable
{
    private enum MovePositionSelection
    {
        PlayerPosition,
        CustomPosition,
    }

    [SerializeField] private GameObject _movedObject;
    [SerializeField] private MovePositionSelection _selectedMoveToPosition;

    [Header("Fill This If Custom Position is Selected.")]
    [SerializeField] private Vector3 _customPosition;

    public override IEnumerator Interact()
    {
        Vector3 moveToPos = GetMoveToPosition();
        _movedObject.transform.position = moveToPos;
        yield break;
    }

    Vector3 GetMoveToPosition()
    {
        switch (_selectedMoveToPosition)
        {
            case MovePositionSelection.PlayerPosition:
                return PlayerController.Instance.transform.position;

            case MovePositionSelection.CustomPosition:
                return _customPosition;

            default:
                return Vector3.zero;
        }
    }
}
