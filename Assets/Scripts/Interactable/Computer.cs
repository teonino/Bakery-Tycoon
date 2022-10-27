using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class Computer : Interactable {
    [SerializeField] private GameObject mainCanvas;
    //[SerializeField] private AssetReference computerPanelAsset;
    [SerializeField] private GameObject computerPanelAsset;

    public override void Effect() => computerPanelAsset.SetActive(true);

}
