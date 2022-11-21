using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabsManagement : MonoBehaviour {
    public List<GameObject> panels;
    [SerializeField] private List<Button> tabs;

    [Header("Tab Color")]
    [SerializeField] private Color selectedTabsColor;
    [SerializeField] private Color normalTabsColor;

    private GameObject currentPanel;
    private GameManager gameManager;

    private void Start() {
        gameManager = FindObjectOfType<GameManager>();
        currentPanel = panels[0];

        if (gameManager.IsGamepad()) {
            gameManager.SetEventSystemToStartButton(tabs[0].gameObject);
        }
    }

    public void ShowPricePanel() {
        currentPanel.SetActive(false);
        panels[0].SetActive(true);
        currentPanel = panels[0];
        ResetTheColor();
    }

    public void ShowHiringPanel() {
        panels[3].SetActive(true);
        ResetTheColor();
    }

    public void ShowIakePanel() {
        currentPanel.SetActive(false);
        panels[2].SetActive(true);
        currentPanel = panels[2];
        ResetTheColor();
    }

    public void ShowAmafoodPanel() {
        currentPanel.SetActive(false);
        panels[1].SetActive(true);
        currentPanel = panels[1];
        ResetTheColor();
    }

    public void ShowDetailsPanelProduct() {
        panels[4].transform.SetAsLastSibling();
    }
    public void ShowYourCartPanel() {
        panels[5].transform.SetAsLastSibling();
    }

    public void ShowYourAccountPanel() {
        currentPanel.SetActive(false);
        panels[3].SetActive(true);
        currentPanel = panels[3];
        ResetTheColor();
    }

    public void ShowStatisticPanel() {
        currentPanel.SetActive(false);
        panels[4].SetActive(true);
        currentPanel = panels[4];
        ResetTheColor();
    }

    public void ResetTheColor() {
        //for (int i = 0; i < Tabs.Count; i++)
        //{
        //    Tabs[i].GetComponent<Image>().color = NormalTabsColor;
        //}
    }

}
