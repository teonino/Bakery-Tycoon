using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayTimeUI : MonoBehaviour {

    TextMeshProUGUI text;
    int secondRemaining;
    DayTime dayTime;

    private void Start() {
        text = GetComponent<TextMeshProUGUI>();
    }
    public void StartDayTime(DayTime dayTime, int seconds) {
        this.dayTime = dayTime;
        secondRemaining = seconds;
        if (dayTime == DayTime.Morning) {
            StartCoroutine(TimeRemaining());
        }
        else {
            text.SetText(GetDay());
        }
    }

    private IEnumerator TimeRemaining() {
        text.SetText(GetDay() + " " + secondRemaining / 60 + ":");
        if (secondRemaining % 60 < 10)
            text.text += "0" + secondRemaining % 60;
        else
            text.text += secondRemaining % 60;

        yield return new WaitForSeconds(1);
        secondRemaining--;
        if (secondRemaining > 0)
            StartCoroutine(TimeRemaining());
    }

    public string GetDay() {
        string s = "";
        switch (dayTime) {
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
}
