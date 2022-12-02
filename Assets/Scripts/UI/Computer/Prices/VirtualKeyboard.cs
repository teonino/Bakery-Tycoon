using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class VirtualKeyboard : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector] public ProductSO product;
    public TMP_InputField inputField;
    public GameObject firstButton;
    [HideInInspector] public PriceButton priceButton;
    public RawImage productImage;

    private GameManager gameManager;

    private void OnEnable() {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.SetEventSystemToStartButton(firstButton);
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
            gameManager.SetProductPrice(product, int.Parse(inputField.text));
            priceButton.SetVirtualKeyboardValue();
            Addressables.Release(gameObject);
        }
    }
}
