using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpreadPaste : Minigame {
    public int nbIterationAimed = 3;
    public TextMeshProUGUI text;

    int nbIteration = 0;

    new void Start() {
        base.Start();

        controller.playerInput.SpreadPaste.Enable();
        controller.playerInput.SpreadPaste.Q.performed += Q;
        controller.playerInput.SpreadPaste.Z.performed += Z;
        controller.playerInput.SpreadPaste.D.performed += D;
        controller.playerInput.SpreadPaste.X.performed += X;

        controller.playerInput.SpreadPaste.Z.Disable();
        controller.playerInput.SpreadPaste.D.Disable();
        controller.playerInput.SpreadPaste.X.Disable();
    }

    private void Q(InputAction.CallbackContext context) {
        if (context.performed) {
            controller.playerInput.SpreadPaste.Q.Disable();
            controller.playerInput.SpreadPaste.Z.Enable();
            text.SetText("Press Z");
        }
    }

    private void Z(InputAction.CallbackContext context) {
        if (context.performed) {
            controller.playerInput.SpreadPaste.Z.Disable();
            controller.playerInput.SpreadPaste.D.Enable();
            text.SetText("Press D");
        }
    }

    private void D(InputAction.CallbackContext context) {
        if (context.performed) {
            controller.playerInput.SpreadPaste.D.Disable();
            controller.playerInput.SpreadPaste.X.Enable();
            text.SetText("Press X");
        }
    }

    private void X(InputAction.CallbackContext context) {
        if (context.performed) {
            controller.playerInput.SpreadPaste.X.Disable();
            controller.playerInput.SpreadPaste.Q.Enable();
            nbIteration++;
            text.SetText("Press A");

            if (nbIteration == nbIterationAimed) {
                controller.playerInput.SpreadPaste.Q.performed -= Q;
                controller.playerInput.SpreadPaste.Z.performed -= Z;
                controller.playerInput.SpreadPaste.D.performed -= D;
                controller.playerInput.SpreadPaste.X.performed -= X;
                controller.playerInput.SpreadPaste.Disable();
                End();
                text.SetText("Well played!");
            }
        }
    }
}
