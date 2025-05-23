using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TabsManagement : MonoBehaviour {
    [SerializeField] private List<GameObject> panels;
    [SerializeField] private List<GameObject> tabs;
    //[SerializeField] private GameObject statisticTab;
    [SerializeField] private Controller controller;
    [SerializeField] private Day day;
    [SerializeField] private PlayerControllerSO playerController;
    [SerializeField] internal bool canChangeTab = true;

    [Header("ButtonAnimation")]
    [SerializeField] private Vector3 normalScale;
    [SerializeField] private Vector3 BigScale;
    [SerializeField] private float AnimationLenght;

    private GameObject currentPanel;
    private int currentPanelIndex = 0;
    private Color normalColor = new Color(1,1, 1, 1);
    private Color selectedColor = new Color(0.8f,0.8f, 0.8f, 1);

    private void Awake() {
        day.DayTimeChange += EnableStatistic;
    }

    private void Start() {
        playerController.GetPlayerController().playerInput.Tabs.NextTab.performed += NextTab;
        playerController.GetPlayerController().playerInput.Tabs.PreviousTab.performed += PreviousTab;
    }

    private void EnableStatistic() {
        //if (day.GetDayTime() == DayTime.Evening)
        //    statisticTab.SetActive(true);
    }


    private void OnEnable() {
        currentPanel = panels[currentPanelIndex];
        currentPanel.SetActive(true);
        tabs[currentPanelIndex].GetComponentInChildren<Image>().color = selectedColor;

        //if (controller.IsGamepad()) {
        //    StartCoroutine(WaitForGamepad());
        //}

        playerController.GetPlayerController().playerInput.Tabs.Enable();
    }

    private void NextTab(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            if (canChangeTab)
            {
                print("tabs changed");
                panels[currentPanelIndex].SetActive(false);

                tabs[currentPanelIndex].GetComponentInChildren<Image>().color = normalColor;

                if (currentPanelIndex == panels.Count - 1)
                    currentPanelIndex = 0;
                else
                    currentPanelIndex++;
                panels[currentPanelIndex].SetActive(true);
                tabs[currentPanelIndex].GetComponentInChildren<Image>().color = selectedColor;
                //controller.SetEventSystemToStartButton(tabs[currentPanelIndex]);
            }
        }
    }

    private void PreviousTab(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            if (canChangeTab)
            {
                print("tabs changed");
                panels[currentPanelIndex].SetActive(false);
                tabs[currentPanelIndex].GetComponentInChildren<Image>().color = normalColor;
                if (currentPanelIndex == 0)
                    currentPanelIndex = panels.Count - 1;
                else
                    currentPanelIndex--;
                panels[currentPanelIndex].SetActive(true);
                tabs[currentPanelIndex].GetComponentInChildren<Image>().color = selectedColor;
                //controller.SetEventSystemToStartButton(tabs[currentPanelIndex]);
            }
        }
    }

    private IEnumerator WaitForGamepad() {
        yield return new WaitForEndOfFrame();
        controller.SetEventSystemToStartButton(tabs[currentPanelIndex]);
    }

    private void OnDisable() {
        playerController.GetPlayerController().playerInput.Tabs.Disable();
    }

    private void OnDestroy() {
        playerController.GetPlayerController().playerInput.Tabs.NextTab.performed -= NextTab;
        playerController.GetPlayerController().playerInput.Tabs.PreviousTab.performed -= PreviousTab;
    }

    public void ShowPanel(GameObject panel) {
        currentPanel.SetActive(false);
        panel.SetActive(true);
        currentPanel = panel;
    }
}
