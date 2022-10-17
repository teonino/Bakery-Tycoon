using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabsManagement : MonoBehaviour
{
    [Header("Menu Panel")]
    [SerializeField] private GameObject PricePanel;
    [SerializeField] private GameObject HiringPanel;
    [SerializeField] private GameObject IkaePanel;
    [SerializeField] private GameObject AmafoodPanel;
    private bool f = false;
    private bool t = true;

    public void ShowPricePanel()
    {
        PricePanel.SetActive(t);
        HiringPanel.SetActive(f);
        IkaePanel.SetActive(f);
        AmafoodPanel.SetActive(f);
    }

    public void ShowHiringPanel()
    {
        PricePanel.SetActive(f);
        HiringPanel.SetActive(t);
        IkaePanel.SetActive(f);
        AmafoodPanel.SetActive(f);
    }

    public void ShowIakePanel()
    {
        PricePanel.SetActive(f);
        HiringPanel.SetActive(f);
        IkaePanel.SetActive(t);
        AmafoodPanel.SetActive(f);
    }

    public void ShowAmafoodPanel()
    {
        PricePanel.SetActive(f);
        HiringPanel.SetActive(f);
        IkaePanel.SetActive(f);
        AmafoodPanel.SetActive(t);
    }

}
