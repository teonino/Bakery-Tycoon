using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {

    [SerializeField] protected PlayerControllerSO playerControllerSO;
    protected bool canInterract = false;

    protected virtual void Start() {
        playerControllerSO = FindObjectOfType<PlayerController>().GetPlayerControllerSO();
    }

    public abstract void Effect();
    public abstract bool CanInterract();
}
