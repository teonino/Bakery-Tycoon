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
        if (!playerController.GetItemHold() && TmpBuild.instance.day.GetDayTime() != DayTime.Evening) {
            playerController.DisableInput();

            workplacePanelAsset.InstantiateAsync(GameObject.FindGameObjectWithTag("MainCanvas").transform).Completed += (go) => {
                manager = go.Result.GetComponent<WorkstationManager>();
                manager.skipRequirement = skipRequirement;
                manager.skipMinigame = skipMinigame;
                workplacePanel = go.Result;
            };

            playerController.playerInput.UI.Enable();
            playerController.playerInput.UI.Quit.performed += Quit;
        }
    }

    //Function when player quit the workstation by themself
    public void Quit(InputAction.CallbackContext context) {
        if (context.performed) {
            manager.ResetManager();
            playerController.playerInput.UI.Quit.performed -= Quit;
            playerController.playerInput.UI.Disable();
            playerController.EnableInput();
            if (workplacePanel)
                Addressables.ReleaseInstance(workplacePanel);
        }
    }

    //Give the item to the player once its done
    public void CloseWorkplace(GameObject go) {
        Transform arm = playerController.GetItemSocket().transform;

        playerController.SetItemHold(go);
        playerController.GetItemHold().transform.SetParent(arm); //the arm of the player becomes the parent
        playerController.GetItemHold().transform.localPosition = Vector3.zero;
        playerController.EnableInput();
        if (workplacePanel)
            Addressables.ReleaseInstance(workplacePanel);
    }
}
