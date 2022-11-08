using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DeliveryManager : MonoBehaviour {
    [SerializeField] private AssetReference ingredientButtonAsset;
    [SerializeField] private AssetReference ingredientRackAsset;
    [SerializeField] private Cart cartPanel;
    [SerializeField] private GameObject computerPanel;

    private GameManager gameManager;
    private PlayerController playerController;
    private GameObject content;
    private List<GameObject> ingredientButtonList;
    private List<GameObject> ingredientRackList;
    private int nbButton = 0;
    private int lenght;
    private float cartWeight = 0;
    private int cartCost = 0;

    public Dictionary<IngredientSO, int> cart;

    void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        content = GetComponentInChildren<VerticalLayoutGroup>().gameObject;
        ingredientRackList = new List<GameObject>();
        ingredientButtonList = new List<GameObject>();

        lenght = gameManager.GetIngredientsLenght();
        playerController = gameManager.GetPlayerController();

    }

    private void OnEnable() {
        if (gameObject.activeSelf) {
            //Manage Inputs
            playerController.DisableInput();
            playerController.playerInput.UI.Enable();
            playerController.playerInput.UI.Quit.performed += Quit;
        }
    }

    private void Start() {
        //Init Cart
        if (cart == null)
            InitCart();

        //Instantiate buttons
        for (int i = 0; i < lenght; i++) {
            ingredientButtonAsset.InstantiateAsync().Completed += (go) => {
                go.Result.GetComponent<DeliveryButton>().deliveryManager = this;
                go.Result.GetComponent<DeliveryButton>().SetIngredient(gameManager.GetIngredientList()[nbButton].ingredient);
                go.Result.name = "Delivery Button " + gameManager.GetIngredientList()[nbButton].ingredient;
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

    public void SetIngredient(IngredientSO ingredient, int amount) {
        cart[ingredient] = amount;
        CalculateCartCostAndWeight();
        DisplayCart();
    }

    private void CalculateCartCostAndWeight() {
        cartCost = 0;
        cartWeight = 0;
        foreach (KeyValuePair<IngredientSO, int> ingredient in cart) {
            cartCost += ingredient.Key.price * ingredient.Value;
            cartWeight += ingredient.Key.weight * ingredient.Value;
        }
    }

    void SetupButtons() {
        if (ingredientRackList.Count > 0 && nbButton == lenght) {
            int maxButtonInRack = (int)Math.Floor(content.GetComponent<RectTransform>().rect.x / ingredientButtonList[0].GetComponent<RectTransform>().rect.x) - 1;
            for (int i = 0; i < lenght; i++) {
                if (i / maxButtonInRack < ingredientRackList.Count) {
                    ingredientButtonList[i].transform.SetParent(ingredientRackList[i / maxButtonInRack].transform);
                    ingredientButtonList[i].transform.localScale = Vector3.one;
                }
            }
        }
    }

    private void InitCart() {
        cart = new Dictionary<IngredientSO, int>();
        foreach (StockIngredient stockIngredient in gameManager.GetIngredientList()) {
            cart.Add(stockIngredient.ingredient, 0);
        }
    }


    public void DisplayCart() {
        Cart currentCart = cartPanel;
        currentCart.cart = cart;
        currentCart.cartWeight = cartWeight;
        currentCart.cartCost = cartCost;
        currentCart.deliveryManager = this;
        currentCart.InitCart();
    }


    //public void DisplayStock() {
    //    stockAsset.InstantiateAsync(transform).Completed += (go) => {
    //        go.Result.GetComponent<Warehouse>().deliveryManager = this;
    //        gameManager.playerController.playerInput.UI.Quit.performed -= Quit;
    //        gameManager.playerController.playerInput.UI.Quit.performed += go.Result.GetComponent<Warehouse>().Quit;
    //    };
    //}


    public void ResetCart() {
        cart.Clear();
        cartPanel.ClearText();
        InitCart();
        cartWeight = 0;
        cartCost = 0;
    }


    public void Reset(bool resetCart) {
        nbButton = 0;

        foreach (GameObject go in ingredientButtonList)
            Addressables.ReleaseInstance(go);
        ingredientButtonList.Clear();

        foreach (GameObject go in ingredientRackList)
            Addressables.ReleaseInstance(go);
        ingredientRackList.Clear();

        if (resetCart)
            ResetCart();
    }

    public void Quit(InputAction.CallbackContext context) {
        playerController.playerInput.UI.Quit.performed -= Quit;
        playerController.playerInput.UI.Disable();
        playerController.EnableInput();

        computerPanel.SetActive(false);
    }
}
