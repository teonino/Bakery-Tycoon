using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class Computer : Interactable {
    [SerializeField] private InterractQuest interractQuest;
    [SerializeField] private InterractQuest secondInterractQuest;
    private GameObject computerPanel;
    [SerializeField] private SFXPlayer sfxPlayer;

    private void Awake() {
        computerPanel = FindObjectOfType<ComputerManager>(true).gameObject;
    }

    public override void Effect() {
        if (computerPanel)
            computerPanel.SetActive(true);
        interractQuest?.OnInterract();
        sfxPlayer.InteractSound();
        secondInterractQuest?.OnInterract();
    }

    public override bool CanInterract() {
        return true;
    }
}
