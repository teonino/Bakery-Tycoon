using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class RecipeBookManager : MonoBehaviour {
    [SerializeField] private ListProduct products;
    [SerializeField] private AssetReference recipeAsset;
    [SerializeField] private AssetReference ingredientDisplayAsset;
    [SerializeField] private GameObject layoutGroup;

    private List<GameObject> recipes;

    private void Awake() {
        recipes = new List<GameObject>();
    }

    private void OnEnable() {
        if (recipes.Count == 0) {
            foreach (ProductSO product in products.GetProductList()) {
                recipeAsset.InstantiateAsync(layoutGroup.transform).Completed += (go) => {
                    go.Result.GetComponent<WorkstationProductButton>().SetProduct(product);
                    recipes.Add(go.Result);

                    if (recipes.Count == 1)
                        layoutGroup.GetComponent<RectTransform>().sizeDelta = new Vector2(layoutGroup.GetComponent<RectTransform>().rect.width, (recipes[0].GetComponent<RectTransform>().rect.height + layoutGroup.GetComponent<VerticalLayoutGroup>().spacing) * products.GetProductLenght());
                };
            }
        }
    }
}
