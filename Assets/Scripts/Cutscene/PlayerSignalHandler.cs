using UnityEngine;

public class PlayerSignalHandler : MonoBehaviour
{
    private PlayerController _player;

    private void Start()
    {
        _player = PlayerController.Instance;
    }

    public void OnFreezePlayerSignal()
    {
        _player.StopPlayerMovement();
    }

    public void OnUnfreezePlayerSignal()
    {
        _player.ResumePlayerMovement();
    }
}
