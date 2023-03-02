using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommandRecap : MonoBehaviour {
    private float time;
    private float timeMax;
    private AICustomer customer;
    private CustomerWaitingTime waitingTimeSO;
    [SerializeField] private TextMeshProUGUI timeRemainingText;
    [SerializeField] private TextMeshProUGUI orderText;
    [SerializeField] private Image waitingImage;
    [SerializeField] private List<Sprite> backgroundSprite;

    public void StartCoroutineText() {
        time = waitingTimeSO.GetWaitingTime();
        timeMax = time;
        if(customer.isRegular())
        {
            this.gameObject.GetComponent<Image>().sprite = backgroundSprite[0];
        }
        else
        {
            this.gameObject.GetComponent<Image>().sprite = backgroundSprite[1];
        }
        StartCoroutine(SetText());
    }

    private IEnumerator SetText() {
        timeRemainingText.text = GetTime();
        orderText.text = customer.requestedProduct.name;
        waitingImage.fillAmount = time / timeMax;

        yield return new WaitForSeconds(1);
        time--;
        StartCoroutine(SetText());
    }

    private string GetTime() {
        string s = "";

        if (time / 60 > 0) {
            s += Mathf.Floor(time / 60) + "";
            if (time % 60 >= 10)
                s += ":" + time % 60 ;
            else
                s += ":0" + time % 60;
        }
        else 
            s += time % 60 + "";
        return s + " min";

    }

    public void SetWaitingTime(CustomerWaitingTime waitingTime) => waitingTimeSO = waitingTime;
    public void SetCustomer(AICustomer customer) => this.customer = customer;
    public AICustomer GetCustomer() => customer;

    private void OnDestroy() {
        StopAllCoroutines();
    }
}
