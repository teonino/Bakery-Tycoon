using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    public void SetMoney(int money) {
        GetComponent<TextMeshProUGUI>().SetText("Money : " + money + "€");
    }
}
