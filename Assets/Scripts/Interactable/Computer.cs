using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class Computer : Interactable {
    
    [SerializeField] private GameObject computerPanelAsset;

    private GameObject mainCanvas;
    private void Start() {
        mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas");
    }

    public override void Effect() => computerPanelAsset.SetActive(true);

}
