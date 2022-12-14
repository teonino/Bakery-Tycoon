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
    [SerializeField] private CartUI cartPanel;
    [SerializeField] private GameObject computerPanel;
    [SerializeField] private ListIngredient ingredients;
    [SerializeField] private PlayerControllerSO playerControllerSO;
    [SerializeField] private Controller controller;
    [SerializeField] private int scrollSpeed;
    [SerializeField] private OrderQuest orderQuest;
    [SerializeField] private GameObject scroll;

    private RectTransform scrollRectTransform;
    private PlayerController playerController;
    private List<GameObject> ingredientButtonList;
    private List<GameObject> ingredientRackList;
    private int nbButton = 0;
    private int lenght;
    private float cartWeight = 0;
    private int cartCost = 0;
    private int maxButtonInRack;

    public Dictionary<IngredientSO, int> cart;

    void Awake() {
        scrollRectTransform = scroll.GetComponent<RectTransform>();
        ingredientRackList = new List<GameObject>();
        ingredientButtonList = new List<GameObject>();

        playerController = playerControllerSO.GetPlayerController();

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
        lenght = ingredients.GetIngredientLenght();

        //Init Cart
        if (cart == null)
            InitCart();

        //Instantiate buttons
        for (int i = 0; i < lenght; i++) {
            ingredientButtonAsset.InstantiateAsync().Completed += (go) => {
                go.Result.GetComponent<DeliveryButton>().deliveryManager = this;
                go.Result.GetComponent<DeliveryButton>().SetIngredient(ingredients.GetIngredientList()[nbButton].ingredient);
                ingredientButtonList.Add(go.Result);
                nbButton++;
                SetupRacks();
            };
        }
    }

    void SetupRacks() {
        if (ingredientButtonList.Count > 0 && nbButton == lenght) {
            maxButtonInRack = (int)Math.Floor(scroll.GetComponent<RectTransform>().rect.width / ingredientButtonList[0].GetComponent<RectTransform>().sizeDelta.x);
            for (int i = 0; i < ingredientButtonList.Count; i++) {
                if (i % maxButtonInRack == 0) {
                    ingredientRackAsset.InstantiateAsync(scroll.transform).Completed += (go) => {
                        ingredientRackList.Add(go.Result);
                        SetupButtons();
                    };
                }
            }
        }
    }

    void SetupButtons() {
        if (ingredientRackList.Count * maxButtonInRack >= ingredientButtonList.Count) {
            scrollRectTransform.sizeDelta = new Vector2(scroll.GetComponent<RectTransform>().rect.width, ingredientRackList[0].GetComponent<RectTransform>().rect.height * ingredientRackList.Count);
            for (int i = 0; i < lenght; i++) {
                for (int j = 0; j < lenght; j++) {
                    if (j / maxButtonInRack < ingredientRackList.Count) {
                        ingredientButtonList[j].transform.SetParent(ingredientRackList[j / maxButtonInRack].transform);
                        ingredientButtonList[j].transform.localScale = Vector3.one;
                    }
                }
            }
        }
    }

    private void Update() {
        if (controller.IsGamepad()) {
            scrollRectTransform.position -= new Vector3(0, playerControllerSO.GetPlayerController().playerInput.UI.ScrollWheel.ReadValue<Vector2>().y * scrollSpeed, 0);
        }
    }

    public void SetIngredient(IngredientSO ingredient, int amount) {
        cart[ingredient] = amount;

        orderQuest.CheckOrder(ingredient, cart[ingredient]);

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

    private void InitCart() {
        cart = new Dictionary<IngredientSO, int>();
        foreach (StockIngredient stockIngredient in ingredients.GetIngredientList()) {
            cart.Add(stockIngredient.ingredient, 0);
        }
    }

    public void DisplayCart() {
        CartUI currentCart = cartPanel;
        currentCart.cart = cart;
        currentCart.cartWeight = cartWeight;
        currentCart.cartCost = cartCost;
        currentCart.deliveryManager = this;
        currentCart.InitCart();
    }

    public void ResetCart() {
        cart.Clear();
        cartPanel.ClearText();
        InitCart();
        cartWeight = 0;
        cartCost = 0;
        foreach (GameObject go in ingredientButtonList)
            go.GetComponentInChildren<AmmountManager>().ResetAmount();
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

    public IEnumerator UpdateStockButtons(int secondDelay) {
        yield return new WaitForSeconds(secondDelay);
        for (int i = 0; i < ingredientButtonList.Count; i++)
            ingredientButtonList[i].GetComponent<DeliveryButton>().UpdateStock();
    }

    public void Quit(InputAction.CallbackContext context) {
        playerController.playerInput.UI.Quit.performed -= Quit;
        playerController.playerInput.UI.Disable();
        playerController.EnableInput();
        computerPanel.SetActive(false);
    }
}
