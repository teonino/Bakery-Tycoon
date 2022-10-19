using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class Computer : Interactable
{
    [SerializeField] private GameObject mainCanvas;
    //[SerializeField] private AssetReference computerPanelAsset;
    [SerializeField] private GameObject computerPanelAsset;

    public override void Effect() {
        computerPanelAsset.SetActive(true);
        //computerPanelAsset.InstantiateAsync(mainCanvas.transform).Completed += (go) => {
        //    go.Result.GetComponent<DeliveryManager>().computer = this;
        //    mainCanvas.GetComponent<TabsManagement>().DifferentPanel.Add(go.Result.GetComponentInChildren<DeliveryManager>().gameObject);
        //};
    }
}
