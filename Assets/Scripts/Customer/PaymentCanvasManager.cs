using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PaymentCanvasManager : MonoBehaviour {
    public int timeDisplaying = 2;
    public TextMeshProUGUI basePriceText;
    public TextMeshProUGUI bonusPriceText;

    // Start is called before the first frame update
    public void Init(int normal, int bonus) {
        int basePrice = normal;
        int bonusPrice = bonus;

        basePriceText.SetText(basePrice + "€");
        if (bonusPrice > 0)
            bonusPriceText.SetText("+ " +bonusPrice + "€");
        else
            bonusPriceText.SetText("");

        StartCoroutine(Lifespan());
    }

    private IEnumerator Lifespan() {
        yield return null;
        gameObject.SetActive(true);

        yield return new WaitForSeconds(timeDisplaying);
        Destroy(gameObject);
    }
}
