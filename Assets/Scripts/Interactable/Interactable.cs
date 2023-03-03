using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {

    [SerializeField] protected PlayerControllerSO playerControllerSO;
    protected bool canInterract = false;

    public abstract void Effect();
    public abstract bool CanInterract();
}
