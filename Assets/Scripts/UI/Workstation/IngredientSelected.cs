using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientSelected : MonoBehaviour {
    [SerializeField] private RawImage ingredientImage;
    [SerializeField] private RawImage backgroundImage;
    [SerializeField] public bool inUse;
    private IngredientSO ingredient;

    public void SetIngredient(IngredientSO ingredient) {
        this.ingredient = ingredient;
        ingredientImage.texture = ingredient.image;
        ingredientImage.enabled = true;
        inUse = true;
    }

    public void RemoveIngredient() {
        this.ingredient = null;
        ingredientImage.texture = null;
        ingredientImage.enabled = false;
        inUse = false;
    }

    public void DisableBackground() => backgroundImage.enabled = false;

    public IngredientSO GetIngredient() => ingredient;
}
