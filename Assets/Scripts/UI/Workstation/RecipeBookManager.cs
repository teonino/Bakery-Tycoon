using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RecipeBookManager : MonoBehaviour {
    [SerializeField] private ListProduct products;
    [SerializeField] private SwitchTabPanel switchTabPanel;
    [SerializeField] private Controller controller;
    [SerializeField] protected PlayerControllerSO playerController;
    [SerializeField] private ProductUnlockedSO productUnlocked;
    [SerializeField] private List<GameObject> productDescriptions;
    [SerializeField] private InterractQuest interractQuest;

    private int indexProduct = 0;
    private bool init = false;
    private void Awake()
    {
        playerController.GetPlayerController().playerInput.RecipeBook.NextPage.performed += NextPage;
        playerController.GetPlayerController().playerInput.RecipeBook.PreviousPage.performed += PreviousPage;
    }

    protected virtual void OnEnable() {
        if (init)
            CheckButton();

        Time.timeScale = 0f;
        playerController.GetPlayerController().playerInput.RecipeBook.Enable();
        productUnlocked.action += DisplayProduct;
        interractQuest?.OnInterract();
    }

    private void Start() {
        Display();
        init = true;
    }

    private void NextPage(InputAction.CallbackContext ctx) {
        if (ctx.performed && indexProduct < products.GetProductList().Count / 4)
        {
            print("aled nextpage");
            indexProduct++;
            switchTabPanel?.GoOnNextTab();
            Display();
        }
        else
            print("if nextpage failed");
    }

    private void PreviousPage(InputAction.CallbackContext ctx) {
        if (ctx.performed && indexProduct > 0) {
            print("aled Backpage");
            indexProduct--;
            switchTabPanel?.GoOnPreviousTab();
            Display();
        }
        else
            print("if previous failed");

    }

    private void Display() {
        for (int i = 4 * indexProduct; i < 4 * (indexProduct + 1); i++) {
            productDescriptions[i % 4].SetActive(true);
            if (i < products.GetProductList().Count)
                productDescriptions[i % 4].GetComponent<WorkstationProductButton>().SetProduct(products.GetProductList()[i]);
            else
                productDescriptions[i % 4].SetActive(false);
        }
    }

    private void DisplayProduct(ProductSO product) {
        WorkstationProductButton button;
        for (int i = 4 * indexProduct; i < 4 * (indexProduct + 1); i++) {
            button = productDescriptions[i % 4].GetComponent<WorkstationProductButton>();
            if (product == button.GetProduct())
                button.DislayProduct();
        }
    }

    private void CheckButton() {
        for (int i = 0; i < productDescriptions.Count; i++) {
            if (productDescriptions[i].GetComponent<WorkstationProductButton>().GetProduct().unlocked)
                productDescriptions[i].GetComponent<WorkstationProductButton>().DislayProduct();
        }
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
        productUnlocked.action -= DisplayProduct;
        playerController.GetPlayerController().playerInput.RecipeBook.Disable();
    }
}
