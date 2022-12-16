using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerManager : MonoBehaviour {
    [SerializeField] private PlayerControllerSO playerControllerSO;
    [SerializeField] private Controller controller;
    [SerializeField] private List<GameObject> tabs;

    private void Update() {
        if (gameObject.activeSelf && playerControllerSO.GetPlayerInputState())
            playerControllerSO?.GetPlayerController().DisableInput();
    }

    private void OnDisable() {
        for (int i = 0; i < tabs.Count; i++)
            tabs[i].gameObject.SetActive(false);

        controller?.SetEventSystemToStartButton(null);
    }
}
