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

    public void Test(InputAction.CallbackContext context) {
        if (context.performed)
            End();
    }

    private void OnDestroy() {
        controller.playerInput.TestMinigame.Disable();
        controller.playerInput.TestMinigame.TestAction.performed -= Test;
    }
}
