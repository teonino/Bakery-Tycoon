using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatisticUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI content;
    private GameManager gameManager;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Enable() {
        gameObject.SetActive(true);
    }

    private void OnEnable() {
        DayStatistics stats = gameManager.GetDayStatistics();

        content.text = "Most product solds : " + stats.GetMostProductSold().Key + " (" + stats.GetPercentageAmongAllProduct() + "%)\n";
        content.text += "Product Solds : " + stats.GetMostProductSold().Value + "\n";
        content.text += "Low Quantity ressources : " + stats.GetLowestIngredient() + "\n";
        content.text += "Money spent : " + stats.GetMoneySpent() + "\n";
        content.text += "Money won : " + stats.GetMoneyEarned() + "\n";
        content.text += "Total : " + (stats.GetMoneyEarned() - stats.GetMoneySpent()) + "\n";
    }
}
