using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {

    protected PlayerController playerController;

    protected void Awake() {
        playerController = FindObjectOfType<PlayerController>();
    }
    public abstract void Effect();
}
