using System;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    [SerializeField] private bool daySystemEnable;
    [SerializeField] private int startLightRotation;
    [SerializeField] private int endLightRotation;
    [SerializeField] private int secondBeforeLightMovement;
    [SerializeField] private Day day;
    [SerializeField] private Light[] MuralLight;


    private Light light;
    private float timeElapsed;
    private int duration;
    private Action displaySkipButton;
    private float initialColorTemperature;
    private float targetColorTemperature = 5500;

    void Start()
    {
        displaySkipButton = FindObjectOfType<SkipDayButton>().DisplayButton;
        displaySkipButton();
        duration = day.GetMorningDuration() + day.GetDayDuration();
        light = GetComponent<Light>();
        initialColorTemperature = light.colorTemperature;
        UpdateLightList();
    }

    void FixedUpdate()
    {
        if (daySystemEnable)
        {
            if (timeElapsed < duration)
            {
                transform.rotation = Quaternion.Euler(Vector3.right * Mathf.Lerp(startLightRotation, endLightRotation, timeElapsed / duration));
                timeElapsed += Time.deltaTime;

                if (day.GetDayTime() == DayTime.Morning)
                {
                    if ((day.GetMorningDuration() - timeElapsed ) <= secondBeforeLightMovement)
                    {
                        light.colorTemperature = Mathf.Lerp(targetColorTemperature, initialColorTemperature, (day.GetMorningDuration() - timeElapsed) / secondBeforeLightMovement);
                        light.shadowStrength = Mathf.Lerp(0, 1, (day.GetMorningDuration() - timeElapsed) / secondBeforeLightMovement);
                    }
                }
                else if (day.GetDayTime() == DayTime.Day)
                {
                    if ((day.GetDayDuration() - timeElapsed) <= secondBeforeLightMovement)
                    {
                        light.colorTemperature = Mathf.Lerp(targetColorTemperature, initialColorTemperature, (duration - timeElapsed) / secondBeforeLightMovement);
                        light.shadowStrength = Mathf.Lerp(1, 0, (duration - timeElapsed) / secondBeforeLightMovement);
                    }

                    for (int i = 0; i < MuralLight.Length; i++)
                    {
                        float time = 1.2f;
                        MuralLight[i].intensity = Mathf.Lerp(MuralLight[i].intensity, 0, time * Time.deltaTime);
                    }
                }
                else if(day.GetDayTime() == DayTime.Evening)
                {
                    for (int i = 0; i < MuralLight.Length; i++)
                    {
                        float time = 1.2f;
                        MuralLight[i].intensity = Mathf.Lerp(MuralLight[i].intensity, 0, time * Time.deltaTime);
                    }
                }


                    if (timeElapsed > day.GetMorningDuration() && day.GetDayTime() == DayTime.Morning)
                {
                        Updateday();
                        initialColorTemperature = light.colorTemperature;
                        targetColorTemperature = 3000;
                }


            }
            else if (day.GetDayTime() == DayTime.Day)
            {
                Updateday();

            }
        }
    }

    public void Updateday()
    {
        day.OnNextDayPhase();
        if (day.GetDayTime() == DayTime.Evening)
            displaySkipButton();
    }

    public void UpdateLightList()
    {
        GameObject[] MuralLightObject = GameObject.FindGameObjectsWithTag("Light");
        MuralLight = new Light[MuralLightObject.Length];
        for(int i = 0; i < MuralLightObject.Length; i++)
        {
            MuralLight[i] = MuralLightObject[i].GetComponent<Light>();
        }
    }

}
