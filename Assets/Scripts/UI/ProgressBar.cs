using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

    [SerializeField] private float duration;
    public Action onDestroy;

    private float timeElapsed = 0;

    // Start is called before the first frame update
    void Update() {
        GetComponent<Slider>().value = Mathf.Lerp(0, 1, timeElapsed / duration);
        timeElapsed += Time.deltaTime;

        if (timeElapsed > duration) {
            if (onDestroy != null)
                onDestroy.Invoke();
            Addressables.ReleaseInstance(transform.parent.gameObject);
        }
    }

    public void SetDuration(int duration) => this.duration = duration;
}
