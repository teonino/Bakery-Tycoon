using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Sink : Interactable {
    public override void Effect() {
        if (playerControllerSO.GetPlayerController().GetItemHold() && playerControllerSO.GetPlayerController().GetItemHold().tag == "Plate") {
            Addressables.ReleaseInstance(playerControllerSO.GetPlayerController().GetItemHold());
            playerControllerSO.GetPlayerController().SetItemHold(null);
        }
    }
}
