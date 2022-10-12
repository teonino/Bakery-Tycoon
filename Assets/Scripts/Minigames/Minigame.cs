using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class Minigame : MonoBehaviour {
    
    protected WorkstationManager workplacePanel; 

    protected PlayerController controller;
    protected float launchTime;
    protected float endTime;

    // Start is called before the first frame update
    protected void Start() {
        workplacePanel = transform.parent.gameObject.GetComponent<WorkstationManager>();
        controller = FindObjectOfType<PlayerController>();
        launchTime = Time.time;
    }

    protected void End() {
        endTime = Time.time;
        workplacePanel.MinigameComplete();
    }

    protected float GetTimer() => endTime - launchTime; //Return time taken to complete the minigame
}
