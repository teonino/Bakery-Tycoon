using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class Computer : Interactable {
    private GameObject computerPanel;

    private void Awake() {
        computerPanel = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("ComputerPanel").gameObject;
    }

    public override void Effect() {
        if(computerPanel)
            computerPanel.SetActive(true);
    }
}
