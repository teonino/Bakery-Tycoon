using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReputationUI : MonoBehaviour {
    [SerializeField] Reputation reputation;

    private void Start() {
        reputation.SetUpdateUI(SetReputation);
        SetReputation();
    }

    public void SetReputation() {
        GetComponent<TextMeshProUGUI>().SetText("Reputation Lv " + reputation.GetLevel() + 1  + " : " + reputation.GetExperience() + " / " + reputation.GetExpNeeded());
    }
}
