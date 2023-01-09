using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest")]
public abstract class Quest : ScriptableObject {
    [Header("Global Quest Parameters")]
    [SerializeField] protected string title;
    [SerializeField] private bool randomize;
    [SerializeField] private RewardType reward;
    [SerializeField] private int rewardAmount;
    [SerializeField] private Money money;
    [SerializeField] private Reputation reputation;
    [SerializeField] protected bool isActive = false;

    public Action OnCompletedAction;
    private enum RewardType { Reputation, Money }
    public void SetActive(bool active) => isActive = active;
    public void UpdateUI(TextMeshProUGUI text) => text.text = title;
    public string GetTitle() => title;
    public bool IsActive() => isActive;
    protected void OnCompleted() {
        if (reward == RewardType.Reputation)
            reputation.AddReputation(rewardAmount);
        else if (reward == RewardType.Money)
            money.AddMoney(rewardAmount);

        SetActive(false);
        OnCompletedAction?.Invoke();
    }
}
