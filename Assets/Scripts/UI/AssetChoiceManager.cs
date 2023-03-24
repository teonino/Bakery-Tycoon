using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AssetChoiceManager : MonoBehaviour {
    [SerializeField] private Controller controller;
    [SerializeField] private PlayerControllerSO playerController;
    [SerializeField] private Button buttonA;
    [SerializeField] private Button buttonB;
    [SerializeField] private RawImage imageA;
    [SerializeField] private RawImage imageB;

    private FurnitureManager furnitureManager;
    private Money money;
    private FurnitureSO furnitureSO;

    private void OnEnable() {
        controller.SetEventSystemToStartButton(buttonA.gameObject);
        money = FindObjectOfType<MoneyUI>().GetMoney();
    }

    void Start() {
        buttonA.GetComponent<Button>().onClick.AddListener(BuyFurnitureA);
        buttonB.GetComponent<Button>().onClick.AddListener(BuyFurnitureB);
    }

    private void BuyFurnitureA() {
        furnitureSO.GetAssetA().InstantiateAsync().Completed += (go) => {
            money.AddMoney(-furnitureSO.GetPrice());
            furnitureManager.GetBuildingMode().SetSelectedGO(go.Result, true);
            gameObject.SetActive(false);
            furnitureManager.Quit();
        };
    }
    private void BuyFurnitureB() {
        furnitureSO.GetAssetB().InstantiateAsync().Completed += (go) => {
            money.AddMoney(-furnitureSO.GetPrice());
            furnitureManager.GetBuildingMode().SetSelectedGO(go.Result, true);
            gameObject.SetActive(false);
            furnitureManager.Quit();
        };
    }

    public void SetFurnitureSO(FurnitureSO furniture) {
        furnitureSO = furniture;
        imageA.texture = furnitureSO.GetTextureA();
        imageB.texture = furnitureSO.GetTextureB();
    }
    public void SetFurnitureManager(FurnitureManager value) => furnitureManager = value;

    public void Quit(InputAction.CallbackContext ctx) {
        gameObject.SetActive(false);
        furnitureManager.SetButtonForGamepad();
        playerController.GetPlayerController().playerInput.UI.Quit.performed -= Quit;
        playerController.GetPlayerController().playerInput.UI.Quit.performed += furnitureManager.Quit;
    }
}
