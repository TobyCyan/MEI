using UnityEngine;

public class ObjectEnableObserver : MonoBehaviour, IObserver
{
    public void UpdateSelf()
    {
        gameObject.SetActive(true);
    }
}
