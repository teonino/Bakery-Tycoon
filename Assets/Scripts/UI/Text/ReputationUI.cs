using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReputationUI : MonoBehaviour {
    [SerializeField] Reputation reputation;

    private void OnEnable() {
        reputation.UpdateUI += SetReputation;
        SetReputation();
    }
    private void OnDisable() {
        reputation.UpdateUI -= SetReputation;
    }
    public Reputation GetReputation() => reputation;

    public void SetReputation() {
        GetComponent<TextMeshProUGUI>().SetText("Reputation Lv " + reputation.GetLevel() + 1  + " : " + reputation.GetExperience() + " / " + reputation.GetExpNeeded());
    }
}
