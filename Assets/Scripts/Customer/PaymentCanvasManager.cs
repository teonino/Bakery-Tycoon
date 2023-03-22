using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PaymentCanvasManager : MonoBehaviour {
    public int timeDisplaying = 2;

    [SerializeField] private GameObject basePricePanel;
    [SerializeField] private GameObject bonusPricePanel;
    public TextMeshProUGUI basePriceText;
    public TextMeshProUGUI bonusPriceText;

    // Start is called before the first frame update
    public void Init(int normal, int bonus) {
        int basePrice = normal;
        int bonusPrice = bonus;

        basePriceText.SetText(basePrice.ToString());
        if (bonusPrice > 0)
        {
            bonusPricePanel.SetActive(true);
            bonusPriceText.SetText("+ " + bonusPrice);
        }
        else
        {
            bonusPricePanel.SetActive(false);
            bonusPriceText.SetText("");
        }

        StartCoroutine(Lifespan());
    }

    private IEnumerator Lifespan() {
        yield return new WaitForEndOfFrame();
        basePricePanel.SetActive(true);
        basePriceText.gameObject.SetActive(true);
        yield return new WaitForSeconds(timeDisplaying);
        Destroy(gameObject);
    }
}
