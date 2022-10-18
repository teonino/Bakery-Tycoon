using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class DeliveryManager : MonoBehaviour {
    [SerializeField] private AssetReference ingredientButtonAsset;
    [SerializeField] private AssetReference cartAsset;
    [SerializeField] private AssetReference stockAsset;

    private GameManager gameManager;
    private List<GameObject> ingredientButtonList;
    private int nbButton = 0;

    public Computer computer;
    public Dictionary<IngredientSO, int> cart;
    float weightCart = 0;

    void Start() {
        //Get references
        gameManager = FindObjectOfType<GameManager>();
        ingredientButtonList = new List<GameObject>();
        cart = new Dictionary<IngredientSO, int>();

        //Manage Inputs
        gameManager.playerController.DisableInput();
        gameManager.playerController.playerInput.UI.Enable();
        gameManager.playerController.playerInput.UI.Quit.performed += Quit;

        //Init Cart
        InitCart();

        //Instantiate buttons
        for (int i = 0; i < gameManager.GetLenghtIngredients(); i++) {
            ingredientButtonAsset.InstantiateAsync(transform).Completed += (go) => { 
                go.Result.GetComponent<DeliveryButton>().stockmanager = this;
                go.Result.GetComponent<DeliveryButton>().SetIngredient(gameManager.ingredientLists[nbButton].ingredient);
                ingredientButtonList.Add(go.Result);
                SetupButtons();
            };
        }
    }

    public void AddIngredient(IngredientSO ingredient) {
        cart[ingredient]++;
        weightCart += ingredient.weight;
    }

    void SetupButtons() {
        if(nbButton == gameManager.GetLenghtIngredients() - 1) {
            for(int i = 0; i < gameManager.GetLenghtIngredients(); i++)
                ingredientButtonList[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(20 + 90 * (i % 4), -40 - (110 * (i / 4)) , 0);
        }
        nbButton++;
    }

    private void InitCart() {
        foreach (StockIngredient stockIngredient in gameManager.ingredientLists) {
            cart.Add(stockIngredient.ingredient, 0);
        }
    }

    public void Reset() {
        cart.Clear();
        InitCart();
        weightCart = 0;
    }

    public void DisplayCart() {
        cartAsset.InstantiateAsync(transform).Completed += (go) => {
            Cart currentCart = go.Result.GetComponent<Cart>();
            currentCart.cart = cart;
            currentCart.cartWeight = weightCart;
            currentCart.deliveryManager = this;
            gameManager.playerController.playerInput.UI.Quit.performed -= Quit;
            gameManager.playerController.playerInput.UI.Quit.performed += currentCart.Quit;
        };
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
