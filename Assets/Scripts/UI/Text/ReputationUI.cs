using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReputationUI : MonoBehaviour
{
    public void SetReputation(int reputation) {
        GetComponent<TextMeshProUGUI>().SetText("Reputation : " + reputation);
    }
}
