using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour {
    [SerializeField] private AssetReference notificationPanelAsset;
    [SerializeField] private NotificationEvent notificationSO;
    [SerializeField] private float displayTimeInSecond;
    [SerializeField] private float timeBetweenEachNotification = 0.5f;
    [SerializeField] private SFX_SO sfx;
    [SerializeField] private NotificationType notifType;

    private Queue<NotificationType> queue;
    private bool notificationDisplayed = false;

    private void Awake() {
        queue = new Queue<NotificationType>();
    }

    private void OnEnable() {
        notificationSO.action += AddNotif;
    }

    private void OnDisable() {
        notificationSO.action -= AddNotif;
    }

    private void AddNotif(NotificationType type) {
        queue.Enqueue(type);
        if (!notificationDisplayed && queue.Count > 0)
            StartCoroutine(DisplayNotification());
    }

    private IEnumerator DisplayNotification() {
        NotificationPanel panel = null;
       
        notificationPanelAsset.InstantiateAsync(transform).Completed += (go) => {
            panel = go.Result.GetComponent<NotificationPanel>(); 
            panel.SetTitleText(queue.Peek().GetTitle());
            panel.SetDescriptionText(queue.Peek().GetDescription());
            panel.SetImage(queue.Peek().GetSprite());
            sfx.Invoke("Notification");
        };

        notificationDisplayed = true;

        yield return new WaitForSeconds(displayTimeInSecond);

        queue.Dequeue();
        notificationDisplayed = false;
        Addressables.ReleaseInstance(panel.gameObject);

        yield return new WaitForSeconds(timeBetweenEachNotification);

        if (queue.Count > 0)
            StartCoroutine(DisplayNotification());
    }

    public void DEBUG_CreateNotif() {
        AddNotif(notifType);
    }
}
