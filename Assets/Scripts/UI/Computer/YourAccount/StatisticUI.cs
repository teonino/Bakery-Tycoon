using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class StatisticUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI content;
    [SerializeField] private ListIngredient ingredients;
    [SerializeField] private Statistics stats;
    [SerializeField] private GameObject computerPanel;
    private GameManager gameManager;
    private PlayerController playerController;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        playerController = gameManager.GetPlayerController();
    }

    public void Enable() {
        gameObject.SetActive(true);
    }

    private void Start() { 
        playerController.DisableInput();
        playerController.playerInput.UI.Enable();
        playerController.playerInput.UI.Quit.performed += Quit;
    }

    private void OnEnable() {
        content.text = "Most product solds : " + TmpBuild.instance.stats.GetMostProductSold().Key + " (" + TmpBuild.instance.stats.GetPercentageAmongAllProduct() + "%)\n";
        content.text += "Product Solds : " + TmpBuild.instance.stats.GetMostProductSold().Value + "\n";
        content.text += "Low Quantity ressources : " + TmpBuild.instance.stats.GetLowestIngredient(TmpBuild.instance.ingredients) + "\n";
        content.text += "Money spent : " + TmpBuild.instance.stats.GetMoneySpent() + "\n";
        content.text += "Money won : " + TmpBuild.instance.stats.GetMoneyEarned() + "\n";
        content.text += "Total : " + (TmpBuild.instance.stats.GetMoneyEarned() - TmpBuild.instance.stats.GetMoneySpent()) + "\n";
    }

    public void Quit(InputAction.CallbackContext context) {
        playerController.playerInput.UI.Quit.performed -= Quit;
        playerController.playerInput.UI.Disable();
        playerController.EnableInput();

        computerPanel.SetActive(false);
    }
}
