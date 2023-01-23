using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class Workstation : Interactable {
    [SerializeField] private AssetReference workplacePanelAsset;
    [SerializeField] private Day day;
    [SerializeField] private DebugState debug;
    [Header("Debug parameters")]
    [SerializeField] private bool skipRequirement = false;
    [SerializeField] private bool skipMinigame = false;
    [SerializeField] private InterractQuest interractQuest;

    private WorkstationManager manager;
    private GameObject workplacePanel;

    private void Start() {
        if (!debug.GetDebug())
            skipMinigame = skipRequirement = false;

        manager = FindObjectOfType<WorkstationManager>(true);
        manager.skipMinigame = skipMinigame;
        manager.skipRequirement = skipRequirement;
        workplacePanel = manager.gameObject;
    }

    public override void Effect() {
        if (!playerControllerSO.GetPlayerController().GetItemHold()) {
            playerControllerSO.GetPlayerController().DisableInput();

            manager.gameObject.SetActive(true);

            playerControllerSO.GetPlayerController().playerInput.UI.Enable();
            playerControllerSO.GetPlayerController().playerInput.UI.Quit.performed += Quit;
            interractQuest.OnInterract();
        }
    }

    //Function when player quit the workstation by themself
    public void Quit(InputAction.CallbackContext context) {
        if (context.performed) {
            manager.ResetManager();
            playerControllerSO.GetPlayerController().playerInput.UI.Quit.performed -= Quit;
            playerControllerSO.GetPlayerController().playerInput.UI.Disable();
            playerControllerSO.GetPlayerController().EnableInput();
            workplacePanel.gameObject.SetActive(false);
        }
    }

    //Give the item to the player once its done
    public void CloseWorkplace(GameObject go) {
        Transform arm = playerControllerSO.GetPlayerController().GetItemSocket().transform;
        
        playerControllerSO.GetPlayerController().SetItemHold(go);
        playerControllerSO.GetPlayerController().GetItemHold().transform.SetParent(arm); //the arm of the player becomes the parent
        playerControllerSO.GetPlayerController().GetItemHold().transform.localPosition = Vector3.zero;
        playerControllerSO.GetPlayerController().EnableInput();
        workplacePanel.gameObject.SetActive(false);
    }
}
