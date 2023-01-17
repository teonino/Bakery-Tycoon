using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class WorkstationProductButton : MonoBehaviour {
    [SerializeField] private ProductSO product;

    [SerializeField] private RawImage image;
    [SerializeField] private TextMeshProUGUI productNbCreated;
    [SerializeField] private TextMeshProUGUI productDescription;
    [SerializeField] private GameObject layoutGroup;
    [SerializeField] private AssetReference ingredientAsset;

    [SerializeField] private GameObject productRequirementPanel;
    private bool requirementMet;

    public void SetProduct(ProductSO product) {
        this.product = product;

        if (product.unlocked) {
            image.texture = product.image;

            productNbCreated.SetText(product.name + " x" + product.nbCreated);
            productDescription.SetText("Ingredients :\n");
        } else {
            productNbCreated.SetText("??????????");
            productDescription.SetText("??????????");
        }


        foreach (IngredientsForProduct ingredient in product.ingredients) {
            ingredientAsset.InstantiateAsync(layoutGroup.transform).Completed += (go) => {
                IngredientSelected ingredientDisplay = go.Result.GetComponent<IngredientSelected>();

                ingredientDisplay.DisableBackground();
                ingredientDisplay.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);
                if (ingredient.isUnlocked() || product.unlocked)
                    ingredientDisplay.SetIngredient(ingredient.ingredient);
            };
        }
    }

    public ProductSO GetProduct() => this.product;

    public void SetRequirement(bool requirementMet) {
        this.requirementMet = requirementMet;
    }
}
