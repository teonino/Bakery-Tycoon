using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CutPaste : Minigame {
    public int nbIterationAimed = 3;
    public TextMeshProUGUI text;

    int nbIteration = 0;


    new void Start() {
        base.Start();

        controller.playerInput.CutPaste.Enable();


        controller.playerInput.CutPaste.S.Disable();
        controller.playerInput.CutPaste.C.Disable();
    }

    private void Q(InputAction.CallbackContext context) {
        if (context.performed) {
            controller.playerInput.CutPaste.A.Disable();
            controller.playerInput.CutPaste.S.Enable();
            text.SetText("Press S");
        }
    }
    private void S(InputAction.CallbackContext context) {
        if (context.performed) {
            controller.playerInput.CutPaste.S.Disable();
            controller.playerInput.CutPaste.C.Enable();
            text.SetText("Press C");
        }
    }
    private void C(InputAction.CallbackContext context) {
        if (context.performed) {
            controller.playerInput.CutPaste.C.Disable();
            controller.playerInput.CutPaste.A.Enable();
            nbIteration++;
            text.SetText("Press A");

            if (nbIteration == nbIterationAimed) {

                controller.playerInput.CutPaste.Disable();
                End();
                text.SetText("Well played!");
            }
        }
    }

    public override void EnableInputs() {
        controller.playerInput.CutPaste.A.performed += Q;
        controller.playerInput.CutPaste.S.performed += S;
        controller.playerInput.CutPaste.C.performed += C;
    }

    public override void DisableInputs() {
        controller.playerInput.CutPaste.A.performed -= Q;
        controller.playerInput.CutPaste.S.performed -= S;
        controller.playerInput.CutPaste.C.performed -= C;
    }
}
