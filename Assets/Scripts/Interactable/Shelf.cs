using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Shelf : Interactable {
    private GameObject item;
    public GameObject itemPosition;

    public override void Effect() {
        if (playerController.itemHolded && item == null) {
            playerController.itemHolded.transform.SetParent(transform);
            item = playerController.itemHolded;
            playerController.itemHolded = null;
            item.transform.localPosition = itemPosition.transform.localPosition;
        }
        else if (!playerController.itemHolded && item) {
            Transform arm = playerController.gameObject.transform.GetChild(0);

            playerController.itemHolded = item;
            item = null;
            playerController.itemHolded.transform.SetParent(arm); //the arm of the player becomes the parent
            playerController.itemHolded.transform.localPosition = new Vector3(arm.localPosition.x + arm.localScale.x / 2, 0, 0);
        }
    }

    public GameObject GetItem() => item;
    public GameObject RemoveItem() => item = null;
}
