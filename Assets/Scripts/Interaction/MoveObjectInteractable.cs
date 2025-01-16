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
    [SerializeField] private GameObject _movedObject;
    [SerializeField] private MovePosition.Position _selectedMoveToPosition;

    [Header("Fill This If Custom Position is Selected.")]
    [SerializeField] private Vector3 _customPosition;

    public override IEnumerator Interact()
    {
        Vector3 moveToPos = MovePosition.GetMovePosX(_selectedMoveToPosition, _customPosition);
        _movedObject.transform.position = moveToPos;
        yield break;
    }

}
