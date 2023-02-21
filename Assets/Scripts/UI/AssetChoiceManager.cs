using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssetChoiceManager : MonoBehaviour {
    [SerializeField] private Controller controller;
    [SerializeField] private GameObject buttonA;
    [SerializeField] private GameObject buttonB;

    private FurnitureManager furnitureManager;
    private FurnitureSO furnitureSO;
    // Start is called before the first frame update
    void Start() {
        buttonA.GetComponent<Button>().onClick.AddListener(BuyFurnitureA);
        buttonB.GetComponent<Button>().onClick.AddListener(BuyFurnitureB);

        controller.SetEventSystemToStartButton(buttonA);
    }

    private void BuyFurnitureA() {
        furnitureSO.GetAssetA().InstantiateAsync().Completed += (go) => {
            furnitureManager.GetBuildingMode().SetSelectedGO(go.Result);
            gameObject.SetActive(false);
            furnitureManager.Quit();
        };
    }
    private void BuyFurnitureB() {
        furnitureSO.GetAssetB().InstantiateAsync().Completed += (go) => {
            furnitureManager.GetBuildingMode().SetSelectedGO(go.Result);
            gameObject.SetActive(false);
            furnitureManager.Quit();
        };
    }

    public void SetFurnitureSO(FurnitureSO furniture) {
        furnitureSO = furniture;
        buttonA.GetComponent<RawImage>().texture = furnitureSO.GetTextureA();
        buttonB.GetComponent<RawImage>().texture = furnitureSO.GetTextureB();
    }
    public void SetFurnitureManager(FurnitureManager value) => furnitureManager = value;
}
