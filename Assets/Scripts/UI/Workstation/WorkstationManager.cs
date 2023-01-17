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
    [SerializeField] private GameObject IngredientPanel;
    [SerializeField] private GameObject RecipePanel;
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

        deliveries.UpdateUI += UpdateStocksButton;
    }

    private void EnableIngredientButton(IngredientSO ingredient) {
        foreach(GameObject item in ingredientButtonList) {
            if (item.GetComponent<WorkstationIngredientButton>().GetIngredient() == ingredient)
                item.SetActive(true);
        }

        ResizeScroll();
    }

    private void OnEnable() {
        List<StockIngredient> stocks = ingredients.GetIngredientList();

        if (ingredientButtonList.Count > 0)
            if (controller.IsGamepad())
                StartCoroutine(waitForGamepad());

        IngredientPanel.SetActive(true);
        RecipePanel.SetActive(false);

        playerControllerSO.GetPlayerController().playerInput.Workstation.ChangeTab.performed += DisplayRecipes;
        playerControllerSO.GetPlayerController().playerInput.Workstation.ChangeTab.performed -= DisplayIngredients;

        playerControllerSO.GetPlayerController().playerInput.Workstation.Enable();
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
    private void SetupButton() {
        if (rackList.Count * maxButtonInRack >= ingredientButtonList.Count) {
            ResizeScroll();
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
            playerControllerSO.GetPlayerController().DisableInput();
        }

        if (IngredientPanel.activeSelf && !ingredientPanelEnabled) {
            if (ingredientButtonList.Count > 0)
                if (controller.IsGamepad())
                    StartCoroutine(waitForGamepad());
            ingredientPanelEnabled = true;
        }
    }

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

    public void Cook(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            //Check product with selected Ingredient
            foreach (ProductSO product in allProducts.GetProductList()) {
                if (product.ingredients.Count == nbIngredientSelected) {
                    bool matchingIngredient = true;
                    for (int i = 0; i < product.ingredients.Count && matchingIngredient; i++) {
                        bool checkMatchingIngredients = false; //Check if ingredient in product is in ingredient selected
                        for (int j = 0; j < nbIngredientSelected; j++)
                            if (product.ingredients[i].ingredient == ingredientsSelected[j].GetIngredient())
                                checkMatchingIngredients = true;

                        if (!checkMatchingIngredients)
                            matchingIngredient = false;
                    }
                    if (matchingIngredient)
                        currentProduct = product;
                }
            }

            //if null, associated with no known product
            if (currentProduct) {
                //check is crafting station available
                if (currentProduct.CheckRequirement()) {
                    IngredientPanel.SetActive(false);
                    playerControllerSO.GetPlayerController().playerInput.Workstation.Disable();
                    LaunchIngredientMinigame();
                }
                else
                    StartCoroutine(DisplayErrorText("Crafting Station Missing"));
            }
            else
                StartCoroutine(DisplayErrorText("This Recipe Doesn't Exist"));
        }
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

            ingredientsSelected[indexMinigame].GetIngredient().minigame.minigameAsset.InstantiateAsync(transform).Completed += (go) => {
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
            if (!currentProduct.unlocked) {
                currentProduct.unlocked = true; //Action to display it on DeliveryManager + Almanach
                productUnlocked.Invoke(currentProduct);
            }
            RemoveIngredients();
        }
    }

    private void RemoveIngredients() {
        foreach (IngredientSelected ingredientSelected in ingredientsSelected) {
            if (ingredientSelected.GetIngredient()) {
                ingredients.RemoveIngredientStock(ingredientSelected.GetIngredient(), 1); //Remove from stock
                ingredientSelected.RemoveIngredient(); //Set to null 
                nbIngredientSelected--;
            }
        }

        UpdateStocksButton();
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
        //itemQuality += quality;
        currentMinigame = null;
        currentMinigameCounter++;
        //LaunchMinigame();
        LaunchIngredientMinigame();
    }

    public void DisplayRecipes(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            IngredientPanel.SetActive(false);
            RecipePanel.SetActive(true);
            ingredientPanelEnabled = false;

            playerControllerSO.GetPlayerController().playerInput.Workstation.ChangeTab.performed -= DisplayRecipes;
            playerControllerSO.GetPlayerController().playerInput.Workstation.ChangeTab.performed += DisplayIngredients;
        }
    }

    public void DisplayIngredients(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            IngredientPanel.SetActive(true);
            RecipePanel.SetActive(false);

            playerControllerSO.GetPlayerController().playerInput.Workstation.ChangeTab.performed += DisplayRecipes;
            playerControllerSO.GetPlayerController().playerInput.Workstation.ChangeTab.performed -= DisplayIngredients;
        }
    }

    private IEnumerator waitForGamepad() {
        yield return new WaitForEndOfFrame();
        controller.SetEventSystemToStartButton(ingredientButtonList[0]);
    }

    private void OnDisable() {
        controller?.SetEventSystemToStartButton(null);
        RecipePanel.SetActive(false);

        playerControllerSO.GetPlayerController().playerInput.Workstation.Disable();
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
