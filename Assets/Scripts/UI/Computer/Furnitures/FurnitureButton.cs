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

    // Start is called before the first frame update
    void Start() {
        titleText.text = furnitureSO.GetName();
        informationText.text =
            "Price : " + furnitureSO.GetPrice() + " €" + "\n" +
            "Style : " + furnitureSO.GetStyle() + "\n" +
            "Type : " + furnitureSO.GetType();

        image.texture = furnitureSO.GetTexture();
    }
    public void SetFurnitureManager(FurnitureManager value) => furnitureManager = value;
    public void SetFurniture(FurnitureSO value) => furnitureSO = value;
    public FurnitureSO GetFurniture() => furnitureSO;
}
