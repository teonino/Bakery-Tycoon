using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Sink : Interactable {
    public override void Effect() {
        if (playerController.GetItemHold() && playerController.GetItemHold().tag == "Plate") {
            print("Zigounette");
            Addressables.ReleaseInstance(playerController.GetItemHold());
            playerController.SetItemHold(null);
        }
    }
}
