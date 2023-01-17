using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class RecipeBookManager : MonoBehaviour {
    [SerializeField] private ListProduct products;
    [SerializeField] private Controller controller;
    [SerializeField] private PlayerControllerSO playerController;
    [SerializeField] private AssetReference recipeAsset;
    [SerializeField] private AssetReference ingredientDisplayAsset;
    [SerializeField] private GameObject scroll;
    [SerializeField] private ScrollSpeedSO scrollSpeed;

    private RectTransform scrollRectTransform;
    private List<GameObject> recipes;

    private void Awake() {
        recipes = new List<GameObject>();
        scrollRectTransform = scroll.GetComponent<RectTransform>();
    }

    private void Update() {
        if (controller.IsGamepad() && gameObject.activeInHierarchy) {
            scrollRectTransform.position -= new Vector3(0, playerController.GetPlayerController().playerInput.UI.ScrollWheel.ReadValue<Vector2>().y * scrollSpeed.GetScrollSpeed(), 0);
        }
    }

    private void OnEnable() {
        if (recipes.Count == 0) {
            foreach (ProductSO product in products.GetProductList()) {
                recipeAsset.InstantiateAsync(scroll.transform).Completed += (go) => {
                    go.Result.GetComponent<WorkstationProductButton>().SetProduct(product);
                    recipes.Add(go.Result);

                    if (recipes.Count == 1)
                        scroll.GetComponent<RectTransform>().sizeDelta = new Vector2(scroll.GetComponent<RectTransform>().rect.width, (recipes[0].GetComponent<RectTransform>().rect.height + scroll.GetComponent<VerticalLayoutGroup>().spacing) * products.GetProductLenght());
                };
            }
        }
    }
}
