using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DeliveryManager : MonoBehaviour {
    [SerializeField] private AssetReference buttonAsset;
    [SerializeField] private AssetReference rackAsset;
    [SerializeField] private CartUI cartPanel;
    [SerializeField] private GameObject computerPanel;
    [SerializeField] private ListIngredient ingredients;
    [SerializeField] private ListProduct products;
    [SerializeField] private PlayerControllerSO playerControllerSO;
    [SerializeField] private ListDeliveries deliveries;
    [SerializeField] private Controller controller;
    [SerializeField] private int scrollSpeed;
    [SerializeField] private OrderQuest orderQuest;
    [SerializeField] private GameObject ingredientScroll;
    [SerializeField] private GameObject productScroll;
    [SerializeField] private SwitchListDelivery SwitchListScript;

    private RectTransform ingredientScrollRectTransform;
    private RectTransform productScrollRectTransform;
    private PlayerController playerController;
    private List<GameObject> ingredientButtonList;
    private List<GameObject> productButtonList;
    private List<GameObject> ingredientRackList;
    private List<GameObject> productRackList;
    private int nbButton = 0;
    private int lenght;
    private float cartWeight = 0;
    private int cartCost = 0;
    private int maxButtonInRack;

    public Dictionary<IngredientSO, int> cart;

    void Awake() {
        ingredientScrollRectTransform = ingredientScroll.GetComponent<RectTransform>();
        productScrollRectTransform = productScroll.GetComponent<RectTransform>();

        ingredientRackList = new List<GameObject>();
        ingredientButtonList = new List<GameObject>();
        productRackList = new List<GameObject>();
        productButtonList = new List<GameObject>();

        deliveries.UpdateUI += UpdateStockButtons;

        playerController = playerControllerSO.GetPlayerController();

    }
    private void OnEnable() {
        if (gameObject.activeSelf) {
            //Manage Inputs
            playerController.DisableInput();
            playerController.playerInput.UI.Enable();
            playerController.playerInput.UI.Quit.performed += Quit;

            playerController.playerInput.Amafood.Enable();
            playerController.playerInput.Amafood.AddIngredient.performed += Add;
            playerController.playerInput.Amafood.RemoveIngredient.performed += Remove;
        }
    }

    private void Add(InputAction.CallbackContext ctx) {
        DeliveryButton button;
        controller.GetEventSystemCurrentlySelected().transform.parent.gameObject.TryGetComponent<DeliveryButton>(out button);
        if (button)
            button.GetComponentInChildren<AmmountManager>().PlusButtonIsClicked();
    }

    private void Remove(InputAction.CallbackContext ctx) {
        DeliveryButton button;
        controller.GetEventSystemCurrentlySelected().transform.parent.gameObject.TryGetComponent<DeliveryButton>(out button);
        if (button)
            button.GetComponentInChildren<AmmountManager>().MinusButtonIsClicked();
    }

    private void Start() {
        lenght = ingredients.GetIngredientLenght();

        //Init Cart
        if (cart == null)
            InitCart();

        //Instantiate buttons
        for (int i = 0; i < lenght; i++) {
            buttonAsset.InstantiateAsync().Completed += (go) => {
                DeliveryButton button = go.Result.GetComponent<DeliveryButton>();
                button.deliveryManager = this;
                button.SetIngredient(ingredients.GetIngredientList()[nbButton].ingredient);
                button.SetIngredientSO(ingredients);
                ingredientButtonList.Add(go.Result);
                nbButton++;
                SetupRacks(ingredientRackList, ingredientButtonList, ingredientScroll, ingredientScrollRectTransform);
            };
        }
    }

    void SetupRacks(List<GameObject> rackList, List<GameObject> buttonList, GameObject scroll, RectTransform scrollRect) {
        if (buttonList.Count > 0 && nbButton == lenght) {
            maxButtonInRack = (int)Math.Floor(scroll.GetComponent<RectTransform>().rect.width / buttonList[0].GetComponent<RectTransform>().sizeDelta.x);
            for (int i = 0; i < buttonList.Count; i++) {
                if (i % maxButtonInRack == 0) {
                    rackAsset.InstantiateAsync(scroll.transform).Completed += (go) => {
                        rackList.Add(go.Result);
                        SetupButtons(rackList, buttonList, scroll, scrollRect);
                    };
                }
            }
        }
    }

    void SetupButtons(List<GameObject> rackList, List<GameObject> buttonList, GameObject scroll, RectTransform scrollRect) {
        if (rackList.Count * maxButtonInRack >= buttonList.Count) {
            scrollRect.sizeDelta = new Vector2(scroll.GetComponent<RectTransform>().rect.width, buttonList[0].GetComponent<RectTransform>().rect.height * (rackList.Count + 1));
            for (int i = 0; i < lenght; i++) {
                for (int j = 0; j < lenght; j++) {
                    if (j / maxButtonInRack < rackList.Count) {
                        buttonList[j].transform.SetParent(rackList[j / maxButtonInRack].transform);
                        buttonList[j].transform.localScale = Vector3.one;
                    }
                }
            }

            if (productButtonList.Count == 0)
                SetupProductButton();
            else
                foreach (GameObject go in productButtonList)
                    go.GetComponent<DeliveryButton>().SetIngredientButton(ingredientButtonList);
        }
    }

    private void SetupProductButton() {
        nbButton = 0;
        lenght = products.GetProductLenght();

        for (int i = 0; i < lenght; i++) {
            buttonAsset.InstantiateAsync().Completed += (go) => {
                go.Result.GetComponent<DeliveryButton>().deliveryManager = this;
                go.Result.GetComponent<DeliveryButton>().SetProduct(products.GetProductList()[nbButton]);
                productButtonList.Add(go.Result);
                nbButton++;
                SetupRacks(productRackList, productButtonList, productScroll, productScrollRectTransform);
            };
        }
    }

    private void Update() {
        if (controller.IsGamepad()) {
            RectTransform scroll;
            if (ingredientScroll.activeInHierarchy)
                scroll = ingredientScrollRectTransform;
            else
                scroll = productScrollRectTransform;

            float scrollValue = playerControllerSO.GetPlayerController().playerInput.UI.ScrollWheel.ReadValue<Vector2>().y;
            if (scrollValue != 0)
                scroll.position -= new Vector3(0, scrollValue * scrollSpeed, 0);
        }
    }

    public void SetIngredient(IngredientSO ingredient, bool add) {
        if (add)
            cart[ingredient]++;
        else
            cart[ingredient]--;
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

        foreach (GameObject go in productButtonList)
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

    public void UpdateStockButtons() {
        for (int i = 0; i < ingredientButtonList.Count; i++)
            ingredientButtonList[i].GetComponent<DeliveryButton>().UpdateStock();
    }

    public void Quit(InputAction.CallbackContext context) {
        playerController.playerInput.UI.Quit.performed -= Quit;
        playerController.playerInput.UI.Disable();

        playerController.playerInput.Amafood.AddIngredient.performed -= Add;
        playerController.playerInput.Amafood.RemoveIngredient.performed -= Remove;
        playerController.playerInput.Amafood.Disable();

        playerController.EnableInput();
        computerPanel.SetActive(false);
    }
}
