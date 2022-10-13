using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CraftingStation : Interactable {

    public CraftingStationType type;

    [SerializeField] private int dirty = 0;
    [SerializeField] private AssetReference progressBar;

    public void AddDirt() {
        dirty++;
    }

    public override void Effect() {
        //Check cleanness
        if (dirty < 20)
            print("Clean");
        else {
            //Launch Animation
            progressBar.InstantiateAsync(transform).Completed += (go) => {
                go.Result.transform.localPosition = Vector3.up;
                go.Result.GetComponentInChildren<ProgressBar>().SetDuration(dirty / 10);
                go.Result.GetComponentInChildren<ProgressBar>().SetCraftingStation(this);
            };
        }
    }

    public void Clean() => dirty = 0;
}
