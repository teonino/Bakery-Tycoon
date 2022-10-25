using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {

    protected GameManager gameManager;
    protected PlayerController playerController;

    protected void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        playerController = FindObjectOfType<PlayerController>();
    }
    public abstract void Effect();
}
