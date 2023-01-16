using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientSelected : MonoBehaviour {
    [SerializeField] private RawImage image;
    private IngredientSO ingredient;

    public void SetIngredient(IngredientSO ingredient) {
        this.ingredient = ingredient;
        image.texture = ingredient.image;
        image.enabled = true;
    }

    public void RemoveIngredient() {
        this.ingredient = null;
        image.texture = null;
        image.enabled = false;
    }

    public IngredientSO GetIngredient() => ingredient;
}
