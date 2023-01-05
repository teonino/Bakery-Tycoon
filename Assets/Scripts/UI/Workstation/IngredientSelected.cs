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
    }

    public void RemoveIngredient() {
        this.ingredient = null;
        image.texture = null;
    }

    public IngredientSO GetIngredient() => ingredient;
}
