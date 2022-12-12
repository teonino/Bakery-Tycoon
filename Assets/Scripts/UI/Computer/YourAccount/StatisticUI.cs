using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class StatisticUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI content;
    [SerializeField] private ListIngredient ingredients;
    [SerializeField] private Statistics stats;
    [SerializeField] private PlayerControllerSO playerControllerSO;
    [SerializeField] private GameObject computerPanel; 

    private PlayerController playerController;

    private void Awake() {
        playerController = playerControllerSO.GetPlayerController();
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
        content.text =  "Most product solds : " + stats.GetMostProductSold().Key + " (" + stats.GetPercentageAmongAllProduct() + "%)\n";
        content.text += "Product Solds : " + stats.GetMostProductSold().Value + "\n";
        content.text += "Low Quantity ressources : " + stats.GetLowestIngredient(ingredients) + "\n";
        content.text += "Money spent : " + stats.GetMoneySpent() + "\n";
        content.text += "Money won : " + stats.GetMoneyEarned() + "\n";
        content.text += "Total : " + (stats.GetMoneyEarned() - stats.GetMoneySpent()) + "\n";
    }

    public void Quit(InputAction.CallbackContext context) {
        playerController.playerInput.UI.Quit.performed -= Quit;
        playerController.playerInput.UI.Disable();
        playerController.EnableInput();

        computerPanel.SetActive(false);
    }
}
