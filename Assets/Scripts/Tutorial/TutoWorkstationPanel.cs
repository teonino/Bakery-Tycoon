using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutoWorkstationPanel : WorkstationManager {
    [SerializeField] private InterractQuest addIngredientQuest;
    [SerializeField] private CreateQuest cookQuest;
    [SerializeField] private InterractQuest createPasteQuest;
    [SerializeField] private Tutorial tutorial;
    private bool canCook = true;

    protected override void SetupButton() {
        base.SetupButton();
        tutorial?.Invoke();
    }

    public override void IngredientSelected(IngredientSO ingredient) {
        if (addIngredientQuest.OnInterract())
            tutorial.UnlockAddIngredient();

        if (tutorial.CanAddIngredient())
            base.IngredientSelected(ingredient);
    }

    public override void Cook(InputAction.CallbackContext ctx) {
        if (CheckProduct(cookQuest.GetProduct())) {
            cookQuest.Completed();
            canCook = true;
        }

        if(canCook)
            base.Cook(ctx);
    }

    protected override void CreateProduct() {
        createPasteQuest?.OnInterract();
        base.CreateProduct();
    }
}
