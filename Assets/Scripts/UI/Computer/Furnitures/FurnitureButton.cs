using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FurnitureButton : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI informationText;
    [SerializeField] private RawImage image;

    private FurnitureManager furnitureManager;
    private Money money;
    private FurnitureSO furnitureSO;
    private Button button;

    private void Awake() {
        button = GetComponent<Button>();
        money = FindObjectOfType<MoneyUI>().GetMoney();
    }

    // Start is called before the first frame update
    void Start() {
        titleText.text = furnitureSO.GetName();
        informationText.text =
            "Price : " + furnitureSO.GetPrice() + " €" + "\n" +
            "Style : " + furnitureSO.GetStyle() + "\n" +
            "Type : " + furnitureSO.GetType();

        image.texture = furnitureSO.GetTextureA();

        if (furnitureSO.hasTwoAsset())
            button.onClick.AddListener(DisplayAssetChoiceWindow);
        else
            button.onClick.AddListener(BuyFurniture);
    }

    private void DisplayAssetChoiceWindow() {
        if (money.GetMoney() > furnitureSO.GetPrice())
            furnitureManager.DisplayAssetChoice(furnitureSO);
    }

    private void BuyFurniture() {
        if (money.GetMoney() > furnitureSO.GetPrice())
            furnitureSO.GetAssetA().InstantiateAsync().Completed += (go) => {
                money.AddMoney(-furnitureSO.GetPrice());
                furnitureManager.GetBuildingMode().SetSelectedGO(go.Result, true);
                furnitureManager.Quit();
            };
    }

    public void SetFurnitureManager(FurnitureManager value) => furnitureManager = value;
    public void SetFurniture(FurnitureSO value) => furnitureSO = value;
    public FurnitureSO GetFurniture() => furnitureSO;
}
