using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class Computer : Interactable
{
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private AssetReference stockPanelAsset;

    private GameObject stockPanel;

    public override void Effect() {
        stockPanelAsset.InstantiateAsync(mainCanvas.transform).Completed += (go) => {
            stockPanel = go.Result;
            go.Result.GetComponent<DeliveryManager>().computer = this;
        };
    }
}
