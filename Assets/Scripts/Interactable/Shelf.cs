using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Shelf : Interactable {
    public GameObject item;
    public GameObject itemPosition;
    public bool spawnAsset;
    public AssetReference debugAsset;

    public override void Effect() {
        if (playerController.itemHolded && !item) {
            PutDownItem(playerController.itemHolded);
            playerController.itemHolded = null;
        }
        else if (!playerController.itemHolded && item)
            TakeItem();
        else if (playerController.itemHolded && item) {
            GameObject tmpPlayer = playerController.itemHolded;
            playerController.itemHolded = null;
            TakeItem();
            PutDownItem(tmpPlayer);
        }

        if (playerController.itemHolded == null && item == null && spawnAsset)
            debugAsset.InstantiateAsync(transform).Completed += (go) => {
                item = go.Result;
                item.transform.position = itemPosition.transform.position;
            };
    }

    private void PutDownItem(GameObject go) {
        go.transform.SetParent(transform);
        item = go;
        go = null;
        item.transform.localPosition = itemPosition.transform.localPosition;
    }

    private void TakeItem() {
        Transform arm = playerController.gameObject.transform.GetChild(0);

        playerController.itemHolded = item;
        item = null;
        playerController.itemHolded.transform.SetParent(arm); //the arm of the player becomes the parent
        playerController.itemHolded.transform.localPosition = new Vector3(arm.localPosition.x + arm.localScale.x / 2, 0, 0);
    }

    public GameObject GetItem() => item;
    public GameObject RemoveItem() => item = null;
}
