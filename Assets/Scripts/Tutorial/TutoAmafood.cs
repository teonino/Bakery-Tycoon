using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutoAmafood : DeliveryManager {
    [SerializeField] private Tutorial tutorial;
    [SerializeField] private OrderQuest orderQuest;
    [SerializeField] private InterractQuest productAmafoodInterract;

    protected override void Start() {
        base.Start();
        playerControllerSO.GetPlayerController().playerInput.Amafood.ChangeList.Disable();
        playerControllerSO.GetPlayerController().playerInput.UI.Quit.Disable();
    }


    protected override IEnumerator waitForGamepad(GameObject obj) {
        yield return new WaitForEndOfFrame();
        controller.SetEventSystemToStartButton(obj);
        controller.RegisterCurrentSelectedButton();
        yield return new WaitForEndOfFrame();
        tutorial.Invoke();
    }

    public override void SetIngredient(IngredientSO ingredient, bool add) {
        base.SetIngredient(ingredient, add);
        orderQuest.CheckIngredient(ingredient);

        playerControllerSO.GetPlayerController().playerInput.Amafood.ChangeList.Enable();
        playerControllerSO.GetPlayerController().playerInput.UI.Quit.Enable();
    }

    public override void DisplayProductList(InputAction.CallbackContext context) {
        base.DisplayProductList(context);
        productAmafoodInterract.OnInterract();
    }
}
