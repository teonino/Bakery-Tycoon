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

    [SerializeField] private ProductSO product;
    [SerializeField] private TMP_InputField priceText;
    [SerializeField] private RawImage image;
    [SerializeField] private AssetReference virtualkeyboard;

    private GameManager gameManager;
    private ComputerManager computerManager;
    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        computerManager = FindObjectOfType<ComputerManager>();
    }

    public void SetPrice() {
        if (gameManager.GetPlayerController().GetInputType() == InputType.KeyboardMouse)
            product.price = (int)Math.Round(float.Parse(priceText.text), 0);
        else {
            virtualkeyboard.InstantiateAsync(FindObjectOfType<PriceManager>().transform).Completed += (go) => {
                go.Result.GetComponent<VirtualKeyboard>().product = product;
                go.Result.GetComponent<VirtualKeyboard>().priceButton = this;
            };
            computerManager.RegisterCurrentSelectedButton();
        }
    }

    public void SetProduct(ProductSO product) {
        this.product = product;
        image.texture = product.image;
        priceText.text = gameManager.GetProductPrice(product) + "";
    }

    public void SetVirtualKeyboardValue() {
        priceText.text = gameManager.GetProductPrice(product) + "";
        computerManager.SetEventSystemToLastButton();
    }
}
