using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour {
    [SerializeField] private Money money;

    private void Start() {
        UpdateUI(money.GetMoney());
    }

    private void OnEnable() {
        money.OnMoneyChanged += UpdateUI;
    }

    private void OnDisable() {
        money.OnMoneyChanged -= UpdateUI;
    }

    private void UpdateUI(int money) {
        GetComponent<TextMeshProUGUI>().text = $"Money : {money}€";
    }
}
