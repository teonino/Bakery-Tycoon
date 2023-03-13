using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Money", menuName = "Data/Money")]
public class Money : Data
{
    [SerializeField] private int money;
    [SerializeField] private Statistics stats;
    [SerializeField] private SFX_SO sfx;

    public Action<int> OnMoneyChanged;


    public override void ResetValues() {
        money = 100;
    }

    public int GetMoney() => money;
    public void AddMoney(int value) {
        money += value;
        OnMoneyChanged?.Invoke(money);
        if (value > 0)
        {
            sfx.Invoke("Money");
            stats.AddMoney(value);
        }
        else
            stats.RemoveMoney(value);
    }

}
