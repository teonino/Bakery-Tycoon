using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RollPaste : Minigame
{
    private int distance = 2000;

    new void Start() {
        base.Start();

        controller.playerInput.RollPaste.Enable();
        controller.playerInput.RollPaste.Roll.performed += Roll;
    }

    private void Roll(InputAction.CallbackContext context) {
        distance -= Mathf.Abs((int)controller.playerInput.RollPaste.Roll.ReadValue<float>());
        if (distance < 0) {
            controller.playerInput.RollPaste.Roll.performed -= Roll;
            controller.playerInput.RollPaste.Disable();
            End();
        }
    }
}
