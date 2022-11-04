using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmountManager : MonoBehaviour {
    [SerializeField] private IngredientDescription ingredientDescription;
    [SerializeField] private int ammountToBuy;
    [SerializeField] private TextMeshProUGUI textAmmount;

    private void Start() {
        ammountToBuy = ingredientDescription.nbIngredient;
    }

    public void MinusButtonIsClicked() {
        if (ammountToBuy > 0) {
            ammountToBuy -= 1;
            textAmmount.text = ammountToBuy.ToString();
        }
    }

    public void PlusButtonIsClicked() {
        ammountToBuy += 1;
        textAmmount.text = ammountToBuy.ToString();
    }

    public int GetAmount() => ammountToBuy;
}
