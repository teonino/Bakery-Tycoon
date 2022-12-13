using System;
using UnityEngine;

public class DayManager : MonoBehaviour {
    [SerializeField] private bool daySystemEnable;
    [SerializeField] private int startLightRotation;
    [SerializeField] private int endLightRotation;
    [SerializeField] private Day day;
    private Light light;

    private float timeElapsed;
    private int duration;
    private Action displaySkipButton;

    void Start() {
        displaySkipButton = FindObjectOfType<SkipDayButton>().DisplayButton;
        displaySkipButton();
        duration = day.GetMorningDuration()+ day.GetDayDuration();
        light = GetComponent<Light>();
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

            //if(timeElapsed < duration / 2)
            //{
            //    light.shadowStrength = Mathf.Lerp(1, 0, timeElapsed / duration *2);
            //}
            //else
            //{
            //    float tmpTimeElapsed = timeElapsed - duration / 2;
            //    light.shadowStrength = Mathf.Lerp(0, 1, tmpTimeElapsed / duration*2);
            //}
            // Note a moi-même
            // 
            // 
        }
    }

    public void Updateday() {
        day.OnNextDayPhase();
        if (day.GetDayTime() == DayTime.Evening)
            displaySkipButton();
    }
}
