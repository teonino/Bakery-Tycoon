using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

    [SerializeField] Slider readySlider;
    [SerializeField] Slider burningSlider;
    [SerializeField] Image readyFill;
    [SerializeField] Image burningFill;
    private float duration;
    private float burnDuration = 5;
    public Action onDestroy;
    public bool burned = false;

    private float timeElapsed = 0;

    void Update() {
        timeElapsed += Time.deltaTime;
        readySlider.value = Mathf.Lerp(0, 1, timeElapsed / duration);
        if (timeElapsed > duration) {
            readyFill.color = Color.green;
            burningSlider.gameObject.SetActive(true);
            burningSlider.value = Mathf.Lerp(0, 1, (timeElapsed - duration) / burnDuration);
            if (timeElapsed > duration + burnDuration) {
                burned = true;
                //Reduce reputation
            }
        }
    }

    public void SetDuration(int duration) => this.duration = duration;
}
