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
        PricePanel.transform.SetAsLastSibling();

    }

    public void ShowHiringPanel()
    {
        HiringPanel.transform.SetAsLastSibling();

    }

    public void ShowIakePanel()
    {
        IkaePanel.transform.SetAsLastSibling();

    }

    public void ShowAmafoodPanel()
    {
        AmafoodPanel.transform.SetAsLastSibling();
    }

}
