using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : Interactable {
    public List<Chair> chairs;
    public List<GameObject> itemPositions;
    public List<GameObject> items;

    private int indexItem;

    private void Awake() {
        foreach (Chair chair in chairs) {
            chair.table = this;
        }
    }

    public int GetChairAvailable() {
        int index = 0;
        foreach (Chair chair in chairs) {
            if (!chair.ocuppied)
                return index;
            index++;
        }
        return -1;
    }
    public override void Effect() {
        if (playerControllerSO.GetPlayerController().GetItemHold() && GetItem(true)) {
            PutDownItem(playerControllerSO.GetPlayerController().GetItemHold());
            if (playerControllerSO.GetPlayerController().GetItemHold().GetComponent<ProductHolder>().product.GetAmount() <= 0)
                playerControllerSO.GetPlayerController().SetItemHold(null);
        }
        else if (!playerControllerSO.GetPlayerController().GetItemHold() && (GetItem(false) || CheckPlate()))
            TakeItem();
        else if (playerControllerSO.GetPlayerController().GetItemHold() && GetAllItem(false)) {
            GameObject tmpPlayer = playerControllerSO.GetPlayerController().GetItemHold();
            playerControllerSO.GetPlayerController().SetItemHold(null);
            TakeItem();
            PutDownItem(tmpPlayer);
        }
    }

    public override bool CanInterract() {
        canInterract = (playerControllerSO.GetPlayerController().GetItemHold() && GetItem(true)) ||
            (!playerControllerSO.GetPlayerController().GetItemHold() && (GetItem(false) || CheckPlate())) ||
            (playerControllerSO.GetPlayerController().GetItemHold() && GetAllItem(false));

        return canInterract;
    }

    private bool CheckPlate() {
        foreach (GameObject item in items) {
            if (item && item.tag == "Plate")
                return true;
        }
        return false;
    }

    public bool GetItem(bool positionAvailable) {
        foreach (GameObject item in items) {
            if (item == !positionAvailable)
                return true;
        }
        return false;
    }
    public bool GetAllItem(bool positionAvailable) {
        bool returnValue = true;

        foreach (GameObject item in items) {
            if (item == !positionAvailable)
                returnValue = false;
        }
        return returnValue;
    }

    private void PutDownItem(GameObject go) {
        bool itemPutDown = false;
        for (int i = 0; i < chairs.Count; i++) {
            if (chairs[i].customer && go && go.GetComponent<ProductHolder>() && !itemPutDown) {
                if (chairs[i].customer.state != AIState.eating && go.GetComponent<ProductHolder>().product.productSO && chairs[i].customer.requestedProduct.name == go.GetComponent<ProductHolder>().product.GetName() && !items[i]) {
                    if (go.GetComponent<ProductHolder>().product.GetAmount() > 1) {
                        indexItem = i;

                        go.GetComponent<ProductHolder>().product.productSO.asset.InstantiateAsync(transform).Completed += (go) => {
                            items[indexItem] = go.Result;
                            items[indexItem].transform.localPosition = itemPositions[indexItem].transform.localPosition;
                            go.Result.GetComponent<ProductHolder>().DisplayOneProduct();
                        };

                        go.GetComponent<ProductHolder>().RemoveAmount();
                        itemPutDown = true;
                    }
                    else {
                        items[i] = go;
                        go.transform.SetParent(transform);
                        go = null;
                        items[i].transform.localPosition = itemPositions[i].transform.localPosition;
                        itemPutDown = true;
                    }
                }
            }
        }
        //if (!itemPutDown) {
        //    for (int i = 0; i < items.Count; i++) {
        //        if (!items[i] && go && go.tag != "paste") {
        //            if (go.GetComponent<ProductHolder>().product.GetAmount() > 1 ) {
        //                items[i] = go.GetComponent<ProductHolder>().product.productSO.asset.InstantiateAsync(transform).Result;
        //                items[i].transform.localPosition = itemPositions[i].transform.localPosition;
        //                go.GetComponent<ProductHolder>().DisplayOneGameObject();
        //            }
        //            else { 
        //                items[i] = go;
        //                go.transform.SetParent(transform);
        //                go = null;
        //                items[i].transform.localPosition = itemPositions[i].transform.localPosition;
        //            }
        //        }
        //    }
        //}
    }


    private void TakeItem() {
        Transform arm = playerControllerSO.GetPlayerController().gameObject.transform.GetChild(0);

        for (int i = 0; i < chairs.Count; i++) {
            if (items[i] && !playerControllerSO.GetPlayerController().GetItemHold() && !items[i].GetComponent<ProductHolder>().blocked) {
                playerControllerSO.GetPlayerController().SetItemHold(items[i]);
                items[i] = null;
                playerControllerSO.GetPlayerController().GetItemHold().transform.SetParent(arm); //the arm of the player becomes the parent
                playerControllerSO.GetPlayerController().GetItemHold().transform.localPosition = new Vector3(arm.localPosition.x + arm.localScale.x / 2, 0, 0);
            }
        }
    }
}
