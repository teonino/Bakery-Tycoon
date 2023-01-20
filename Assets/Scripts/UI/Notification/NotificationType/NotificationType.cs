using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Notification", menuName = "Data/Notification")]
public class NotificationType : ScriptableObject {
    [SerializeField] private string title;
    [SerializeField] private string description;
    [SerializeField] private Sprite image;

    public string GetTitle() => title;
    public string GetDescription() => description;
    public Sprite GetSprite() => image;
}
