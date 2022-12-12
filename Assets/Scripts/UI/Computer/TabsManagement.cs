using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabsManagement : MonoBehaviour {
    [SerializeField] private GameObject firstPanel;
    [SerializeField] private GameObject firstTab;
    [SerializeField] private Controller controller;

    private GameObject currentPanel;

    private void OnEnable() {
        currentPanel = firstPanel;
        currentPanel.SetActive(true);

        if (controller.IsGamepad()) {
            controller.SetEventSystemToStartButton(firstTab);
        }
    }

    public void ShowPanel(GameObject panel) {
        currentPanel.SetActive(false);
        panel.SetActive(true);
        currentPanel = panel;
    }
}
