using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

/** <summary>
    This enum helps define where to move an object or freeze the player during a cutscene.
    </summary>
*/
public class MovePosition : MonoBehaviour
{
    public enum Position
    {
        PLAYER_POSITION,
        CUSTOM_POSITION,
    }

    /** <summary>
        Returns the move position depends on the defined Position enum member.
        </summary>
     */ 
    public static Vector3 GetMovePosX(Position movePosition, Vector3 customFreezePos)
    {
        switch (movePosition)
        {
            case Position.PLAYER_POSITION:
                return PlayerController.Instance.transform.position;

            case Position.CUSTOM_POSITION:
                return customFreezePos;
        }
        return Vector3.zero;
    }
}
