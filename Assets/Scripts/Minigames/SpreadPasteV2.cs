using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpreadPasteV2 : SpreadPaste {
    [SerializeField] private float valuePerStep;
    private InputAction lastAction;

    // Start is called before the first frame update
    new void Start() {
        base.Start();

        InputAction firstAction = playerController.playerInput.SpreadPasteV2.SpreadPasteFirstAction;
        InputAction secondAction = playerController.playerInput.SpreadPasteV2.SpreadPasteSecondAction;

        firstAction.performed += FirstActionFired;
        secondAction.performed += SecondActionFired;

        string inputName = GetControl(playerController.playerInput.SpreadPasteV2.SpreadPasteFirstAction, 0);
        string inputNameTwo = GetControl(playerController.playerInput.SpreadPasteV2.SpreadPasteSecondAction, 0);
        text.SetText("Press " + inputName + " / " + inputNameTwo + " alternatively");
    }

    // Update is called once per frame
    void FixedUpdate() {
        CheckInZone();
        slider.value -= Time.deltaTime / 3;
        if (timeInZone >= timeAimed) {
            End();
        }
    }

    private void FirstActionFired(InputAction.CallbackContext ctx) {
        if (ctx.performed && ctx.action != lastAction) {
            slider.value += valuePerStep;
            lastAction = ctx.action;
        }
    }

    private void SecondActionFired(InputAction.CallbackContext ctx) {
        if (ctx.performed && ctx.action != lastAction) {
            slider.value += valuePerStep;
            lastAction = ctx.action;
        }
    }

    public override void EnableInputs() {
        playerController.playerInput.SpreadPasteV2.Enable();
    }

    public override void DisableInputs() {
        playerController.playerInput.SpreadPasteV2.Disable();
    }
}
