using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;


public class Shelf : Interactable {
    [SerializeField] private GameObject item;
    [SerializeField] private GameObject itemPosition;
    [SerializeField] private bool spawnAsset;
    [SerializeField] private AssetReference debugAsset;
    [SerializeField] private DebugState debugState;
    [SerializeField] private InterractQuest interractQuest;
    [SerializeField] private SFXPlayer sfxPlayer;


    protected void Start() {
        if (!debugState.GetDebug())
            spawnAsset = false;
    }

    public override void Effect() {
        if (playerControllerSO.GetPlayerController().GetItemHold() && !item) {
            PutDownItem(playerControllerSO.GetPlayerController().GetItemHold());
            playerControllerSO.GetPlayerController().SetItemHold(null);
            interractQuest?.OnInterract();
        }
        else if (!playerControllerSO.GetPlayerController().GetItemHold() && item)
            TakeItem();
        else if (playerControllerSO.GetPlayerController().GetItemHold() && item) {
            GameObject tmpPlayer = playerControllerSO.GetPlayerController().GetItemHold();
            playerControllerSO.GetPlayerController().SetItemHold(null);
            TakeItem();
            PutDownItem(tmpPlayer);
        }

        if (playerControllerSO.GetPlayerController().GetItemHold() == null && item == null && spawnAsset)
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
        sfxPlayer.InteractSound();

    }

    private void TakeItem() {
        Transform arm = playerControllerSO.GetPlayerController().GetItemSocket().transform;

        playerControllerSO.GetPlayerController().SetItemHold(item);
        item = null;
        playerControllerSO.GetPlayerController().GetItemHold().transform.SetParent(arm); //the arm of the player becomes the parent
        playerControllerSO.GetPlayerController().GetItemHold().transform.localPosition = new Vector3(arm.localPosition.x + arm.localScale.x / 2, 0, 0);
        sfxPlayer.InteractSound();
    }

    public GameObject GetItem() => item;
    public GameObject RemoveItem() => item = null;
}
