using System;
using System.Collections;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    [SerializeField] private bool daySystemEnable;
    [SerializeField] private int startLightRotation;
    [SerializeField] private int endLightRotation;
    [SerializeField] private Day day;
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
                    light.colorTemperature = Mathf.Lerp(initialColorTemperature, targetColorTemperature, timeElapsed / day.GetMorningDuration());
                    light.shadowStrength = Mathf.Lerp(1, 0, timeElapsed / day.GetMorningDuration());
                }
                else if (day.GetDayTime() == DayTime.Day)
                {
                    light.colorTemperature = Mathf.Lerp(initialColorTemperature, targetColorTemperature, (timeElapsed - day.GetMorningDuration()) / duration);
                    light.shadowStrength = Mathf.Lerp(0, 1, (timeElapsed - day.GetMorningDuration()) / duration);
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
}
