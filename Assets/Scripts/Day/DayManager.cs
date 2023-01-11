using System;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour {
    [SerializeField] private bool daySystemEnable;
    [SerializeField] private int startLightRotation;
    [SerializeField] private int endLightRotation;
    [SerializeField] private int secondBeforeLightMovement;
    [SerializeField] private Day day;
    private Light[] MuralLight;


    private Light lightComponent;
    private float timeElapsed;
    private Action displaySkipButton;
    private float initialColorTemperature;
    //private float targetColorTemperature = 5500;
    //private float originalIntensity = 0.75f;
    //private float goalIntensity = 1.75f;

    void Start() {
        displaySkipButton = FindObjectOfType<SkipDayButton>().DisplayButton;
        displaySkipButton?.Invoke();
        lightComponent = GetComponent<Light>();
        initialColorTemperature = lightComponent.colorTemperature;
        UpdateLightList();
    }

    void FixedUpdate() {
        if (daySystemEnable) {
            if (timeElapsed < day.GetDayDuration()) {
                transform.rotation = Quaternion.Euler(Vector3.right * Mathf.Lerp(startLightRotation, endLightRotation, timeElapsed / day.GetDayDuration()));
                timeElapsed += Time.deltaTime;

                if (timeElapsed > day.GetDayDuration() && day.GetDayTime() == DayTime.Day) {
                    UpdateDay();
                    initialColorTemperature = lightComponent.colorTemperature;
                    //targetColorTemperature = 3000;
                }
            }

            else if (day.GetDayTime() == DayTime.Evening) {
                for (int i = 0; i < MuralLight.Length; i++) {
                    float time = 1.2f;
                    MuralLight[i].intensity = Mathf.Lerp(MuralLight[i].intensity, 1, time * Time.deltaTime);
                }
            }
        }
    }

    public void UpdateDay() {
        day.OnNextDayPhase();
        if (day.GetDayTime() == DayTime.Evening)
            displaySkipButton();
    }

    public void UpdateLightList() {
        GameObject[] MuralLightObject = GameObject.FindGameObjectsWithTag("Light");
        MuralLight = new Light[MuralLightObject.Length];
        for (int i = 0; i < MuralLightObject.Length; i++) {
            MuralLight[i] = MuralLightObject[i].GetComponent<Light>();
        }
    }
}
