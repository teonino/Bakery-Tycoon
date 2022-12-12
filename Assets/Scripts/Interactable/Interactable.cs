using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {

    [SerializeField] protected PlayerControllerSO playerControllerSO;

    public abstract void Effect();
}
