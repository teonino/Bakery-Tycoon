using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NotificationSO", menuName = "Event/Notification")]
public class NotificationEvent : ScriptableObject {
    public Action<NotificationType> action;
    public void Invoke(NotificationType type) => action?.Invoke(type);
}
