using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReputationUI : MonoBehaviour {
    public void SetReputation(Reputation reputation, int experienceNeeded) {
        GetComponent<TextMeshProUGUI>().SetText("Reputation Lv " + reputation.level + 1  + " : " + reputation.experience + " / " + experienceNeeded);
    }
}
