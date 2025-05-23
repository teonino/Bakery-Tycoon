using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class WorkstationManager : MonoBehaviour {
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
    [SerializeField] protected PlayerControllerSO playerControllerSO;
    [SerializeField] private Controller controller;
    [SerializeField] private ProductUnlockedSO productUnlocked;
    [SerializeField] private IngredientUnlockSO ingredientUnlock;
    [SerializeField] private GameObject LetsCookPanel;
    [SerializeField] private GameObject minigamePosition;

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
    private LocalizedStringComponent noRecipeText;
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
        noRecipeText = noRecipeTextGO.GetComponentInChildren<LocalizedStringComponent>();
    }

    protected virtual void OnEnable() {
        List<StockIngredient> stocks = ingredients.GetIngredientList();
        noRecipeTextGO.SetActive(false);
        LetsCookPanel.SetActive(false);

        if (ingredientButtonList.Count > 0)
            if (controller.IsGamepad())
                StartCoroutine(waitForGamepad());

        ingredientPanel.SetActive(true);
        //recipePanel.SetActive(false);

        deliveries.UpdateUI += UpdateStocksButton;
        ingredientUnlock.action += EnableIngredientButton;

        CheckButton();

        playerControllerSO.GetPlayerController().playerInput.UI.Quit.performed += Quit;
        playerControllerSO.GetPlayerController().playerInput.Workstation.DisplayRecipe.performed += DisplayRecipes;
        playerControllerSO.GetPlayerController().playerInput.Workstation.DisplayRecipe.performed -= DisplayIngredients;
        playerControllerSO.GetPlayerController().playerInput.Workstation.Cook.performed += Cook;
        playerControllerSO.GetPlayerController().playerInput.UI.Enable();
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

        playerControllerSO.GetPlayerController().playerInput.Workstation.DisplayRecipe.performed += DisplayRecipes;
        playerControllerSO.GetPlayerController().playerInput.Workstation.Cook.performed += Cook;
    }


    private void SetupRacks() {
        if (ingredientButtonList.Count > 0 && nbButton == lenght) {
            maxButtonInRack = (int)Math.Floor(scollRectTransform.rect.width / ingredientButtonList[0].GetComponent<RectTransform>().sizeDelta.x);
            for (int i = 0; i < ingredientButtonList.Count; i++) {
                if (i % maxButtonInRack == 0) {
                    rackAsset.InstantiateAsync(scroll.transform).Completed += (go) => {
                        go.Result.name = "Rack " + rackList.Count;
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

    protected virtual void Update() {
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


        if (controller.GetEventSystemCurrentlySelected() && controller.GetEventSystemCurrentlySelected().transform.parent && controller.GetEventSystemCurrentlySelected().transform.parent.gameObject != lastRackSelected) {
            if (scroll.activeInHierarchy) {
                for (int i = 0; i < rackList.Count; i++) {
                    if (rackList[i] == controller.GetEventSystemCurrentlySelected().transform.parent.gameObject) {
                        lastRackSelected = rackList[i];
                        print("Rack" + i);
                    }
                }
            }
        }
    }


    private GameObject lastRackSelected = null;

    public virtual void IngredientSelected(IngredientSO ingredient) {

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
        if (ingredientsSelected[0].inUse || ingredientsSelected[1].inUse || ingredientsSelected[2].inUse) {
            LetsCookPanel.SetActive(true);
        }
        else if (!ingredientsSelected[0].inUse && !ingredientsSelected[1].inUse && !ingredientsSelected[2].inUse) {
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
                    workplace.startMinigames(true);
                }
                else
                    StartCoroutine(DisplayErrorText("CraftstationMissing"));
            }
            else
                StartCoroutine(DisplayErrorText("RecipeNotExistText"));
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
        if (!noRecipeTextGO.activeSelf) {
            noRecipeText.SetKey(msg);
            noRecipeTextGO.SetActive(true);
            yield return new WaitForSeconds(1);
            noRecipeTextGO.SetActive(false);
        }
    }

    public void LaunchIngredientMinigame() {
        gameObject.SetActive(false);
        if (!skipMinigame && currentMinigameCounter < nbIngredientSelected) {
            int indexMinigame;

            //if (nbIngredientSelected > 3) {
            //    indexMinigame = UnityEngine.Random.Range(0, 4);

            //    if (currentMinigameCounter > 0) {
            //        while (indexMinigame == firstIndexMinigame || indexMinigame == secondIndexMinigame)
            //            indexMinigame = UnityEngine.Random.Range(0, 5);
            //        if (currentMinigameCounter == 1) {
            //            secondIndexMinigame = indexMinigame;
            //        }
            //    }
            //    else {
            //        firstIndexMinigame = indexMinigame;
            //    }
            //}

            //else 

            indexMinigame = currentMinigameCounter;


            while (!ingredientsSelected[indexMinigame].GetIngredient())
                indexMinigame++;

            ingredientsSelected[indexMinigame].GetIngredient().minigame.minigameAsset.InstantiateAsync(minigamePosition.transform).Completed += (go) => {
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

    private void RemoveIngredientSelected(IngredientSelected ingredientSelected) {
        ingredientSelected.RemoveIngredient(); //Set to null 
        nbIngredientSelected--;
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
        currentMinigame = null;
        currentMinigameCounter++;
        LaunchIngredientMinigame();
        workplace.startMinigames(false);
    }

    public void DisplayRecipes(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            ingredientPanel.SetActive(false);
            recipePanel.SetActive(true);
            ingredientPanelEnabled = false;

            playerControllerSO.GetPlayerController().playerInput.Workstation.Cook.performed -= Cook;
            playerControllerSO.GetPlayerController().playerInput.Workstation.DisplayRecipe.performed -= DisplayRecipes;
            playerControllerSO.GetPlayerController().playerInput.Workstation.DisplayRecipe.performed += DisplayIngredients;
        }
    }

    public void DisplayIngredients(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            ingredientPanel.SetActive(true);
            recipePanel.SetActive(false);

            playerControllerSO.GetPlayerController().playerInput.Workstation.Cook.performed += Cook;
            playerControllerSO.GetPlayerController().playerInput.Workstation.DisplayRecipe.performed += DisplayRecipes;
            playerControllerSO.GetPlayerController().playerInput.Workstation.DisplayRecipe.performed -= DisplayIngredients;
        }
    }

    private IEnumerator waitForGamepad() {
        yield return new WaitForEndOfFrame();
        controller.SetEventSystemToStartButton(ingredientButtonList[0]);
    }

    private void OnDisable() {
        controller?.SetEventSystemToStartButton(null);
        playerControllerSO.GetPlayerController().playerInput.Workstation.Disable();
        playerControllerSO.GetPlayerController().playerInput.UI.Disable();
        playerControllerSO.GetPlayerController().playerInput.UI.Quit.performed -= Quit;
        deliveries.UpdateUI -= UpdateStocksButton;
        ingredientUnlock.action -= EnableIngredientButton;
    }

    private void RemoveAllSelectedIngredients() {
        foreach (IngredientSelected ingredientSelected in ingredientsSelected)
            if (ingredientSelected.GetIngredient())
                RemoveIngredientSelected(ingredientSelected);
    }

    public void LaunchQuit()
    {
        Quit();
    }

    public void Quit(InputAction.CallbackContext context)
    {
        Quit();
    }

    private void Quit()
    {
        ResetManager();
        playerControllerSO.GetPlayerController().playerInput.UI.Quit.performed -= Quit;
        playerControllerSO.GetPlayerController().playerInput.UI.Disable();
        playerControllerSO.GetPlayerController().EnableInput();
        gameObject.SetActive(false);
        recipePanel.SetActive(false);
        RemoveAllSelectedIngredients();
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

        playerControllerSO.GetPlayerController().playerInput.Workstation.DisplayRecipe.performed -= DisplayRecipes;
        playerControllerSO.GetPlayerController().playerInput.Workstation.DisplayRecipe.performed -= DisplayIngredients;
        playerControllerSO.GetPlayerController().playerInput.Workstation.Cook.performed -= Cook;
    }
}
