using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private TMP_Text notificationText;
    [SerializeField] private float duration = 1f;

    private void Awake() => Instance = this;

    public void ShowNotification(string message)
    {
        StartCoroutine(Show(message));
    }

    private IEnumerator Show(string message)
    {
        notificationPanel.SetActive(true);
        notificationText.text = message;

        yield return new WaitForSeconds(duration);

        notificationPanel.SetActive(false);
    }
}
