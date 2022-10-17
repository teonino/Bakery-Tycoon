using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class Workstation : Interactable {
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private AssetReference workplacePanelAsset;

    private WorkstationManager manager;
    private GameObject workplacePanel;

    public override void Effect() {
        if (!playerController.itemHolded) {
            playerController.DisableInput();

            workplacePanelAsset.InstantiateAsync(mainCanvas.transform).Completed += (go) => {
                manager = go.Result.GetComponent<WorkstationManager>();
                workplacePanel = go.Result;
            };

            playerController.playerInput.UI.Enable();
            playerController.playerInput.UI.Quit.performed += Quit;
        }
    }

    //Function when player quit the workstation by themself
    public void Quit(InputAction.CallbackContext context) {
        if (context.performed) {
            if (manager.currentProduct) {
                manager.ResetManager();
            }
            playerController.playerInput.UI.Quit.performed -= Quit;
            playerController.playerInput.UI.Disable();
            playerController.EnableInput();
            Addressables.ReleaseInstance(workplacePanel);
        }
    }

    //Give the item to the player once its done
    public void CloseWorkplace(GameObject go) {
        Transform arm = playerController.gameObject.transform.GetChild(0);

        playerController.itemHolded = go;
        playerController.itemHolded.transform.SetParent(arm); //the arm of the player becomes the parent
        playerController.itemHolded.transform.localPosition = new Vector3(arm.localPosition.x + arm.localScale.x / 2, 0, 0);
        playerController.EnableInput();
        Addressables.ReleaseInstance(workplacePanel);
    }
}
