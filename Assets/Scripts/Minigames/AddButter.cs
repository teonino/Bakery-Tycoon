using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class AddButter : Minigame
{
    public TextMeshProUGUI text;

    new void Start() {
        base.Start();
        controller.playerInput.AddButter.Enable();

        controller.playerInput.AddButter.A.performed += A;
        controller.playerInput.AddButter.Z.performed += Z;
        controller.playerInput.AddButter.E.performed += E;
        controller.playerInput.AddButter.Q.performed += Q;
        controller.playerInput.AddButter.S.performed += S;
        controller.playerInput.AddButter.D.performed += D;
        controller.playerInput.AddButter.W.performed += W;
        controller.playerInput.AddButter.X.performed += X;
        controller.playerInput.AddButter.C.performed += C;

        controller.playerInput.AddButter.Z.Disable();
        controller.playerInput.AddButter.E.Disable();
        controller.playerInput.AddButter.Q.Disable();
        controller.playerInput.AddButter.S.Disable();
        controller.playerInput.AddButter.D.Disable();
        controller.playerInput.AddButter.W.Disable();
        controller.playerInput.AddButter.X.Disable();
        controller.playerInput.AddButter.C.Disable();
    }

    private void A(InputAction.CallbackContext context) {
        if (context.performed) {
            controller.playerInput.AddButter.A.Disable();
            controller.playerInput.AddButter.Z.Enable();
            text.SetText("Press Z");
        }
    }

    private void Z(InputAction.CallbackContext context) {
        if (context.performed) {
            controller.playerInput.AddButter.Z.Disable();
            controller.playerInput.AddButter.E.Enable();
            text.SetText("Press E");
        }
    }
    private void E(InputAction.CallbackContext context) {
        if (context.performed) {
            controller.playerInput.AddButter.E.Disable();
            controller.playerInput.AddButter.Q.Enable();
            text.SetText("Press Q");
        }
    }
    private void Q(InputAction.CallbackContext context) {
        if (context.performed) {
            controller.playerInput.AddButter.Q.Disable();
            controller.playerInput.AddButter.S.Enable();
            text.SetText("Press S");
        }
    }
    private void S(InputAction.CallbackContext context) {
        if (context.performed) {
            controller.playerInput.AddButter.S.Disable();
            controller.playerInput.AddButter.D.Enable();
            text.SetText("Press D");
        }
    }
    private void D(InputAction.CallbackContext context) {
        if (context.performed) {
            controller.playerInput.AddButter.D.Disable();
            controller.playerInput.AddButter.W.Enable();
            text.SetText("Press W");
        }
    }
    private void W(InputAction.CallbackContext context) {
        if (context.performed) {
            controller.playerInput.AddButter.W.Disable();
            controller.playerInput.AddButter.X.Enable();
            text.SetText("Press X");
        }
    }
    private void X(InputAction.CallbackContext context) {
        if (context.performed) {
            controller.playerInput.AddButter.X.Disable();
            controller.playerInput.AddButter.C.Enable();
            text.SetText("Press C");
        }
    }
    private void C(InputAction.CallbackContext context) {
        if (context.performed) {
            controller.playerInput.AddButter.C.Disable();

            controller.playerInput.AddButter.A.performed -= A;
            controller.playerInput.AddButter.Z.performed -= Z;
            controller.playerInput.AddButter.E.performed -= E;
            controller.playerInput.AddButter.Q.performed -= Q;
            controller.playerInput.AddButter.S.performed -= S;
            controller.playerInput.AddButter.D.performed -= D;
            controller.playerInput.AddButter.W.performed -= W;
            controller.playerInput.AddButter.X.performed -= X;
            controller.playerInput.AddButter.C.performed -= C;
            controller.playerInput.AddButter.Disable();
            End();
        }
    }
}
