using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

[System.Serializable]
public class MinigameInfo {
    [Space(10)]
    [Header("Function Settings")]
    public AssetReference minigameAsset;

    [Header("Minigame Parameters")]
    [Space(10)]
    public float launchTime;
    public float endTime;

    public MinigameInfo(float launchTime, float endTime) {
        this.launchTime = launchTime;
        this.endTime = endTime;
    }

    public void InitMinigame() {
        minigameAsset.InstantiateAsync();
    }
}
