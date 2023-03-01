using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

    [SerializeField] Image filledImage;
    private float duration;
    public Action onDestroy;
    private float timeElapsed = 0;

    void Update() {
        timeElapsed += Time.deltaTime;
        if (filledImage != null)
        {
            filledImage.fillAmount = Mathf.Lerp(0, 1, timeElapsed / duration);
            if (timeElapsed > duration)
            {
                onDestroy?.Invoke();
                Addressables.ReleaseInstance(gameObject);
            }
        }
    }

    public void SetDuration(int duration) => this.duration = duration;
}
