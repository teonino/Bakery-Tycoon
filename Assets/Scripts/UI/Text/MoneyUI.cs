using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private Money money;

    private void Start() {
        TmpBuild.instance.money.SetUpdateUI(SetMoney);
        SetMoney();
    }

    public void SetMoney() {
        GetComponent<TextMeshProUGUI>().SetText("Money : " + TmpBuild.instance.money.GetMoney() + "€");
    }
}
