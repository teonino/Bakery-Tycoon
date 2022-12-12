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

    private WorkstationManager manager;
    private GameObject workplacePanel;

    private void Start() {
        if (!debug.GetDebug()) 
            skipMinigame = skipRequirement = false;
    }



    public override void Effect() {
        if (!playerControllerSO.GetPlayerController().GetItemHold() && day.GetDayTime() != DayTime.Evening) {
            playerControllerSO.GetPlayerController().DisableInput();

            workplacePanelAsset.InstantiateAsync(GameObject.FindGameObjectWithTag("MainCanvas").transform).Completed += (go) => {
                manager = go.Result.GetComponent<WorkstationManager>();
                manager.skipRequirement = skipRequirement;
                manager.skipMinigame = skipMinigame;
                workplacePanel = go.Result;
            };

            playerControllerSO.GetPlayerController().playerInput.UI.Enable();
            playerControllerSO.GetPlayerController().playerInput.UI.Quit.performed += Quit;
        }
    }

    //Function when player quit the workstation by themself
    public void Quit(InputAction.CallbackContext context) {
        if (context.performed) {
            manager.ResetManager();
            playerControllerSO.GetPlayerController().playerInput.UI.Quit.performed -= Quit;
            playerControllerSO.GetPlayerController().playerInput.UI.Disable();
            playerControllerSO.GetPlayerController().EnableInput();
            if (workplacePanel)
                Addressables.ReleaseInstance(workplacePanel);
        }
    }

    //Give the item to the player once its done
    public void CloseWorkplace(GameObject go) {
        Transform arm = playerControllerSO.GetPlayerController().GetItemSocket().transform;

        playerControllerSO.GetPlayerController().SetItemHold(go);
        playerControllerSO.GetPlayerController().GetItemHold().transform.SetParent(arm); //the arm of the player becomes the parent
        playerControllerSO.GetPlayerController().GetItemHold().transform.localPosition = Vector3.zero;
        playerControllerSO.GetPlayerController().EnableInput();
        if (workplacePanel)
            Addressables.ReleaseInstance(workplacePanel);
    }
}
