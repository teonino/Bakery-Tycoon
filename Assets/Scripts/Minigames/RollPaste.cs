using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RollPaste : Minigame
{
    public int distance = 2000;
    public TextMeshProUGUI text;

    new void Start() {
        base.Start();

        playerController.playerInput.RollPaste.Enable();
        text.SetText(distance + " remaining");
    }

    private void Roll(InputAction.CallbackContext context) {
        distance -= Mathf.Abs((int)playerController.playerInput.RollPaste.Roll.ReadValue<float>());
        text.SetText(distance + " remaining");
        if (distance < 0) {
            playerController.playerInput.RollPaste.Disable();
            End();
        }
    }

    public override void EnableInputs() {
        playerController.playerInput.RollPaste.Roll.performed += Roll;
    }

    public override void DisableInputs() {
        playerController.playerInput.RollPaste.Roll.performed -= Roll;
    }
}
