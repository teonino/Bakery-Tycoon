using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Shelf : Interactable {
    public GameObject item;
    [HideInInspector] public bool occupied;

    public override void Effect() {
        if (playerController.itemHolded && item == null) {
            playerController.itemHolded.transform.SetParent(transform);
            item = playerController.itemHolded;
            playerController.itemHolded = null;
            item.transform.localPosition = new Vector3(0, (transform.localScale.y / 2) + (item.transform.localScale.y / 2), 0);
        }
        else if (!playerController.itemHolded && item) {
            Transform arm = playerController.gameObject.transform.GetChild(0);

            playerController.itemHolded = item;
            item = null;
            playerController.itemHolded.transform.SetParent(arm); //the arm of the player becomes the parent
            playerController.itemHolded.transform.localPosition = new Vector3(arm.localPosition.x + arm.localScale.x / 2, 0, 0);
        }
    }
}
