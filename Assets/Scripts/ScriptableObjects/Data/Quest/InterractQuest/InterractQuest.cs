using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InterractQuest", menuName = "Quest/InterractQuest")]
public class InterractQuest : Quest {
    public void OnInterract() {
        if (isActive) {
            OnCompleted();
        }
    }
}
