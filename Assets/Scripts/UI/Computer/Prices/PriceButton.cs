using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class PriceButton : MonoBehaviour {

    [SerializeField] ProductSO product;
    [SerializeField] TMP_InputField priceText;

    public void SetPrice() {
        product.price = float.Parse(priceText.text, CultureInfo.InvariantCulture.NumberFormat);
    }
}
