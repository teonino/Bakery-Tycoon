using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour {
    [SerializeField] private bool daySystemEnable;
    [SerializeField] private int startLightRotation;
    [SerializeField] private int endLightRotation;
    [SerializeField] private Day day;

    private float timeElapsed;
    private int duration;
    private Action displaySkipButton;

    void Start() {
        displaySkipButton = FindObjectOfType<SkipDayButton>().DisplayButton;
        displaySkipButton();
        duration = day.GetMorningDuration()+ day.GetDayDuration();
    }

    void FixedUpdate() {
        if (daySystemEnable) {
            if (timeElapsed < duration) {
                transform.rotation = Quaternion.Euler(Vector3.right * Mathf.Lerp(startLightRotation, endLightRotation, timeElapsed / duration));
                timeElapsed += Time.deltaTime;

                if (timeElapsed > day.GetMorningDuration() && day.GetDayTime() == DayTime.Morning)
                    Updateday();
            }
            else if (day.GetDayTime() == DayTime.Day)
                Updateday();
        }
    }

    public void Updateday() {
        day.OnNextDayPhase();
        if (day.GetDayTime() == DayTime.Evening)
            displaySkipButton();
    }
}
