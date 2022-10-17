using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestMinigame2 : Minigame {
    // Start is called before the first frame update
    new void Start() {
        base.Start();
        controller.playerInput.TestMinigame2.Enable();
        controller.playerInput.TestMinigame2.TestAction.performed += Test;
    }

    public void Test(InputAction.CallbackContext context) {
        if (context.performed)
            End();
    }

    private void OnDestroy() {
        controller.playerInput.TestMinigame2.Disable();
        controller.playerInput.TestMinigame2.TestAction.performed -= Test;
    }
}
