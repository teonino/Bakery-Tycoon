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

    private void Start() {
        controller = FindObjectOfType<PlayerController>().GetController();
    }

    public void SetPriceButton() {
        if (!controller.IsGamepad()) 
            SetNewPrice((int)Math.Round(float.Parse(priceText.text), 0));
        else {
            virtualkeyboard.InstantiateAsync(FindObjectOfType<PriceManager>().transform).Completed += (go) => {
                go.Result.GetComponent<VirtualKeyboard>().SetProduct(product);
                go.Result.GetComponent<VirtualKeyboard>().SetPriceButton(this);
            };
            controller.RegisterCurrentSelectedButton();
        }
    }

    public void SetProduct(ProductSO product) {
        this.product = product;
        image.texture = product.image;
        priceText.text = product.price + "";
    }

    public void SetNewPrice(int newPrice) {
        product.price = newPrice;
        priceText.text = product.price.ToString();
        controller.SetEventSystemToLastButton();
    }

    public void CheckPrice() {
        if (priceText.text.ToString().Contains("-"))
            priceText.text = priceText.text.ToString().Replace("-", "");
    }
}
