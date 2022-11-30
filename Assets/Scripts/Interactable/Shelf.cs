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

    protected void Start() {
        if (!gameManager.GetDebug())
            spawnAsset = false;
    }

    public override void Effect() {
        if (playerController.GetItemHold() && !item) {
            PutDownItem(playerController.GetItemHold());
            playerController.SetItemHold(null);
        }
        else if (!playerController.GetItemHold() && item)
            TakeItem();
        else if (playerController.GetItemHold() && item) {
            GameObject tmpPlayer = playerController.GetItemHold();
            playerController.SetItemHold(null);
            TakeItem();
            PutDownItem(tmpPlayer);
        }

        if (playerController.GetItemHold() == null && item == null && spawnAsset)
            debugAsset.InstantiateAsync(transform).Completed += (go) => {
                item = go.Result;
                item.transform.position = itemPosition.transform.position;
                item.GetComponent<ProductHolder>().product.amount = 3;
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

        playerController.SetItemHold(item);
        item = null;
        playerController.GetItemHold().transform.SetParent(arm); //the arm of the player becomes the parent
        playerController.GetItemHold().transform.localPosition = new Vector3(arm.localPosition.x + arm.localScale.x / 2, 0, 0);
    }

    public GameObject GetItem() => item;
    public GameObject RemoveItem() => item = null;
}
