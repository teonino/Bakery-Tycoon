using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class VirtualKeyboard : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private ProductSO product;
    [SerializeField] private PriceButton priceButton;
    [SerializeField] private Controller controller;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject firstButton;
    [SerializeField] private RawImage productImage;

    private void OnEnable() {
        controller.SetEventSystemToStartButton(firstButton);
    }

    private void Start() {
        productImage.texture = product.image;
    }

    public void AddNumber(string number) {
        inputField.text += number;
    }

    public void ClearNumber() {
        priceButton.SetVirtualKeyboardValue();
        Addressables.Release(gameObject);
    }

    public void ConfirmNumber() {
        if (inputField.text != "") {
            product.price = int.Parse(inputField.text);
            priceButton.SetVirtualKeyboardValue();
            Addressables.Release(gameObject);
        }
    }

    public void SetProduct(ProductSO product) => this.product = product;
    public void SetPriceButton(PriceButton priceButton) => this.priceButton = priceButton;
}
