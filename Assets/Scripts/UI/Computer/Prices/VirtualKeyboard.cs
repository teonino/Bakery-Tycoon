using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class VirtualKeyboard : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Controller controller;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject firstButton;
    [SerializeField] private RawImage productImage;

    private ProductSO product;
    private PriceButton priceButton;

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
        priceButton.SetNewPrice(0);
        Addressables.Release(gameObject);
    }

    public void ConfirmNumber() {
        if (inputField.text != "") {
            priceButton.SetNewPrice(int.Parse(inputField.text));
            Addressables.Release(gameObject);
        }
    }

    public void SetProduct(ProductSO product) => this.product = product;
    public void SetPriceButton(PriceButton priceButton) => this.priceButton = priceButton;
}
