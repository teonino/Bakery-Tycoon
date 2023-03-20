using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class Workstation : Interactable {
    [SerializeField] private Day day;
    [SerializeField] private DebugState debug;
    [Header("Debug parameters")]
    [SerializeField] private bool skipRequirement = false;
    [SerializeField] private bool skipMinigame = false;

    private WorkstationManager manager;
    private GameObject workplacePanel;
    private SFXPlayer sfxPlayer;

    [SerializeField] private ParticleSystem vfx;

    private void Awake() {
        vfx.Stop();
        sfxPlayer = FindObjectOfType<SFXPlayer>();
    }

    protected override void Start() {
        base.Start();
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
            sfxPlayer?.InteractSound();
            manager.gameObject.SetActive(true);
        }
    }

    public override bool CanInterract() {
        canInterract = !playerControllerSO.GetPlayerController().GetItemHold();
        return canInterract;
    }

    //Give the item to the player once its done
    public virtual void CloseWorkplace(GameObject go) {
        Transform arm = playerControllerSO.GetPlayerController().GetItemSocket().transform;

        playerControllerSO.GetPlayerController().SetItemHold(go);
        playerControllerSO.GetPlayerController().GetItemHold().transform.SetParent(arm); //the arm of the player becomes the parent
        playerControllerSO.GetPlayerController().GetItemHold().transform.localPosition = Vector3.zero;
        playerControllerSO.GetPlayerController().EnableInput();
        workplacePanel.gameObject.SetActive(false);
    }

    public void startMinigames(bool activate)
    {
        if (activate)
            vfx.Play();
        else
            vfx.Stop();
    }

}
