using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Image image;

    public void SetTitleText(string text) => title.text = text;
    public void SetDescriptionText(string text) => description.text = text;
    public void SetImage(Sprite sprite) => image.sprite = sprite;
}
