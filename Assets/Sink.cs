using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Sink : Interactable {
    public override void Effect() {
        if (playerController.itemHolded && playerController.itemHolded.tag == "Plate") {
            Addressables.ReleaseInstance(playerController.itemHolded);
            playerController.itemHolded = null;
        }
    }
}
