using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ChangeMenu : MonoBehaviour {
    [SerializeField] private List<GameObject> Panel = new List<GameObject>();
    [SerializeField] private List<TextMeshProUGUI> TextButton;
    [SerializeField] private MainMenuManager mainMenuManager;
    [SerializeField] private List<Data> datas;
    void Start() {
        Panel[0].SetActive(false); // Options
        Panel[1].SetActive(false); // credit
    }

    public void DisplayOptions() {
        for (int i = 0; i < Panel.Count; i++) {
            Panel[i].SetActive(false);
        }
        //PanelAnimator[0].SetTrigger("TriggerOption");
        Panel[0].SetActive(true);
        Panel[0].transform.SetAsFirstSibling();
    }

    public void DisplayCredit() {
        for (int i = 0; i < Panel.Count; i++) {
            Panel[i].SetActive(false);
        }
        Panel[1].SetActive(true);
    }

    public void DisplayMainMenu() {
        for (int i = 0; i < Panel.Count; i++) {
            Panel[i].SetActive(false);
            for (int j = 0; j < TextButton.Count; j++) {
                TextButton[j].color = new Color(50, 50, 50, 0);

            }
        }
        Panel[2].SetActive(true);
        mainMenuManager.displayMainMenuTools();
    }

    public void LoadTutorial() {
        SceneManager.LoadScene("Tutorial");
    }

    public void Quit() {
        foreach (Data data in datas)
            data.ResetValues();
        Application.Quit();
    }

    public void LoadBakery() {
        foreach (Data data in datas)
            data.ResetValues();
        SceneManager.LoadScene("FirstBakery");
    }

}
