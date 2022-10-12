using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class TestMinigame : Minigame {

    // Start is called before the first frame update
    new void Start() {
        base.Start();
        controller.playerInput.TestMinigame.Enable();
        controller.playerInput.TestMinigame.TestAction.performed += Test;
    }

    new void End() {
        controller.playerInput.TestMinigame.Disable();
        base.End();
    }

    public void Test(InputAction.CallbackContext context) {
        if (context.performed)
        End();
    }
}
