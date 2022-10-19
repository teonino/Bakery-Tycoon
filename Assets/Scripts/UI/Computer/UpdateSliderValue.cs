using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateSliderValue : MonoBehaviour
{
    [Header("Slider")]
    [SerializeField] private Slider Slider;
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI SliderMinimum;
    [SerializeField] private TextMeshProUGUI SliderMaximum;
    [SerializeField] private TextMeshProUGUI ActualPrice;
    [SerializeField] private TextMeshProUGUI ActualQuality;
    [Header("Quality")]
    [SerializeField] private string quality;

    private void Start()
    {
        SliderMinimum.text = Slider.minValue.ToString();
        SliderMaximum.text = Slider.maxValue.ToString();
        ActualPrice.text = Slider.value.ToString();
        ActualQuality.text = quality;

    }
    public void UpdateText()
    {
        SliderMinimum.text = Slider.minValue.ToString();
        SliderMaximum.text = Slider.maxValue.ToString();
        ActualPrice.text = Slider.value.ToString();
    }

}
