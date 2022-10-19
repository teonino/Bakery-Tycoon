using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CraftingStation : Interactable {

    public CraftingStationType type;

    [SerializeField] private int dirty = 0;
    [SerializeField] private AssetReference progressBar;

    private GameObject item;

    public void AddDirt() {
        dirty++;
    }

    public override void Effect() {
        if (playerController.itemHolded && playerController.itemHolded.tag == "Paste") {
            playerController.itemHolded.GetComponent<Product>().product.asset.InstantiateAsync().Completed += (go) => {
                go.Result.transform.position = playerController.itemHolded.transform.position;
                go.Result.transform.SetParent(playerController.itemHolded.transform.parent);
                Addressables.ReleaseInstance(playerController.itemHolded);
                playerController.itemHolded = go.Result;
            };
        }
        else {
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
    }

    public void Clean() => dirty = 0;
}
