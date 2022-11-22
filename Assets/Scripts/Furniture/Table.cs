using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : Interactable {
    [SerializeField] public List<Chair> chairs;
    [SerializeField] private List<GameObject> itemPositions;

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
        if (playerController.itemHolded && GetItem(true)) {
            PutDownItem(playerController.itemHolded);
            playerController.itemHolded = null;
        }
        else if (!playerController.itemHolded && GetItem(false))
            TakeItem();
        else if (playerController.itemHolded && GetAllItem(false)) {
            GameObject tmpPlayer = playerController.itemHolded;
            playerController.itemHolded = null;
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
        go.transform.SetParent(transform);

        //Can only put item if a customer request it
        for (int i = 0; i < chairs.Count; i++) {
            if (chairs[i].customer && go) {
                if (chairs[i].customer.state != AIState.canInteract && chairs[i].customer.requestedProduct.name == go.GetComponent<ProductHolder>().product.GetName()) {
                    items[i] = go;
                    go = null;
                    items[i].transform.localPosition = itemPositions[i].transform.localPosition;
                }
            }
        }

        for (int i = 0; i < items.Count; i++) {
            if (!items[i] && go) {
                items[i] = go;
                go = null;
                items[i].transform.localPosition = itemPositions[i].transform.localPosition;
            }
        }
    }


    private void TakeItem() {
        Transform arm = playerController.gameObject.transform.GetChild(0);

        for (int i = 0; i < chairs.Count; i++) {
            if (items[i] && !playerController.itemHolded) {
                playerController.itemHolded = items[i];
                items[i] = null;
                playerController.itemHolded.transform.SetParent(arm); //the arm of the player becomes the parent
                playerController.itemHolded.transform.localPosition = new Vector3(arm.localPosition.x + arm.localScale.x / 2, 0, 0);
            }
        }
    }
}
