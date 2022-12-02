using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PriceButton : MonoBehaviour {

    [SerializeField] private TMP_InputField priceText;
    [SerializeField] private RawImage image;
    [SerializeField] private AssetReference virtualkeyboard;
    [SerializeField] private Controller controller;
    private ProductSO product;

    private GameManager gameManager;
    private void OnEnable() {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void SetPrice() {
        if (!controller.IsGamepad())
            gameManager.SetProductPrice(product, (int)Math.Round(float.Parse(priceText.text), 0));
        else {
            virtualkeyboard.InstantiateAsync(FindObjectOfType<PriceManager>().transform).Completed += (go) => {
                go.Result.GetComponent<VirtualKeyboard>().product = product;
                go.Result.GetComponent<VirtualKeyboard>().priceButton = this;
            };
            gameManager.RegisterCurrentSelectedButton();
        }
    }

    public void SetProduct(ProductSO product) {
        this.product = product;
        image.texture = product.image;
        priceText.text = gameManager.GetProductPrice(product) + "";
    }

    public void SetVirtualKeyboardValue() {
        priceText.text = gameManager.GetProductPrice(product) + "";
        gameManager.SetEventSystemToLastButton();
    }

    public void CheckPrice() {
        if (priceText.text.ToString().Contains("-"))
            priceText.text = priceText.text.ToString().Replace("-", "");
    }
}
