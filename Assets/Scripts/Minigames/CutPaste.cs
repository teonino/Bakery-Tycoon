using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CutPaste : Minigame {
    [SerializeField] private int valuePerTap = 10;
    [SerializeField] private int valueLost = 1;
    [SerializeField] private int aimedValue = 50;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Slider slider;

    new void Start() {
        base.Start();
        slider.maxValue = aimedValue;
        playerController.playerInput.CutPaste.Enable();

        InputAction action = playerController.playerInput.CutPaste.CutPasteAction;
        string inputName = InputControlPath.ToHumanReadableString(action.bindings[action.GetBindingIndexForControl(action.controls[0])].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);

        text.SetText("Spam " + char.ToUpper(inputName[0]) + inputName.Substring(1));
    }

    void Update() => slider.value -= Time.deltaTime * valueLost;

    private void CutPasteAction(InputAction.CallbackContext context) {
        if (context.performed) {
            slider.value += valuePerTap;
            if (slider.value >= aimedValue) {
                playerController.playerInput.CutPaste.Disable();
                End();
            }
        }
    }

    public override void EnableInputs() {
        playerController.playerInput.CutPaste.CutPasteAction.performed += CutPasteAction;
    }

    public override void DisableInputs() {
        playerController.playerInput.CutPaste.CutPasteAction.performed -= CutPasteAction;
    }
}
