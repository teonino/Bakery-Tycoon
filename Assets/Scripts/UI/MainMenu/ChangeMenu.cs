using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ChangeMenu : MonoBehaviour {
    [SerializeField] internal List<GameObject> Panel = new List<GameObject>();
    [SerializeField] private MainMenuManager mainMenuManager;
    [SerializeField] private List<Data> datas;
    void Start() {
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
        mainMenuManager.StartCoroutine(mainMenuManager.BlackscreenTransition("Tutorial"));
    }

    public void Quit() {
        mainMenuManager.StartCoroutine(mainMenuManager.BlackscreenTransition("quit"));
        foreach (Data data in datas)
            data.ResetValues();
        
    }

    public void LoadBakery() {
        mainMenuManager.StartCoroutine(mainMenuManager.BlackscreenTransition("FirstBakery"));
        foreach (Data data in datas)
            data.ResetValues();
    }

}
