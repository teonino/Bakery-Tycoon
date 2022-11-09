using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IngredientDescription : MonoBehaviour {
    [SerializeField] private RawImage image;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI amounText;
    [SerializeField] private TextMeshProUGUI price;
    [SerializeField] private AmmountManager amount;
    [SerializeField] private GameObject startButton;

    [HideInInspector] public DeliveryManager deliveryManager;
    [HideInInspector] public GameManager gameManager;
    [HideInInspector] public IngredientSO ingredient;
    [HideInInspector] public int nbIngredient;

    void Start() {
        image.texture = ingredient.image;
        title.SetText(ingredient.name);
        price.SetText(ingredient.price + "€ / U");
        description.SetText(ingredient.description);
        amounText.SetText(nbIngredient + "");
        gameManager.SetEventSystemToStartButton(startButton);
        deliveryManager.SetIngredientDescriptionPanel(gameObject);
    }

    public void Quit() {
        gameManager.SetEventSystemToLastButton();
        Addressables.ReleaseInstance(gameObject);
    }
}
