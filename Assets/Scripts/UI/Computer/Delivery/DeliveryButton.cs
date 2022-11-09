using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class DeliveryButton : MonoBehaviour {
    [SerializeField] private Button button;
    [SerializeField] private AssetReference descriptionPanel;
    [SerializeField] private IngredientSO ingredient;
    [HideInInspector] public DeliveryManager deliveryManager;

    private GameManager gameManager;
    //Start is called before the first frame update
    void Start() {
        gameManager = FindObjectOfType<GameManager>();
        GetComponentInChildren<TextMeshProUGUI>().SetText(ingredient.name);
        GetComponentInChildren<RawImage>().texture = ingredient.image;
        button.onClick.AddListener(delegate {
            descriptionPanel.InstantiateAsync(deliveryManager.transform).Completed += (go) => {
                gameManager.RegisterCurrentSelectedButton(); //gameObject.GetComponentInChildren<Button>().gameObject
                go.Result.GetComponent<IngredientDescription>().gameManager = gameManager;
                go.Result.GetComponent<IngredientDescription>().deliveryManager = deliveryManager;
                go.Result.GetComponent<IngredientDescription>().ingredient = ingredient;
                CartUI cart = FindObjectOfType<CartUI>();
                if (cart.cart != null && cart.cart.ContainsKey(ingredient))
                    go.Result.GetComponent<IngredientDescription>().nbIngredient = cart.cart[ingredient];
            };
        });
    }

    public void SetIngredient(IngredientSO ingredient) => this.ingredient = ingredient;
}
