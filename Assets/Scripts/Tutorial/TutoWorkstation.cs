using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoWorkstation : Workstation {
    [Header("Tutorial Parameters")]
    [SerializeField] private Tutorial tutorial;
    [SerializeField] private InterractQuest interractQuest;
    [SerializeField] private InterractQuest secondInterractQuest;
    public override void Effect() {
        if (tutorial.CanInterractWorkstation()) {
            base.Effect();
            interractQuest?.OnInterract();
            secondInterractQuest?.OnInterract();
        }
    }

    public override void CloseWorkplace(GameObject go) {
        base.CloseWorkplace(go);
        tutorial.Invoke();
    }
}
