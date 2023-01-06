using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabsManagement : MonoBehaviour {
    [SerializeField] private GameObject firstPanel;
    [SerializeField] private GameObject firstTab;
    [SerializeField] private GameObject statisticTab;
    [SerializeField] private Controller controller;
    [SerializeField] private Day day;

    private GameObject currentPanel;

    private void Awake() {
        day.DayTimeChange += EnableStatistic;
    }

    private void EnableStatistic() {
        if (day.GetDayTime() == DayTime.Evening)
            statisticTab.SetActive(true);
    }


    private void OnEnable() {
        currentPanel = firstPanel;
        currentPanel.SetActive(true);

        if (controller.IsGamepad()) {
            StartCoroutine(WaitForGamepad());
        }
    }

    private IEnumerator WaitForGamepad() {
        yield return new WaitForEndOfFrame();
        controller.SetEventSystemToStartButton(firstTab);
    }

    private void Update() {
        if (gameObject.transform.parent.gameObject.activeSelf && !controller.GetEventSystemCurrentlySelected()) {
            controller.SetEventSystemToStartButton(firstTab);
        }
    }

    public void ShowPanel(GameObject panel) {
        currentPanel.SetActive(false);
        panel.SetActive(true);
        currentPanel = panel;
    }
}
