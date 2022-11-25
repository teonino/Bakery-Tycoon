using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabsManagement : MonoBehaviour {
    [SerializeField] private GameObject firstPanel;
    [SerializeField] private GameObject firstTab;

    private GameObject currentPanel;
    private GameManager gameManager;

    private void Start() {
        gameManager = FindObjectOfType<GameManager>();

        currentPanel = firstPanel;
        currentPanel.SetActive(true);

        if (gameManager.IsGamepad()) {
            gameManager.SetEventSystemToStartButton(firstTab);
        }
    }

    public void ShowPanel(GameObject panel) {
        currentPanel.SetActive(false);
        panel.SetActive(true);
        currentPanel = panel;
    }
}
