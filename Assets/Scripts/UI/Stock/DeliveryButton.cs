using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryButton : MonoBehaviour
{
    [SerializeField] private IngredientSO ingredient;

    public DeliveryManager stockmanager;

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<TextMeshProUGUI>().SetText(ingredient.name);
        GetComponentInChildren<RawImage>().texture = ingredient.image;
        GetComponentInChildren<Button>().onClick.AddListener(delegate { stockmanager.AddIngredient(ingredient); });
    }

    public void SetIngredient(IngredientSO ingredient) => this.ingredient = ingredient;
}
