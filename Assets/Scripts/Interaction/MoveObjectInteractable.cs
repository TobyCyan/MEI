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
    enum MovePositionSelection
    {
        PlayerPosition,
        CustomPosition,
    }

    [SerializeField] GameObject m_MovedObject;
    [SerializeField] MovePositionSelection m_SelectedMoveToPosition;

    [Header("Fill This If Custom Position is Selected.")]
    [SerializeField] Vector3 m_CustomPosition;


    public override IEnumerator Interact()
    {
        Vector3 moveToPos = GetMoveToPosition();
        m_MovedObject.transform.position = moveToPos;
        yield break;
    }

    Vector3 GetMoveToPosition()
    {
        switch (m_SelectedMoveToPosition)
        {
            case MovePositionSelection.PlayerPosition:
                return PlayerController.Instance.transform.position;

            case MovePositionSelection.CustomPosition:
                return m_CustomPosition;

            default:
                return Vector3.zero;
        }
    }
}
