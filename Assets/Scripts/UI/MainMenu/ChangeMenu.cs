using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ChangeMenu : MonoBehaviour {
    [SerializeField] internal List<GameObject> Panel = new List<GameObject>();
    [SerializeField] private MainMenuManager mainMenuManager;
    [SerializeField] private Tutorial tutorial;
    [SerializeField] private Controller controller;
    [SerializeField] private GameObject firstButton;
    [SerializeField] private List<Data> datas;

    void Start() {
        controller.SetEventSystemToStartButton(firstButton);
        Panel[0].SetActive(false);
        Panel[1].SetActive(false);
    }

    public void DisplayOptions() {
        mainMenuManager.StartCoroutine(mainMenuManager.BlackscreenTransitionInMainMenu(0));
    }

    public void DisplayCredit() {
        mainMenuManager.StartCoroutine(mainMenuManager.BlackscreenTransitionInMainMenu(1));
    }

    public void DisplayMainMenu() {
        mainMenuManager.StartCoroutine(mainMenuManager.BlackscreenTransitionInMainMenu(2));
    }

    public void LoadTutorial() {
        tutorial.SetTutorial(true);
        mainMenuManager.StartCoroutine(mainMenuManager.BlackscreenTransition("Tutorial")); 
        foreach (Data data in datas)
            data.ResetValues();
    }

    public void Quit() {
        mainMenuManager.StartCoroutine(mainMenuManager.BlackscreenTransition("quit"));
        foreach (Data data in datas)
            data.ResetValues();
        
    }

    public void LoadBakery() {
        tutorial.SetTutorial(false);
        mainMenuManager.StartCoroutine(mainMenuManager.BlackscreenTransition("FirstBakery_New"));
        foreach (Data data in datas)
            data.ResetValues();
    }

}
