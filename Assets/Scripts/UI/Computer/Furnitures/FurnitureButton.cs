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
    private FurnitureSO furnitureSO;
    private Button button;

    private void Awake() {
        button = GetComponent<Button>();
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
        furnitureManager.DisplayAssetChoice(furnitureSO);
    }

    private void BuyFurniture() {
        furnitureSO.GetAssetA().InstantiateAsync().Completed += (go) => {
            furnitureManager.GetBuildingMode().SetSelectedGO(go.Result, true);
            furnitureManager.Quit();
        };
    }

    public void SetFurnitureManager(FurnitureManager value) => furnitureManager = value;
    public void SetFurniture(FurnitureSO value) => furnitureSO = value;
    public FurnitureSO GetFurniture() => furnitureSO;
}
