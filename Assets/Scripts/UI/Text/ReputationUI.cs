using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReputationUI : MonoBehaviour {
    [SerializeField] Reputation reputation;
    [SerializeField] private TextMeshProUGUI ReputationLevel;
    [SerializeField] private Image progressBar;
    public TextMeshProUGUI debugText;

    private void OnEnable() {
        reputation.UpdateUI += SetReputation;
        SetReputation();
    }
    private void OnDisable() {
        reputation.UpdateUI -= SetReputation;
    }
    public Reputation GetReputation() => reputation;

    private void Update()
    {
        SetReputation();
    }

    public void SetReputation() {
        float a = reputation.GetExperience(), b = reputation.GetExpNeeded();
        float dividedRep = a / b;
        progressBar.fillAmount = dividedRep;
        print(dividedRep);
        ReputationLevel.SetText(reputation.GetLevel().ToString());
        //debugText.SetText("Reputation Lv " + reputation.GetLevel() + 1  + " : " + reputation.GetExperience() + " / " + reputation.GetExpNeeded());
    }
}
