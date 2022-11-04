using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabsManagement : MonoBehaviour {
    [Header("Menu Panel")]
    public List<GameObject> differentPanel;

    [Header("Tabs")]
    [SerializeField] private List<Button> Tabs;

    [Header("Tab Color")]
    [SerializeField] private Color SelectedTabsColor;
    [SerializeField] private Color NormalTabsColor;

    private GameObject currentPanel;

    private void OnEnable() {
        currentPanel = differentPanel[1];
    }

    public void ShowPricePanel() {
        currentPanel.SetActive(false);
        differentPanel[0].SetActive(true);
        currentPanel = differentPanel[0];
        ResetTheColor();
        //Tabs[0].GetComponent<Image>().color = SelectedTabsColor;
    }

    public void ShowHiringPanel() {
        differentPanel[3].SetActive(true);
        ResetTheColor();
        //Tabs[1].GetComponent<Image>().color = SelectedTabsColor;
    }

    public void ShowIakePanel() {
        differentPanel[2].SetActive(true);
        ResetTheColor();
        //Tabs[2].GetComponent<Image>().color = SelectedTabsColor;
    }

    public void ShowAmafoodPanel() {
        currentPanel.SetActive(false);
        differentPanel[1].SetActive(true);
        currentPanel = differentPanel[1];
        ResetTheColor();
        //Tabs[3].GetComponent<Image>().color = SelectedTabsColor;
    }

    public void ShowDetailsPanelProduct() {
        differentPanel[4].transform.SetAsLastSibling();
    }
    public void ShowYourCartPanel() {
        differentPanel[5].transform.SetAsLastSibling();
    }

    public void ResetTheColor() {
        //for (int i = 0; i < Tabs.Count; i++)
        //{
        //    Tabs[i].GetComponent<Image>().color = NormalTabsColor;
        //}
    }

}
