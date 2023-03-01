using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SpreadPaste : Minigame {
    [SerializeField] protected float timeAimed = 3;
    [SerializeField] protected float minValue = 0;
    [SerializeField] protected float maxValue = 0;
    [SerializeField] protected TextMeshProUGUI text;
    [SerializeField] protected Slider slider;
    [SerializeField] protected Image sliderFiller;
    [SerializeField] protected GameObject validZone;

    protected float timeInZone = 0;
    new void Start() {
        base.Start();
        slider.maxValue = timeAimed;

        validZone.GetComponent<RectTransform>().anchorMin = new Vector2(minValue, 0);
        validZone.GetComponent<RectTransform>().anchorMax = new Vector2(maxValue, 1);
        validZone.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        validZone.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);

        string inputName = GetControl(playerController.playerInput.SpreadPaste.SpreadPasteAction);
        text.SetText("Hold " + char.ToUpper(inputName[0]) + inputName.Substring(1));
    }


    private void FixedUpdate() {
        if (playerController.playerInput.SpreadPaste.SpreadPasteAction.ReadValue<float>() > 0)
            slider.value += Time.deltaTime;
        slider.value -= Time.deltaTime / 3;

        CheckInZone();
        if (timeInZone >= timeAimed) {
            End();
        }  
    }

    protected void CheckInZone() {
        if (slider.value >= minValue && slider.value <= maxValue) {
            timeInZone += Time.deltaTime;
            sliderFiller.color = Color.white;
        }
        else
            sliderFiller.color = new Color(192, 192, 192);
    }

    public override void EnableInputs() {
        playerController.playerInput.SpreadPaste.Enable();
    }

    public override void DisableInputs() {
        playerController.playerInput.SpreadPaste.Disable();
    }
}
