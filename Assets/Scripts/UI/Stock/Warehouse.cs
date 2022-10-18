using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class Warehouse : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stockText;
    private GameManager gameManager;

    [HideInInspector]
    public DeliveryManager deliveryManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        stockText.SetText("");
        foreach (StockIngredient stockIngredient in gameManager.ingredientLists) {
            if(stockIngredient.amount > 0)
                stockText.text += stockIngredient.ingredient.name + " x" + stockIngredient.amount + "\n";
        }
    }

    public void Quit(InputAction.CallbackContext context) {
        gameManager.playerController.playerInput.UI.Quit.performed -= Quit;
        gameManager.playerController.playerInput.UI.Quit.performed += deliveryManager.Quit;
        if (gameObject)
            Addressables.ReleaseInstance(gameObject);
    }
}
