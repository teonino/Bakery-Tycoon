using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorkstationManager : MonoBehaviour {
    [SerializeField] private int nbIngredientMax = 3;
    [SerializeField] private int scrollSpeed;
    [SerializeField] private AssetReference ingredientButtonAsset;
    [SerializeField] private AssetReference rackAsset;
    [SerializeField] private AssetReference ingredientSelectedAsset;
    //[SerializeField] private GameObject stockPanel;
    [SerializeField] private GameObject IngredientPanel;
    [SerializeField] private GameObject RecipePanel;
    [SerializeField] private GameObject ingredientSelectedParent;
    [SerializeField] private GameObject cookButton;
    [SerializeField] private GameObject scroll;
    [SerializeField] private GameObject noRecipeText;
    [SerializeField] private TextMeshProUGUI stockListText;
    [SerializeField] private ListProduct allProducts;
    [SerializeField] private ListIngredient ingredients;
    [SerializeField] private PlayerControllerSO playerControllerSO;
    [SerializeField] private Controller controller;

    private List<IngredientSelected> ingredientsSelected;
    private List<GameObject> ingredientButtonList;
    private List<GameObject> rackList;
    private Workstation workplace;
    private int nbButton = 0;
    private int currentMinigameCounter = 0;
    private Minigame currentMinigame;
    private int lenght;
    private int maxButtonInRack;
    private RectTransform scollRectTransform;
    private int nbIngredientSelected = 0;

    [HideInInspector] public bool skipRequirement = false;
    [HideInInspector] public bool skipMinigame = false;
    [HideInInspector] public ProductSO currentProduct;

    private void Awake() {
        workplace = FindObjectOfType<Workstation>();
        ingredientButtonList = new List<GameObject>();
        rackList = new List<GameObject>();
        ingredientsSelected = new List<IngredientSelected>();
        scollRectTransform = scroll.GetComponent<RectTransform>();
        lenght = ingredients.GetIngredientLenght();
    }

    private void OnEnable() {
        //Setup Stock
        stockListText.SetText("");
        List<StockIngredient> stocks = ingredients.GetIngredientList();

        foreach (StockIngredient stock in stocks)
            stockListText.text += stock.ingredient.name + " : " + stock.amount + "\n";

        //foreach (GameObject button in ingredientButtonList)
        //    button.GetComponent<WorkstationIngredientButton>().SetRequirement(CheckRequirement(button.GetComponent<WorkstationProductButton>().GetProduct()));

        if (ingredientButtonList.Count > 0) 
            if (controller.IsGamepad())
                StartCoroutine(waitForGamepad());
        DisplayIngredients();
    }

    //Create buttons
    private void Start() {
        for (int i = 0; i < lenght; i++) {
            ingredientButtonAsset.InstantiateAsync().Completed += (go) => {
                WorkstationIngredientButton button = go.Result.GetComponent<WorkstationIngredientButton>();
                button.workplacePanel = this;
                button.SetIngredient(ingredients.GetIngredientList()[nbButton].ingredient);
                //button.SetRequirement(CheckRequirement(allProducts.GetProductList()[nbButton]));
                ingredientButtonList.Add(go.Result);
                nbButton++;
                SetupRacks();
            };
        }

        for (int i = 0; i < nbIngredientMax; i++) {
            ingredientSelectedAsset.InstantiateAsync(ingredientSelectedParent.transform).Completed += (go) => {
                ingredientsSelected.Add(go.Result.GetComponent<IngredientSelected>());
            };
        }
    }

    private void SetupRacks() {
        if (ingredientButtonList.Count > 0 && nbButton == lenght) {
            maxButtonInRack = (int)Math.Floor(scollRectTransform.rect.width / ingredientButtonList[0].GetComponent<RectTransform>().sizeDelta.x) - 1;
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

    //Once enough button created, we position them
    private void SetupButton() {
        if (rackList.Count * maxButtonInRack >= ingredientButtonList.Count) {
            scollRectTransform.sizeDelta = new Vector2(scollRectTransform.rect.width, rackList[0].GetComponent<RectTransform>().rect.height * rackList.Count);
            for (int i = 0; i < lenght; i++) {
                if (i / maxButtonInRack < rackList.Count) {
                    ingredientButtonList[i].transform.SetParent(rackList[i / maxButtonInRack].transform);
                    ingredientButtonList[i].transform.localScale = Vector3.one;
                }
            }

            if (controller.IsGamepad())
                controller.SetEventSystemToStartButton(ingredientButtonList[0]);
            else
                controller.SetEventSystemToStartButton(null);
        }
    }
    private void Update() {
        if (controller.IsGamepad()) {
            scollRectTransform.position -= new Vector3(0, playerControllerSO.GetPlayerController().playerInput.UI.ScrollWheel.ReadValue<Vector2>().y * scrollSpeed, 0);
        }

        if (gameObject.activeSelf) {
            if (!controller.GetEventSystemCurrentlySelected() && ingredientButtonList.Count > 0) {
                controller.SetEventSystemToStartButton(ingredientButtonList[0]);
            }
            playerControllerSO.GetPlayerController().DisableInput();
        }
    }

    public bool CheckRequirement(ProductSO product) {
        if (skipRequirement) return true;

        bool requirementMet = true;

        //Check Crafting Station
        List<CraftingStation> craftingStations = new List<CraftingStation>(FindObjectsOfType<CraftingStation>());
        if (requirementMet) {
            requirementMet = false;
            foreach (CraftingStation craftingStation in craftingStations)
                if (craftingStation.GetCraftingStationType() == product.craftStationRequired) requirementMet = true;
        }

        //Check Ingredients
        foreach (IngredientSO ingredient in product.ingredients)
            if (ingredients.GetIngredientAmount(ingredient) <= 0) requirementMet = false;

        return requirementMet;
    }

    //public void SetProduct(ProductSO product) {
    //    this.currentProduct = product;
    //    DisableButtons();
    //    LaunchMinigame();
    //}

    public void IngredientSelected(IngredientSO ingredient) {
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
    }

    //private void LaunchMinigame() {
    //    if (currentProduct) {
    //        //Launch minigame
    //        if (currentMinigameCounter != currentProduct.minigames.Count && !skipMinigame)
    //            currentProduct.minigames[currentMinigameCounter].minigameAsset.InstantiateAsync(transform).Completed += (go) => {
    //                go.Result.name = " Panel " + currentMinigameCounter;
    //                currentMinigame = go.Result.GetComponent<Minigame>();
    //            };
    //        //Create product
    //        else {
    //            if (!currentProduct.hasPaste) {
    //                currentProduct.asset.InstantiateAsync().Completed += (go) => {
    //                    if (currentMinigameCounter >= 0)
    //                        go.Result.GetComponent<Product>().quality = itemQuality / currentMinigameCounter;
    //                    workplace.CloseWorkplace(go.Result);
    //                    ResetManager();
    //                };
    //            }
    //            else {
    //                ProductSO tmpProduct = currentProduct;
    //                currentProduct.pasteAsset.InstantiateAsync().Completed += (go) => {
    //                    if (currentMinigameCounter > 0)
    //                        go.Result.GetComponent<ProductHolder>().product.quality = itemQuality / currentMinigameCounter;
    //                    go.Result.GetComponent<ProductHolder>().product.SetProduct(tmpProduct);
    //                    workplace.CloseWorkplace(go.Result);
    //                    ResetManager();
    //                };
    //            }

    //            RemoveIngredients(currentProduct);
    //        }
    //    }
    //}

    public void Cook() {
        //Check product with selected Ingredient
        foreach (ProductSO product in allProducts.GetProductList()) {
            if (product.ingredients.Count == nbIngredientSelected) {
                bool matchingIngredient = true;
                for (int i = 0; i < product.ingredients.Count && matchingIngredient; i++) {
                    bool checkMatchingIngredients = false; //Check if ingredient in product is in ingredient selected
                    for (int j = 0; j < nbIngredientSelected; j++) {
                        if (product.ingredients[i] == ingredientsSelected[j].GetIngredient()) {
                            checkMatchingIngredients = true;
                        }
                    }

                    if (!checkMatchingIngredients)
                        matchingIngredient = false;
                }

                if (matchingIngredient)
                    currentProduct = product;
            }
        }

        if (currentProduct) {
            IngredientPanel.SetActive(false);
            LaunchIngredientMinigame();
            foreach (IngredientSelected ingredientSelected in ingredientsSelected)
                ingredientSelected.RemoveIngredient();
        } else {
            StartCoroutine(DisplayNoRecipeText());
        }
    }

    private IEnumerator DisplayNoRecipeText() {
        noRecipeText.SetActive(true);
        yield return new WaitForSeconds(1);
        noRecipeText.SetActive(false);
    }

    public void LaunchIngredientMinigame() {

        if (!skipMinigame && currentMinigameCounter < nbIngredientSelected) {
            ingredientsSelected[currentMinigameCounter].GetIngredient().minigame.minigameAsset.InstantiateAsync(transform).Completed += (go) => {
                go.Result.name = " Panel " + currentMinigameCounter;
                currentMinigame = go.Result.GetComponent<Minigame>();
            };
        }
        else {
            if (!currentProduct.hasPaste) {
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

            RemoveIngredients();
        }
    }

    private void RemoveIngredients() {
        for (int i = 0; i < nbIngredientSelected; i++) {
            ingredients.RemoveIngredientStock(ingredientsSelected[i].GetIngredient(), 1);
        }
    }

    public void ResetManager() {
        if (currentMinigame) {
            currentMinigame.DisableInputs();
            Addressables.ReleaseInstance(currentMinigame.gameObject);
        }
        currentMinigameCounter = 0;
        currentProduct = null;
    }

    public void MinigameComplete(int quality) {
        //itemQuality += quality;
        currentMinigame = null;
        currentMinigameCounter++;
        //LaunchMinigame();
        LaunchIngredientMinigame();
    }

    public void DisplayRecipes() {
        IngredientPanel.SetActive(false);
        RecipePanel.SetActive(true);
    }

    public void DisplayIngredients() {
        IngredientPanel.SetActive(true);
        RecipePanel.SetActive(false);
    }

    //private void DisableButtons() {

        //foreach (GameObject go in ingredientButtonList)
        //    go.SetActive(false);

        //foreach (IngredientSelected ingredientDisplay in ingredientsSelected)
        //    ingredientDisplay.gameObject.SetActive(false);

        //nbButton = 0;
        //cookButton.SetActive(false);
        //stockPanel.SetActive(false);
    //}

    //private void EnableButtons() {
    //    if (controller.IsGamepad())
    //        StartCoroutine(waitForGamepad());

        //IngredientPanel.SetActive(true);
        //foreach (GameObject go in ingredientButtonList)
        //    go.SetActive(true);

        //foreach (IngredientSelected ingredientDisplay in ingredientsSelected)
        //    ingredientDisplay.gameObject.SetActive(true);

        //cookButton.SetActive(true);
        //stockPanel.SetActive(true);
    //}

    private IEnumerator waitForGamepad() {
        yield return new WaitForEndOfFrame();
        controller.SetEventSystemToStartButton(ingredientButtonList[0]);
    }

    private void OnDisable() {
        controller?.SetEventSystemToStartButton(null);
        RecipePanel.SetActive(false);     
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
    }
}
