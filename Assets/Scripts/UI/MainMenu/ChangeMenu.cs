using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMenu : MonoBehaviour
{
    [SerializeField] private List<GameObject> Panel = new List<GameObject>();
    [SerializeField] private List<Animator> PanelAnimator = new List<Animator>();
    void Start()
    {
        Panel[0].SetActive(false); // Options
        Panel[1].SetActive(false); // credit
    }

    public void DisplayOptions()
    {
        for (int i = 0; i < Panel.Count; i++)
        {
            Panel[i].SetActive(false);
        }
        PanelAnimator[0].SetTrigger("TriggerOption");
        Panel[0].SetActive(true);
    }

    public void DisplayCredit()
    {
        for (int i = 0; i < Panel.Count; i++)
        {
            Panel[i].SetActive(false);
        }
        Panel[1].SetActive(true);
    }

    public void DisplayMainMenu()
    {
        for (int i = 0; i < Panel.Count; i++)
        {
            Panel[i].SetActive(false);
        }
        Panel[2].SetActive(true);
    }

}
