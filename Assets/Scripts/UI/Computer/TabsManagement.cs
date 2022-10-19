using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabsManagement : MonoBehaviour
{
    [Header("Menu Panel")]
    public List<GameObject> DifferentPanel;

    [Header("Tabs")]
    [SerializeField] private List<Button> Tabs;

    [Header("Tab Color")]
    [SerializeField] private Color SelectedTabsColor;
    [SerializeField] private Color NormalTabsColor;


    public void ShowPricePanel()
    {
        DifferentPanel[0].transform.SetAsLastSibling();
        ResetTheColor();
        //Tabs[0].GetComponent<Image>().color = SelectedTabsColor;
    }

    public void ShowHiringPanel()
    {
        DifferentPanel[1].transform.SetAsLastSibling();
        ResetTheColor();
        //Tabs[1].GetComponent<Image>().color = SelectedTabsColor;
    }

    public void ShowIakePanel()
    {
        DifferentPanel[2].transform.SetAsLastSibling();
        ResetTheColor();
        //Tabs[2].GetComponent<Image>().color = SelectedTabsColor;
    }

    public void ShowAmafoodPanel()
    {
        DifferentPanel[3].transform.SetAsLastSibling();
        ResetTheColor();
        //Tabs[3].GetComponent<Image>().color = SelectedTabsColor;
    }

    public void ShowDetailsPanelProduct()
    {
        DifferentPanel[4].transform.SetAsLastSibling();
    }
    public void ShowYourCartPanel()
    {
        DifferentPanel[5].transform.SetAsLastSibling();
    }

    public void ResetTheColor()
    {
        //for (int i = 0; i < Tabs.Count; i++)
        //{
        //    Tabs[i].GetComponent<Image>().color = NormalTabsColor;
        //}
    }

}
