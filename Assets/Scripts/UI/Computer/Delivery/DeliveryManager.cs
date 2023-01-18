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
    [SerializeField] private ScrollSpeedSO scrollSpeed;
    [SerializeField] private ProductUnlockedSO productUnlocked;
    [SerializeField] private IngredientUnlockSO ingredientUnlocked;
    [SerializeField] private OrderQuest orderQuest;
    [SerializeField] private GameObject ingredientScroll;
    [SerializeField] private GameObject ingredientsList;
    [SerializeField] private GameObject productScroll;
    [SerializeField] private GameObject productList;

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
        productUnlocked.action += EnableProductButton;
        ingredientUnlocked.action += EnableIngredientButton;

        playerController = playerControllerSO.GetPlayerController();
    }

    private void EnableProductButton(ProductSO product) {
        foreach (GameObject item in productButtonList) {
            if (item.GetComponent<DeliveryButton>().product == product) {
                item.SetActive(true);
            }
        }
        ResizeScroll(productRackList, productButtonList, productScroll, productScrollRectTransform);
    }

    private void EnableIngredientButton(IngredientSO ingredient) {
        foreach (GameObject item in ingredientButtonList) {
            if (item.GetComponent<DeliveryButton>().ingredient == ingredient) {
                item.SetActive(true);
            }
        }
        ResizeScroll(ingredientRackList, ingredientButtonList, ingredientScroll, ingredientScrollRectTransform);
    }

    private void OnEnable() {
        if (gameObject.activeSelf) {
            //Manage Inputs
            playerController.DisableInput();
            playerController.playerInput.UI.Enable();
            playerController.playerInput.UI.Quit.performed += Quit;
            playerControllerSO.GetPlayerController().playerInput.Amafood.Enable();
        }

        if (ingredientButtonList.Count > 0) {
            if (ingredientsList.activeInHierarchy)
                StartCoroutine(waitForGamepad(ingredientButtonList[0].GetComponentInChildren<Button>().gameObject));
            else
                StartCoroutine(waitForGamepad(productButtonList[0].GetComponentInChildren<Button>().gameObject));
        }
    }

    private IEnumerator waitForGamepad(GameObject obj) {
        yield return new WaitForEndOfFrame();
        controller.SetEventSystemToStartButton(obj);
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

                if (!ingredients.GetIngredientList()[nbButton].ingredient.unlocked)
                    go.Result.SetActive(false);

                nbButton++;
                SetupRacks(ingredientRackList, ingredientButtonList, ingredientScroll, ingredientScrollRectTransform);
            };
        }

        playerControllerSO.GetPlayerController().playerInput.Amafood.ChangeList.performed += SwitchList;
    }

    void SetupRacks(List<GameObject> rackList, List<GameObject> buttonList, GameObject scroll, RectTransform scrollRect) {
        if (buttonList.Count > 0 && nbButton == lenght) {
            maxButtonInRack = (int)Math.Floor(scroll.GetComponent<RectTransform>().rect.width / buttonList[0].GetComponent<RectTransform>().sizeDelta.x) - 1;
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

    private void ResizeScroll(List<GameObject> rackList, List<GameObject> buttonList, GameObject scroll, RectTransform scrollRect) {
        scrollRect.sizeDelta = new Vector2(
            scroll.GetComponent<RectTransform>().rect.width, 
            buttonList[0].GetComponent<RectTransform>().rect.height * (rackList.Count + 1));
    }

    void SetupButtons(List<GameObject> rackList, List<GameObject> buttonList, GameObject scroll, RectTransform scrollRect) {
        if (rackList.Count * maxButtonInRack >= buttonList.Count) {
            ResizeScroll(rackList, buttonList, scroll, scrollRect);
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
            else {
                foreach (GameObject go in productButtonList)
                    go.GetComponent<DeliveryButton>().SetIngredientButton(ingredientButtonList);

                controller.SetEventSystemToStartButton(ingredientButtonList[0].GetComponentInChildren<Button>().gameObject);
            }
        }
    }

    private void SetupProductButton() {
        nbButton = 0;
        lenght = products.GetProductLenght();

        for (int i = 0; i < lenght; i++) {
            buttonAsset.InstantiateAsync().Completed += (go) => {
                DeliveryButton button = go.Result.GetComponent<DeliveryButton>();
                button.deliveryManager = this;
                button.SetProduct(products.GetProductList()[nbButton]);
                productButtonList.Add(go.Result);

                if (!products.GetProductList()[nbButton].unlocked)
                    go.Result.SetActive(false);

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
                scroll.position -= new Vector3(0, scrollValue * scrollSpeed.GetScrollSpeed(), 0);
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
    public void SwitchList(InputAction.CallbackContext context) {
        if (ingredientsList.activeSelf && productButtonList.Count > 0) {
            ingredientsList.SetActive(false);
            productList.SetActive(true);
            controller.SetEventSystemToStartButton(productButtonList[0].GetComponentInChildren<Button>().gameObject);
        }
        else {
            ingredientsList.SetActive(true);
            productList.SetActive(false);
            controller.SetEventSystemToStartButton(ingredientButtonList[0].GetComponentInChildren<Button>().gameObject);
        }
    }

    private void CalculateCartCostAndWeight() {
        cartCost = 0;
        cartWeight = 0;
        foreach (KeyValuePair<IngredientSO, int> ingredient in cart) {
            cartCost += ingredient.Key.price * ingredient.Value;
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

    private void OnDisable() {
        playerControllerSO.GetPlayerController().playerInput.Amafood.Disable();
    }

    private void OnDestroy() {
        playerControllerSO.GetPlayerController().playerInput.Amafood.ChangeList.performed -= SwitchList;
        deliveries.UpdateUI -= UpdateStockButtons;
        productUnlocked.action -= EnableProductButton;
        ingredientUnlocked.action -= EnableIngredientButton;
    }

    public void Quit(InputAction.CallbackContext context) {
        playerController.playerInput.UI.Quit.performed -= Quit;
        playerController.playerInput.UI.Disable();

        playerController.EnableInput();
        computerPanel.SetActive(false);
    }
}
