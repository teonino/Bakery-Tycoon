using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Money", menuName = "Data/Money")]
public class Money : ScriptableObject
{
    [SerializeField] private int money;
    [SerializeField] private Statistics stats;
    [SerializeField] private Action UpdateUI;

    private void OnEnable() {
        money = 100;
    }

    public int GetMoney() => money;
    public void SetUpdateUI(Action updateUI) => UpdateUI = updateUI;
    public void AddMoney(int value) {
        money += value;
        UpdateUI?.Invoke();
        stats.AddMoney(value);
    }

    public void RemoveMoney(int value) {
        money -= value;
        UpdateUI?.Invoke();
        stats.RemoveMoney(value);
    }
}
