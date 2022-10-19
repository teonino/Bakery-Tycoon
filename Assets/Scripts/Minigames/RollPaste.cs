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

        controller.playerInput.RollPaste.Enable();
        controller.playerInput.RollPaste.Roll.performed += Roll;
        text.SetText(distance + " remaining");
    }

    private void Roll(InputAction.CallbackContext context) {
        distance -= Mathf.Abs((int)controller.playerInput.RollPaste.Roll.ReadValue<float>());
        text.SetText(distance + " remaining");
        if (distance < 0) {
            controller.playerInput.RollPaste.Roll.performed -= Roll;
            controller.playerInput.RollPaste.Disable();
            End();
        }
    }
}
