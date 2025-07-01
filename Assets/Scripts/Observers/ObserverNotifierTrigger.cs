using UnityEngine;

public class ObserverNotifierTrigger : ObserverNotifier
{
    [SerializeField] private bool _canRepeatedTrigger = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player == PlayerController.Instance)
        {
            NotifyObservers();
            gameObject.SetActive(_canRepeatedTrigger);
        }
    }
}
