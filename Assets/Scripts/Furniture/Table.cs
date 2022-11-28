using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : Interactable {
    public List<Chair> chairs;
    public List<GameObject> itemPositions;

    public List<GameObject> items;

    private new void Awake() {
        base.Awake();
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
        if (playerController.GetItemHold() && GetItem(true)) {
            PutDownItem(playerController.GetItemHold());
            if (playerController.GetItemHold().GetComponent<ProductHolder>().product.amount <= 0)
                playerController.SetItemHold(null);
        }
        else if (!playerController.GetItemHold() && GetItem(false))
            TakeItem();
        else if (playerController.GetItemHold() && GetAllItem(false)) {
            GameObject tmpPlayer = playerController.GetItemHold();
            playerController.SetItemHold(null);
            TakeItem();
            PutDownItem(tmpPlayer);
        }
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
            if (chairs[i].customer && go && go.GetComponent<ProductHolder>()) {
                if (chairs[i].customer.state != AIState.eating && go.GetComponent<ProductHolder>().product.productSO && chairs[i].customer.requestedProduct.name == go.GetComponent<ProductHolder>().product.GetName() && !items[i]) {
                    if (go.GetComponent<ProductHolder>().product.amount > 1) {
                        items[i] = go.GetComponent<ProductHolder>().product.productSO.asset.InstantiateAsync(transform).Result;
                        items[i].transform.localPosition = itemPositions[i].transform.localPosition;
                        go.GetComponent<ProductHolder>().product.amount--;
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
        if (!itemPutDown) {
            for (int i = 0; i < items.Count; i++) {
                if (!items[i] && go) {
                    items[i] = go;
                    go.transform.SetParent(transform);
                    go = null;
                    items[i].transform.localPosition = itemPositions[i].transform.localPosition;
                }
            }
        }
    }


    private void TakeItem() {
        Transform arm = playerController.gameObject.transform.GetChild(0);

        for (int i = 0; i < chairs.Count; i++) {
            if (items[i] && !playerController.GetItemHold() && !items[i].GetComponent<ProductHolder>().blocked) {
                playerController.SetItemHold(items[i]);
                items[i] = null;
                playerController.GetItemHold().transform.SetParent(arm); //the arm of the player becomes the parent
                playerController.GetItemHold().transform.localPosition = new Vector3(arm.localPosition.x + arm.localScale.x / 2, 0, 0);
            }
        }
    }
}
