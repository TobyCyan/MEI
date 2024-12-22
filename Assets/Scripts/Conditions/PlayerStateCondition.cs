using UnityEngine;

/**
 * A player state condition that checks whether the player has completed the state given.
 * The player state can be obtained after interacting with certain objects.
 */
public class PlayerStateCondition : MonoBehaviour, ICondition
{
    [SerializeField] private PlayerState.State m_PlayerState = PlayerState.State.None;

    private PlayerController m_Player;

    private void Start()
    {
        m_Player = PlayerController.Instance;
    }

    public bool CheckCond()
    {
        return m_Player.IsContainState(m_PlayerState);
    }
}
