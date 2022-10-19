using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class DeliveryManager : MonoBehaviour {
    [SerializeField] private AssetReference ingredientButtonAsset;
    [SerializeField] private AssetReference ingredientRackAsset;
    [SerializeField] private AssetReference stockAsset;
    [SerializeField] private GameObject content;
    [SerializeField] private Cart cartPanel;

    private GameManager gameManager;
    private List<GameObject> ingredientButtonList;
    private List<GameObject> ingredientRackList;
    private int nbButton = 0;

    public Dictionary<IngredientSO, int> cart;
    float weightCart = 0;

    void Start() {
        //Get references
        gameManager = FindObjectOfType<GameManager>();
        cart = new Dictionary<IngredientSO, int>();

        //Manage Inputs
        gameManager.playerController.DisableInput();
        gameManager.playerController.playerInput.UI.Enable();
        gameManager.playerController.playerInput.UI.Quit.performed += Quit;

        //Init Cart
        InitCart();

        //Instantiate buttons
        ingredientRackList = new List<GameObject>();
        ingredientButtonList = new List<GameObject>();
        for (int i = 0; i < gameManager.GetLenghtIngredients(); i++) {
            ingredientButtonAsset.InstantiateAsync().Completed += (go) => {
                go.Result.GetComponent<DeliveryButton>().deliveryManager = this;
                go.Result.GetComponent<DeliveryButton>().SetIngredient(gameManager.ingredientLists[nbButton].ingredient);
                ingredientButtonList.Add(go.Result);
                nbButton++;
                SetupButtons();
            };

            if (i % 4 == 0) {
                ingredientRackAsset.InstantiateAsync(content.transform).Completed += (go) => {
                    ingredientRackList.Add(go.Result);
                    SetupButtons();
                };
            }
        }
    }

    public void AddIngredient(IngredientSO ingredient, int amount) {
        cart[ingredient] += amount;
        weightCart += ingredient.weight * amount;
        DisplayCart();
    }

    void SetupButtons() {
        if (ingredientRackList.Count > 0 && nbButton == gameManager.GetLenghtIngredients()) {
            for (int i = 0; i < gameManager.GetLenghtIngredients(); i++) {
                ingredientButtonList[i].transform.SetParent(ingredientRackList[i / 4].transform);
                ingredientButtonList[i].transform.localScale = Vector3.one;
            }
        }
    }

    private void InitCart() {
        foreach (StockIngredient stockIngredient in gameManager.ingredientLists) {
            cart.Add(stockIngredient.ingredient, 0);
        }
    }

    public void Reset() {
        cart.Clear();
        cartPanel.ClearText();
        InitCart();
        weightCart = 0;
    }

    public void DisplayCart() {
        Cart currentCart = cartPanel;
        currentCart.cart = cart;
        currentCart.cartWeight = weightCart;
        currentCart.deliveryManager = this;
        currentCart.InitCart();

    }

    public void DisplayStock() {
        stockAsset.InstantiateAsync(transform).Completed += (go) => {
            go.Result.GetComponent<Warehouse>().deliveryManager = this;
            gameManager.playerController.playerInput.UI.Quit.performed -= Quit;
            gameManager.playerController.playerInput.UI.Quit.performed += go.Result.GetComponent<Warehouse>().Quit;
        };
    }

    public void Quit(InputAction.CallbackContext context) {
        gameManager.playerController.playerInput.UI.Quit.performed -= Quit;
        gameManager.playerController.playerInput.UI.Disable();
        gameManager.playerController.EnableInput();
        foreach (GameObject go in ingredientButtonList)
            Addressables.ReleaseInstance(go);
        if (gameObject)
            Addressables.ReleaseInstance(gameObject);
    }
}
