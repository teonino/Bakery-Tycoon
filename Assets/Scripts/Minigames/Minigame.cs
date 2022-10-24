using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class Minigame : MonoBehaviour {

    [SerializeField] protected CraftingStationType craftingStationRequired;
    //public AssetReference minigameAsset;
    protected WorkstationManager workplacePanel;
    protected PlayerController controller;
    protected float launchTime;
    protected float endTime;

    // Start is called before the first frame update
    protected void Start() {
        workplacePanel = transform.parent.gameObject.GetComponent<WorkstationManager>();
        controller = FindObjectOfType<PlayerController>();
        launchTime = Time.time;
        EnableInputs();
    }

    protected void End() {
        endTime = Time.time;
        DisableInputs();
        DirtyCraftingStation();
        Addressables.ReleaseInstance(gameObject);
        workplacePanel.MinigameComplete();
    }

    protected float GetTimer() => endTime - launchTime; //Return time taken to complete the minigame

    protected void DirtyCraftingStation() {
        GameObject go = null;
        switch (craftingStationRequired) {
            case CraftingStationType.Hoven:
                go = GameObject.FindGameObjectWithTag("Hoven");
                break;
            default:
                break;
        }

        if (go != null)
            go.GetComponent<CraftingStation>().AddDirt();
    }

    public abstract void EnableInputs();
    public abstract void DisableInputs();
}
