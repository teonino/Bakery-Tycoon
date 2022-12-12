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
        duration = TmpBuild.instance.day.GetMorningDuration()+ TmpBuild.instance.day.GetDayDuration();
    }

    void FixedUpdate() {
        if (daySystemEnable) {
            if (timeElapsed < duration) {
                transform.rotation = Quaternion.Euler(Vector3.right * Mathf.Lerp(startLightRotation, endLightRotation, timeElapsed / duration));
                timeElapsed += Time.deltaTime;

                if (timeElapsed > TmpBuild.instance.day.GetMorningDuration() && TmpBuild.instance.day.GetDayTime() == DayTime.Morning)
                    Updateday();
            }
            else if (TmpBuild.instance.day.GetDayTime() == DayTime.Day)
                Updateday();
        }
    }

    public void Updateday() {
        TmpBuild.instance.day.OnNextDayPhase();
        if (TmpBuild.instance.day.GetDayTime() == DayTime.Evening)
            displaySkipButton();
    }
}
