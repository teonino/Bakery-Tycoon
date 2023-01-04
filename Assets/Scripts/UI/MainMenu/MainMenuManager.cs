using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> Screen = new List<GameObject>();
    [SerializeField] private List<GameObject> Button = new List<GameObject>();
    [SerializeField] private PlayerInput playerInput;




    public void displayMainMenuTools()
    {
        StartCoroutine(DisplayButton());
    }


    public IEnumerator DisplayButton()
    {
        Button[0].SetActive(true);
        Button[1].SetActive(true);
        Button[2].SetActive(true);
        yield return new WaitForSeconds(0.3f);


    }


    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

}
