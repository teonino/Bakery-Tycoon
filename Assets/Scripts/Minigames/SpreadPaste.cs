using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SpreadPaste : Minigame {
    [SerializeField] private float timeAimed = 3;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Slider slider;

    new void Start() {
        base.Start();
        slider.maxValue = timeAimed;
        playerController.playerInput.SpreadPaste.Enable();

        InputAction action = playerController.playerInput.SpreadPaste.SpreadPasteAction;
        string inputName = InputControlPath.ToHumanReadableString(action.bindings[action.GetBindingIndexForControl(action.controls[0])].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);

        text.SetText("Hold " + char.ToUpper(inputName[0]) + inputName.Substring(1));
    }


    private void Update() {
        if (playerController.playerInput.SpreadPaste.SpreadPasteAction.ReadValue<float>() > 0) {
            slider.value += Time.deltaTime;

            if (slider.value >= timeAimed) {
                playerController.playerInput.SpreadPaste.Disable();
                End();
            }
        }
        else {
            slider.value -= Time.deltaTime / 2;
        }
    }

    public override void EnableInputs() {
    }

    public override void DisableInputs() {
    }
}
