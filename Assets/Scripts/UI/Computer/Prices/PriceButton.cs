using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PriceButton : MonoBehaviour {

    [SerializeField] ProductSO product;
    [SerializeField] TMP_InputField priceText;
    [SerializeField] RawImage image;

    public void SetPrice() {
        product.price = (int) Math.Round(float.Parse(priceText.text), 0);
    }

    public void SetProduct(ProductSO product) {
        this.product = product;
        image.texture = product.image;
        priceText.text = product.price + "";
    }
}
