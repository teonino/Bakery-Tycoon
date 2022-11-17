using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class Computer : Interactable {

    [SerializeField] private AssetReference computerPanelAsset;

    private GameObject computerPanel = null;
    private GameObject mainCanvas;

    private void Start() {
        mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas");
    }

    public override void Effect() {
        if (!computerPanel)
            computerPanelAsset.InstantiateAsync(mainCanvas.transform).Completed += (go) => {
                computerPanel = go.Result;
                computerPanel.SetActive(true);
            };
        else
            computerPanel.SetActive(true);
    }
}
