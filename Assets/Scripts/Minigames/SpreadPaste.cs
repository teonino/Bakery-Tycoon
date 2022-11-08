using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SpreadPaste : Minigame {
    [SerializeField] private float timeAimed = 3;
    [SerializeField] private float minValue = 0;
    [SerializeField] private float maxValue = 0;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Slider slider;
    [SerializeField] private Image sliderFiller;
    [SerializeField] private GameObject validZone;

    private float timeInZone = 0;
    new void Start() {
        base.Start();
        slider.maxValue = timeAimed;

        validZone.GetComponent<RectTransform>().anchorMin = new Vector2(minValue, 0);
        validZone.GetComponent<RectTransform>().anchorMax = new Vector2(maxValue, 1);
        validZone.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        validZone.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);

        InputAction action = playerController.playerInput.SpreadPaste.SpreadPasteAction;
        string inputName = InputControlPath.ToHumanReadableString(action.bindings[action.GetBindingIndexForControl(action.controls[0])].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);

        text.SetText("Hold " + char.ToUpper(inputName[0]) + inputName.Substring(1));
    }


    private void Update() {
        if (playerController.playerInput.SpreadPaste.SpreadPasteAction.ReadValue<float>() > 0) {
            slider.value += Time.deltaTime;

            if (timeInZone >= timeAimed) {
                End();
            }
        }
        else 
            slider.value -= Time.deltaTime / 2;

        if (slider.value >= minValue && slider.value <= maxValue) {
            timeInZone += Time.deltaTime;
            sliderFiller.color = Color.green;
        }
        else {
            sliderFiller.color = Color.blue;
        }
    }

    public override void EnableInputs() {
        playerController.playerInput.SpreadPaste.Enable();
    }

    public override void DisableInputs() {
        playerController.playerInput.SpreadPaste.Disable();
    }
}
