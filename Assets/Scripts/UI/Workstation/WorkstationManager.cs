using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class WorkstationManager : MonoBehaviour {
    [SerializeField] private int nbIngredientMax = 3;
    [SerializeField] private ScrollSpeedSO scrollSpeed;
    [SerializeField] private AssetReference ingredientButtonAsset;
    [SerializeField] private AssetReference rackAsset;
    [SerializeField] private GameObject ingredientPanel;
    [SerializeField] private GameObject recipePanel;
    [SerializeField] private GameObject ingredientSelectedParent;
    [SerializeField] private GameObject scroll;
    [SerializeField] private GameObject noRecipeTextGO;
    [SerializeField] private TextMeshProUGUI stockListText;
    [SerializeField] private ListProduct allProducts;
    [SerializeField] private ListIngredient ingredients;
    [SerializeField] private ListDeliveries deliveries;
    [SerializeField] private PlayerControllerSO playerControllerSO;
    [SerializeField] private Controller controller;
    [SerializeField] private ProductUnlockedSO productUnlocked;
    [SerializeField] private IngredientUnlockSO ingredientUnlock;
    [SerializeField] private GameObject LetsCookPanel;

    protected List<IngredientSelected> ingredientsSelected;
    private List<GameObject> ingredientButtonList;
    public List<GameObject> rackList;
    private Workstation workplace;
    private int nbButton = 0;
    private int currentMinigameCounter = 0;
    private Minigame currentMinigame;
    private int lenght;
    private int maxButtonInRack;
    private RectTransform scollRectTransform;
    private int nbIngredientSelected = 0;
    private TextMeshProUGUI noRecipeText;
    private int firstIndexMinigame = -1;
    private int secondIndexMinigame = -1;
    private bool ingredientPanelEnabled = true;

    [HideInInspector] public bool skipRequirement = false;
    [HideInInspector] public bool skipMinigame = false;
    [HideInInspector] public ProductSO currentProduct;

    private void Awake() {
        rackList = new List<GameObject>();
        ingredientButtonList = new List<GameObject>();
        ingredientsSelected = new List<IngredientSelected>();
        lenght = ingredients.GetIngredientLenght();
        workplace = FindObjectOfType<Workstation>();
        scollRectTransform = scroll.GetComponent<RectTransform>();
        noRecipeText = noRecipeTextGO.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable() {
        List<StockIngredient> stocks = ingredients.GetIngredientList();

        if (ingredientButtonList.Count > 0)
            if (controller.IsGamepad())
                StartCoroutine(waitForGamepad());

        ingredientPanel.SetActive(true);
        recipePanel.SetActive(false);

        deliveries.UpdateUI += UpdateStocksButton;
        ingredientUnlock.action += EnableIngredientButton;

        CheckButton();

        playerControllerSO.GetPlayerController().playerInput.Workstation.ChangeTab.performed += DisplayRecipes;
        playerControllerSO.GetPlayerController().playerInput.Workstation.ChangeTab.performed -= DisplayIngredients;
        playerControllerSO.GetPlayerController().playerInput.Workstation.Cook.performed += Cook;
        playerControllerSO.GetPlayerController().playerInput.Workstation.Enable();
    }

    private void CheckButton() {
        foreach (GameObject item in ingredientButtonList) {
            if (item.GetComponent<WorkstationIngredientButton>().GetIngredient().unlocked)
                item.SetActive(true);
        }

        UpdateStocksButton();
    }

    private void EnableIngredientButton(IngredientSO ingredient) {
        foreach (GameObject item in ingredientButtonList) {
            if (item.GetComponent<WorkstationIngredientButton>().GetIngredient() == ingredient)
                item.SetActive(true);
        }

        ResizeScroll();
    }

    //Create buttons
    private void Start() {
        for (int i = 0; i < lenght; i++) {
            ingredientButtonAsset.InstantiateAsync().Completed += (go) => {
                WorkstationIngredientButton button = go.Result.GetComponent<WorkstationIngredientButton>();
                button.workplacePanel = this;
                button.SetIngredient(ingredients.GetIngredientList()[nbButton].ingredient);
                button.SetIngredientSO(ingredients);
                ingredientButtonList.Add(go.Result);

                if (!ingredients.GetIngredientList()[nbButton].ingredient.unlocked)
                    go.Result.SetActive(false);

                nbButton++;
                SetupRacks();
            };
        }

        foreach (Transform t in ingredientSelectedParent.transform)
            ingredientsSelected.Add(t.gameObject.GetComponent<IngredientSelected>());

        playerControllerSO.GetPlayerController().playerInput.Workstation.ChangeTab.performed += DisplayRecipes;
        playerControllerSO.GetPlayerController().playerInput.Workstation.Cook.performed += Cook;
    }

    private void SetupRacks() {
        if (ingredientButtonList.Count > 0 && nbButton == lenght) {
            maxButtonInRack = (int)Math.Floor(scollRectTransform.rect.width / ingredientButtonList[0].GetComponent<RectTransform>().sizeDelta.x);
            for (int i = 0; i < ingredientButtonList.Count; i++) {
                if (i % maxButtonInRack == 0) {
                    rackAsset.InstantiateAsync(scroll.transform).Completed += (go) => {
                        rackList.Add(go.Result);
                        SetupButton();
                    };
                }
            }
        }
    }

    private void ResizeScroll() {
        scollRectTransform.sizeDelta = new Vector2(
            scollRectTransform.rect.width,
            rackList[0].GetComponent<RectTransform>().rect.height * rackList.Count);
    }

    //Once enough button created, we position them
    protected virtual void SetupButton() {
        if (rackList.Count * maxButtonInRack >= ingredientButtonList.Count) {
            ResizeScroll();
            for (int i = 0; i < lenght; i++) {
                if (i / maxButtonInRack < rackList.Count) {
                    ingredientButtonList[i].transform.SetParent(rackList[i / maxButtonInRack].transform);
                    ingredientButtonList[i].transform.localScale = Vector3.one;
                }
            }

            if (controller.IsGamepad()) {
                controller.SetEventSystemToStartButton(ingredientButtonList[0]);

            }
            else
                controller.SetEventSystemToStartButton(null);
        }
    }

    private void UpdateStocksButton() {
        foreach (GameObject go in ingredientButtonList)
            go.GetComponent<WorkstationIngredientButton>().UpdateStock();
    }

    private void Update() {
        if (controller.IsGamepad() && scroll.activeInHierarchy) {
            scollRectTransform.position -= new Vector3(0, playerControllerSO.GetPlayerController().playerInput.UI.ScrollWheel.ReadValue<Vector2>().y * scrollSpeed.GetScrollSpeed(), 0);
        }

        if (gameObject.activeSelf) {
            if (!controller.GetEventSystemCurrentlySelected() && ingredientButtonList.Count > 0) {
                controller.SetEventSystemToStartButton(ingredientButtonList[0]);
            }
            //playerControllerSO.GetPlayerController().DisableInput();
        }

        if (ingredientPanel.activeSelf && !ingredientPanelEnabled) {
            if (ingredientButtonList.Count > 0)
                if (controller.IsGamepad())
                    StartCoroutine(waitForGamepad());
            ingredientPanelEnabled = true;
        }
    }

    public virtual void  IngredientSelected(IngredientSO ingredient) {

        //Check if ingredient already selected
        bool ingredientRemoved = false;
        for (int i = 0; i < ingredientsSelected.Count; i++)
            if (ingredientsSelected[i].GetIngredient() == ingredient) {
                ingredientsSelected[i].RemoveIngredient();
                nbIngredientSelected--;
                ingredientRemoved = true;
            }

        //Check available ingredient slot
        if (!ingredientRemoved) {
            bool ingredientAdded = false;
            for (int i = 0; i < ingredientsSelected.Count && !ingredientAdded; i++) {
                if (ingredientsSelected[i].GetIngredient() == null) {
                    ingredientsSelected[i].SetIngredient(ingredient);
                    nbIngredientSelected++;
                    ingredientAdded = true;
                }
            }
        }
        print("ingredient count: " + ingredientsSelected.Count);
        if (ingredientsSelected[0].inUse || ingredientsSelected[1].inUse || ingredientsSelected[2].inUse)
        {
            LetsCookPanel.SetActive(true);
        }
        else if (!ingredientsSelected[0].inUse && !ingredientsSelected[1].inUse && !ingredientsSelected[2].inUse)
        {
            LetsCookPanel.SetActive(false);
        }
    }

    public virtual void Cook(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            //Check product with selected Ingredient
            foreach (ProductSO product in allProducts.GetProductList()) {
                if (CheckProduct(product)) {
                    currentProduct = product;
                }
            }

            //if null, associated with no known product
            if (currentProduct) {
                //check is crafting station available
                if (currentProduct.CheckRequirement()) {
                    ingredientPanel.SetActive(false);
                    playerControllerSO.GetPlayerController().playerInput.Workstation.Disable();
                    LetsCookPanel.SetActive(false);
                    LaunchIngredientMinigame();
                }
                else
                    StartCoroutine(DisplayErrorText("Crafting Station Missing"));
            }
            else
                StartCoroutine(DisplayErrorText("This Recipe Doesn't Exist"));
        }
    }

    protected bool CheckProduct(ProductSO product) {
        if (product.ingredients.Count == nbIngredientSelected) {
            bool matchingIngredient = true;
            for (int i = 0; i < product.ingredients.Count && matchingIngredient; i++) {
                bool checkMatchingIngredients = false; //Check if ingredient in product is in ingredient selected
                for (int j = 0; j < nbIngredientSelected; j++) {
                    while (!ingredientsSelected[j].GetIngredient())
                        j++;

                    if (product.ingredients[i].ingredient == ingredientsSelected[j].GetIngredient())
                        checkMatchingIngredients = true;
                }
                if (!checkMatchingIngredients)
                    matchingIngredient = false;
            }
            if (matchingIngredient)
                return true;
        }
        return false;
    }

    private IEnumerator DisplayErrorText(string msg) {
        noRecipeText.text = msg;
        noRecipeTextGO.SetActive(true);
        yield return new WaitForSeconds(1);
        noRecipeTextGO.SetActive(false);
    }

    public void LaunchIngredientMinigame() {
        if (!skipMinigame && currentMinigameCounter < nbIngredientSelected) {
            int indexMinigame;

            if (nbIngredientSelected > 3) {
                indexMinigame = UnityEngine.Random.Range(0, 5);

                if (currentMinigameCounter > 0) {
                    while (indexMinigame == firstIndexMinigame || indexMinigame == secondIndexMinigame)
                        indexMinigame = UnityEngine.Random.Range(0, 5);
                    if (currentMinigameCounter == 1) {
                        secondIndexMinigame = indexMinigame;
                    }
                }
                else {
                    firstIndexMinigame = indexMinigame;
                }
            }

            else {
                indexMinigame = currentMinigameCounter;
            }
            while (!ingredientsSelected[indexMinigame].GetIngredient())
                indexMinigame++;

            ingredientsSelected[indexMinigame].GetIngredient().minigame.minigameAsset.InstantiateAsync(transform).Completed += (go) => {
                go.Result.name = " Panel " + currentMinigameCounter;
                currentMinigame = go.Result.GetComponent<Minigame>();
            };
        }
        else {
            CreateProduct();
        }
    }

    protected virtual void CreateProduct() {
        if (currentProduct.pasteAsset == null) {
            currentProduct.asset.InstantiateAsync().Completed += (go) => {
                workplace.CloseWorkplace(go.Result);
                ResetManager();
            };
        }
        else {
            ProductSO tmpProduct = currentProduct;
            currentProduct.pasteAsset.InstantiateAsync().Completed += (go) => {
                go.Result.GetComponent<ProductHolder>().product.SetProduct(tmpProduct);
                workplace.CloseWorkplace(go.Result);
                ResetManager();
            };
        }
        if (!currentProduct.unlocked) {
            currentProduct.unlocked = true; //Action to display it on DeliveryManager + Almanach
            productUnlocked.Invoke(currentProduct);
        }
        RemoveIngredients();
    }

    private void RemoveIngredients() {
        foreach (IngredientSelected ingredientSelected in ingredientsSelected) {
            if (ingredientSelected.GetIngredient()) {
                ingredients.RemoveIngredientStock(ingredientSelected.GetIngredient(), 1); //Remove from stock
                RemoveIngredientSelected(ingredientSelected);
            }
        }

        UpdateStocksButton();
    }

    private void RemoveIngredientSelected(IngredientSelected ingredientSelected)
    {
        ingredientSelected.RemoveIngredient(); //Set to null 
        nbIngredientSelected--;
    }

    public void ResetManager() {
        if (currentMinigame) {
            currentMinigame.DisableInputs();
            Addressables.ReleaseInstance(currentMinigame.gameObject);
        }
        firstIndexMinigame = secondIndexMinigame = -1;
        currentMinigameCounter = 0;
        currentProduct = null;
    }

    public void MinigameComplete(int quality) {
        currentMinigame = null;
        currentMinigameCounter++;
        LaunchIngredientMinigame();
    }

    public void DisplayRecipes(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            ingredientPanel.SetActive(false);
            recipePanel.SetActive(true);
            ingredientPanelEnabled = false;

            playerControllerSO.GetPlayerController().playerInput.Workstation.ChangeTab.performed -= DisplayRecipes;
            playerControllerSO.GetPlayerController().playerInput.Workstation.Cook.performed -= Cook;
            playerControllerSO.GetPlayerController().playerInput.Workstation.ChangeTab.performed += DisplayIngredients;
        }
    }

    public void DisplayIngredients(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            ingredientPanel.SetActive(true);
            recipePanel.SetActive(false);

            playerControllerSO.GetPlayerController().playerInput.Workstation.ChangeTab.performed += DisplayRecipes;
            playerControllerSO.GetPlayerController().playerInput.Workstation.Cook.performed += Cook;
            playerControllerSO.GetPlayerController().playerInput.Workstation.ChangeTab.performed -= DisplayIngredients;
        }
    }

    private IEnumerator waitForGamepad() {
        yield return new WaitForEndOfFrame();
        controller.SetEventSystemToStartButton(ingredientButtonList[0]);
    }

    private void OnDisable() {
        controller?.SetEventSystemToStartButton(null);
        playerControllerSO.GetPlayerController().playerInput.Workstation.Disable();
        deliveries.UpdateUI -= UpdateStocksButton;
        ingredientUnlock.action -= EnableIngredientButton;

        foreach (IngredientSelected ingredientSelected in ingredientsSelected)
            if (ingredientSelected.GetIngredient())  
                RemoveIngredientSelected(ingredientSelected);
    }

    private void OnDestroy() {
        foreach (GameObject go in ingredientButtonList)
            if (go)
                Addressables.ReleaseInstance(go);
        ingredientButtonList.Clear();

        foreach (GameObject go in rackList)
            if (go)
                Addressables.ReleaseInstance(go);

        foreach (IngredientSelected ingredientSelected in ingredientsSelected)
            if (ingredientSelected.gameObject)
                Addressables.ReleaseInstance(ingredientSelected.gameObject);

        rackList.Clear();

        playerControllerSO.GetPlayerController().playerInput.Workstation.ChangeTab.performed -= DisplayRecipes;
        playerControllerSO.GetPlayerController().playerInput.Workstation.ChangeTab.performed -= DisplayIngredients;
        playerControllerSO.GetPlayerController().playerInput.Workstation.Cook.performed -= Cook;
    }
}
