using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CommandRecap : MonoBehaviour {
    private int time;
    private AICustomer customer;
    private CustomerWaitingTime waitingTimeSO;
    [SerializeField] private TextMeshProUGUI text;

    public void StartCoroutineText() {
        time = waitingTimeSO.GetWaitingTime();
        StartCoroutine(SetText());
    }
    private IEnumerator SetText() {
        text.text = customer.requestedProduct.name + " / " + GetTime();


        yield return new WaitForSeconds(1);
        time--;
        StartCoroutine(SetText());
    }

    private string GetTime() {
        string s = "";

        if (time / 60 > 0) {
            s += time / 60 + "";
            if (time % 60 >= 10)
                s += "." + time % 60 ;
            else
                s += ".0" + time % 60;
        }
        else 
            s += time % 60 + "";

        return s + "s";

    }

    public void SetWaitingTime(CustomerWaitingTime waitingTime) => waitingTimeSO = waitingTime;
    public void SetCustomer(AICustomer customer) => this.customer = customer;
    public AICustomer GetCustomer() => customer;

    private void OnDestroy() {
        StopAllCoroutines();
    }
}
