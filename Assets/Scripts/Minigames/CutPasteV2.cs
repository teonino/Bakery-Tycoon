using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CutPasteV2 : Minigame {
    [SerializeField] private TextMeshProUGUI text;
    private int rngInt;
    private InputAction action;

    new void Start() {
        base.Start();

        string inputName = GetControl(action);
        text.SetText("Press " + char.ToUpper(inputName[0]) + inputName.Substring(1));
    }

    private void DirectionalAction(InputAction.CallbackContext ctx) {
        if (ctx.performed)
            End();
    }

    public override void EnableInputs() {
        action = GetRandomAction();
        playerController.playerInput.CutPasteV2.Enable();
        action.performed += DirectionalAction;
    }
    public override void DisableInputs() {
        action.performed -= DirectionalAction;
        playerController.playerInput.CutPasteV2.Disable();
    }
    private InputAction GetRandomAction() {
        rngInt = Random.Range(0, 4);
        List<InputAction> actions = new List<InputAction>();
        actions.Add(playerController.playerInput.CutPasteV2.UpAction);
        actions.Add(playerController.playerInput.CutPasteV2.DownAction);
        actions.Add(playerController.playerInput.CutPasteV2.RightAction);
        actions.Add(playerController.playerInput.CutPasteV2.LeftAction);

        return actions[rngInt];
    }
}
