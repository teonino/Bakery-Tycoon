using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TabsManagement : MonoBehaviour {
    [SerializeField] private List<GameObject> panels;
    [SerializeField] private List<GameObject> tabs;
    [SerializeField] private GameObject statisticTab;
    [SerializeField] private Controller controller;
    [SerializeField] private Day day;
    [SerializeField] private PlayerControllerSO playerController;

    private GameObject currentPanel;
    private int currentPanelIndex = 0;

    private void Awake() {
        day.DayTimeChange += EnableStatistic;
    }

    private void Start() {
        playerController.GetPlayerController().playerInput.Tabs.NextTab.performed += NextTab;
        playerController.GetPlayerController().playerInput.Tabs.PreviousTab.performed += PreviousTab;
    }

    private void EnableStatistic() {
        if (day.GetDayTime() == DayTime.Evening)
            statisticTab.SetActive(true);
    }


    private void OnEnable() {
        currentPanel = panels[currentPanelIndex];
        currentPanel.SetActive(true);

        if (controller.IsGamepad()) {
            StartCoroutine(WaitForGamepad());
        }

        playerController.GetPlayerController().playerInput.Tabs.Enable();
    }

    private void NextTab(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            panels[currentPanelIndex].SetActive(false);
            if (currentPanelIndex == panels.Count - 1)
                currentPanelIndex = 0;
            else
                currentPanelIndex++;
            panels[currentPanelIndex].SetActive(true);

            controller.SetEventSystemToStartButton(tabs[currentPanelIndex]);
        }
    }

    private void PreviousTab(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            panels[currentPanelIndex].SetActive(false);
            if (currentPanelIndex == 0)
                currentPanelIndex = panels.Count - 1;
            else
                currentPanelIndex--;
            panels[currentPanelIndex].SetActive(true);

            controller.SetEventSystemToStartButton(tabs[currentPanelIndex]);
        }
    }

    private IEnumerator WaitForGamepad() {
        yield return new WaitForEndOfFrame();
        controller.SetEventSystemToStartButton(tabs[currentPanelIndex]);
    }

    private void Update() {
        //if (gameObject.transform.parent.gameObject.activeSelf && !controller.GetEventSystemCurrentlySelected()) {
        //    controller.SetEventSystemToStartButton(tabs[currentPanelIndex]);
        //}
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
