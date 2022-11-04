using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class AddButter : Minigame {
    public TextMeshProUGUI text;

    public override void EnableInputs() {
        playerController.playerInput.AddButter.A.performed += A;
        playerController.playerInput.AddButter.Z.performed += Z;
        playerController.playerInput.AddButter.E.performed += E;
        playerController.playerInput.AddButter.Q.performed += Q;
        playerController.playerInput.AddButter.S.performed += S;
        playerController.playerInput.AddButter.D.performed += D;
        playerController.playerInput.AddButter.W.performed += W;
        playerController.playerInput.AddButter.X.performed += X;
        playerController.playerInput.AddButter.C.performed += C;
    }

    public override void DisableInputs() {
        playerController.playerInput.AddButter.A.performed -= A;
        playerController.playerInput.AddButter.Z.performed -= Z;
        playerController.playerInput.AddButter.E.performed -= E;
        playerController.playerInput.AddButter.Q.performed -= Q;
        playerController.playerInput.AddButter.S.performed -= S;
        playerController.playerInput.AddButter.D.performed -= D;
        playerController.playerInput.AddButter.W.performed -= W;
        playerController.playerInput.AddButter.X.performed -= X;
        playerController.playerInput.AddButter.C.performed -= C;
    }

    new void Start() {
        base.Start();
        playerController.playerInput.AddButter.Enable();
        playerController.playerInput.AddButter.Z.Disable();
        playerController.playerInput.AddButter.E.Disable();
        playerController.playerInput.AddButter.Q.Disable();
        playerController.playerInput.AddButter.S.Disable();
        playerController.playerInput.AddButter.D.Disable();
        playerController.playerInput.AddButter.W.Disable();
        playerController.playerInput.AddButter.X.Disable();
        playerController.playerInput.AddButter.C.Disable();
    }

    private void A(InputAction.CallbackContext context) {
        if (context.performed) {
            playerController.playerInput.AddButter.A.Disable();
            playerController.playerInput.AddButter.Z.Enable();
            text.SetText("Press Z");
        }
    }

    private void Z(InputAction.CallbackContext context) {
        if (context.performed) {
            playerController.playerInput.AddButter.Z.Disable();
            playerController.playerInput.AddButter.E.Enable();
            text.SetText("Press E");
        }
    }
    private void E(InputAction.CallbackContext context) {
        if (context.performed) {
            playerController.playerInput.AddButter.E.Disable();
            playerController.playerInput.AddButter.Q.Enable();
            text.SetText("Press Q");
        }
    }
    private void Q(InputAction.CallbackContext context) {
        if (context.performed) {
            playerController.playerInput.AddButter.Q.Disable();
            playerController.playerInput.AddButter.S.Enable();
            text.SetText("Press S");
        }
    }
    private void S(InputAction.CallbackContext context) {
        if (context.performed) {
            playerController.playerInput.AddButter.S.Disable();
            playerController.playerInput.AddButter.D.Enable();
            text.SetText("Press D");
        }
    }
    private void D(InputAction.CallbackContext context) {
        if (context.performed) {
            playerController.playerInput.AddButter.D.Disable();
            playerController.playerInput.AddButter.W.Enable();
            text.SetText("Press W");
        }
    }
    private void W(InputAction.CallbackContext context) {
        if (context.performed) {
            playerController.playerInput.AddButter.W.Disable();
            playerController.playerInput.AddButter.X.Enable();
            text.SetText("Press X");
        }
    }
    private void X(InputAction.CallbackContext context) {
        if (context.performed) {
            playerController.playerInput.AddButter.X.Disable();
            playerController.playerInput.AddButter.C.Enable();
            text.SetText("Press C");
        }
    }
    private void C(InputAction.CallbackContext context) {
        if (context.performed) {
            playerController.playerInput.AddButter.C.Disable();
            playerController.playerInput.AddButter.Disable();
            End();
        }
    }
}
