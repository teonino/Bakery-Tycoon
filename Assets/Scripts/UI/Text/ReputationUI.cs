using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReputationUI : MonoBehaviour {
    [SerializeField] Reputation reputation;

    private void Start() {
        TmpBuild.instance.reputation.SetUpdateUI(SetReputation);
        SetReputation();
    }

    public void SetReputation() {
        GetComponent<TextMeshProUGUI>().SetText("Reputation Lv " + TmpBuild.instance.reputation.GetLevel() + 1  + " : " + TmpBuild.instance.reputation.GetExperience() + " / " + TmpBuild.instance.reputation.GetExpNeeded());
    }
}
