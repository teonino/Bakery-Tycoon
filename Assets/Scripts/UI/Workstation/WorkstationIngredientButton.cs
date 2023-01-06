using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class WorkstationIngredientButton : MonoBehaviour {
    [SerializeField] private ListIngredient ingredients;
    [SerializeField] private DebugState debugState;
    [SerializeField] private RawImage image;
    [SerializeField] private TextMeshProUGUI ingredientInformation;

    private IngredientSO ingredient;
    [HideInInspector] public WorkstationManager workplacePanel;
    private bool requirementMet;

    private void Start() {
        ingredientInformation.SetText($"Stock : {ingredients.GetIngredientAmount(ingredient)}");
        image.texture = ingredient.image;
    }

    public void SelectIngredient() {
        if (debugState.GetDebug() || ingredients.GetIngredientAmount(ingredient) > 0)
            workplacePanel.IngredientSelected(ingredient);
    }

    public IngredientSO GetIngredient() => ingredient;

    public void SetIngredient(IngredientSO ingredient) => this.ingredient = ingredient;
}
