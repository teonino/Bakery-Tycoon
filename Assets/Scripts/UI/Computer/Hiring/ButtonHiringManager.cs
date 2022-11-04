using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonHiringManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image button;
    [SerializeField] private TextMeshProUGUI ButtonText;

    [Header("Boolean")]
    [SerializeField] private bool isHired = false;
    [Header("Button's Color")]
    [SerializeField] private Color HiredColor;
    [SerializeField] private Color FiredColor;


    public void ManageButtonStyle()
    {
        if (isHired)
        {
            button.color = HiredColor;
            ButtonText.text = "Hire";
            Debug.Log("Fired");
            isHired = false;
        }
        else
        {
            button.color = FiredColor;
            ButtonText.text = "Fire";
            Debug.Log("Hired");
            isHired = true;
        }
    }
}
