using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class IngredientDescription : MonoBehaviour {
    [SerializeField] private RawImage image;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI price;
    [SerializeField] private AmmountManager amount;

    [HideInInspector] public DeliveryManager deliveryManager;
    [HideInInspector] public IngredientSO ingredient;

    void Start() {
        image.texture = ingredient.image;
        title.SetText(ingredient.name);
        price.SetText(ingredient.price + "€ / U");
        description.SetText(ingredient.description);
    }

    public void Quit() {
        deliveryManager.AddIngredient(ingredient,amount.GetAmount());
        Addressables.ReleaseInstance(gameObject);
    }
}
