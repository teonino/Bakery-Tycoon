using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerManager : MonoBehaviour {
    [SerializeField] private PlayerControllerSO playerControllerSO;
    [SerializeField] private Controller controller;
    [SerializeField] private List<GameObject> tabs;
    [SerializeField] private TabsManagement tabsManagement;
    [SerializeField] private MoneyUI moneyRef;

    private void Awake()
    {
        tabsManagement.canChangeTab = true;
        moneyRef.subMoney();
    }

    private void Update() {
        if (gameObject.activeSelf && playerControllerSO.GetPlayerInputState())
            playerControllerSO?.GetPlayerController().DisableInput();
    }

    private void OnDisable() {
        for (int i = 0; i < tabs.Count; i++)
            tabs[i]?.gameObject.SetActive(false);
        tabsManagement.canChangeTab = true;
        controller?.SetEventSystemToStartButton(null);
    }

    private void OnDestroy()
    {
        moneyRef.unsubMoney();
    }
}
