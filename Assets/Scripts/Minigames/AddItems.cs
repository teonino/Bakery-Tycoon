using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class AddItems : Minigame
{
    public AssetReference item;
    public TextMeshProUGUI text;

    private new void Start() {
        base.Start();

        string inputName = GetControl(playerController.playerInput.AddItems.AddItemsAction);
        text.SetText("Press " + char.ToUpper(inputName[0]) + inputName.Substring(1));
    }

    public override void DisableInputs() {
        playerController.playerInput.AddItems.AddItemsAction.performed -= AddItemsAction;
        playerController.playerInput.AddItems.Disable();
    }

    public override void EnableInputs() {
        playerController.playerInput.AddItems.Enable();
        playerController.playerInput.AddItems.AddItemsAction.performed += AddItemsAction;
    }

    public void AddItemsAction(InputAction.CallbackContext context) {
        if (context.performed) {
            //Asset animation 
            End();
        }
    }
}
