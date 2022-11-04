using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Bin : Interactable {
    public override void Effect() {
        if (playerController.itemHolded) {
            Addressables.ReleaseInstance(playerController.itemHolded);
            playerController.itemHolded = null;
        }
    }
}
