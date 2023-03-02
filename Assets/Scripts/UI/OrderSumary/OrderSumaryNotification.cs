using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class OrderSumaryNotification : MonoBehaviour {

    [SerializeField] private float offSetTime;
    private float time;
    public Action onDestroy;

    public void StartTimer() {
        StartCoroutine(DisplayTime());
    }

    private IEnumerator DisplayTime() {
        yield return new WaitForSeconds(time);
        onDestroy?.Invoke();
        yield return new WaitForSeconds(offSetTime);
        Addressables.ReleaseInstance(gameObject);
    }

    public void SetText(string v) {
        GetComponentInChildren<TextMeshProUGUI>().text = v;
    }

    public void SetTime(float timeDisplay) {
        time = timeDisplay;
    }
}
