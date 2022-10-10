using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Shelf : Interactable
{
    [SerializeField] private AssetReference assetPaymentCanvas;

    [HideInInspector] public GameObject item;

    public override void Effect() {
        if (playerController.itemHolded) {
            playerController.itemHolded.transform.SetParent(transform);
            item = playerController.itemHolded;
            playerController.itemHolded = null;
            item.transform.localPosition = Vector3.zero;
        }
    }
}
