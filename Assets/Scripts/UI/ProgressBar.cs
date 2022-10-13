using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {
    [SerializeField] private float duration;
    private CraftingStation craftingStation;
    private float timeElapsed = 0;

    // Start is called before the first frame update
    void Update() {
        GetComponent<Slider>().value = Mathf.Lerp(0, 1, timeElapsed / duration);
        timeElapsed += Time.deltaTime;

        if (timeElapsed > duration) {
            Addressables.ReleaseInstance(transform.parent.gameObject);
            craftingStation.Clean();
        }
    }

    public void SetDuration(int duration) => this.duration = duration;
    public void SetCraftingStation(CraftingStation craftingStation) => this.craftingStation = craftingStation;
}
