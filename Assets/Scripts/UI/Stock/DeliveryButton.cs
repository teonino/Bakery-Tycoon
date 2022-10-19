using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class DeliveryButton : MonoBehaviour {
    [SerializeField] private Button button;
    [SerializeField] private AssetReference descriptionPanel;
    [SerializeField] private IngredientSO ingredient;
    [HideInInspector] public DeliveryManager deliveryManager;

    //Start is called before the first frame update
    void Start() {
        GetComponentInChildren<TextMeshProUGUI>().SetText(ingredient.name);
        GetComponentInChildren<RawImage>().texture = ingredient.image;
        GetComponentInChildren<Button>().onClick.AddListener(delegate { 
            descriptionPanel.InstantiateAsync(deliveryManager.transform.parent).Completed += (go) => {
                go.Result.GetComponent<IngredientDescription>().deliveryManager = deliveryManager;
                go.Result.GetComponent<IngredientDescription>().ingredient = ingredient;
            }; 
        });
    }

    public void SetIngredient(IngredientSO ingredient) => this.ingredient = ingredient;
}
