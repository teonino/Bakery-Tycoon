using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour {

    private GameManager gameManager;
    
    [SerializeField] private bool daySystemEnable;
    [SerializeField] private int startLightRotation;
    [SerializeField] private int endLightRotation;
    [SerializeField] private int morningDuration;
    [SerializeField] private int dayDuration;

    private float timeElapsed;
    private int duration;
    private Action<string> updateDayTimeUI;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();
        updateDayTimeUI = FindObjectOfType<DayTimeUI>().SetDayTime;
        updateDayTimeUI(GetDay());
        duration = morningDuration + dayDuration;
    }

    void FixedUpdate() {
        if (daySystemEnable) {
            if (timeElapsed < duration) {
                transform.rotation = Quaternion.Euler(Vector3.right * Mathf.Lerp(startLightRotation, endLightRotation, timeElapsed / duration));
                timeElapsed += Time.deltaTime;

                if (timeElapsed > morningDuration && gameManager.dayTime == DayTime.Morning)
                    Updateday();
            }
            else if (gameManager.dayTime == DayTime.Day)
                Updateday();
        }
    }
    public string GetDay() {
        string s = "";
        switch (gameManager.dayTime) {
            case DayTime.Morning:
                s = "Morning";
                break;
            case DayTime.Day:
                s = "Day";
                break;
            case DayTime.Evening:
                s = "Evening";
                break;
            default:
                break;
        }
        return s;
    }

    private void Updateday() {
        gameManager.dayTime++;
        updateDayTimeUI(GetDay());
    }
}
