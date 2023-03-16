using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutoCartUI : CartUI
{
    [SerializeField] private Tutorial tutorial;
    [SerializeField] private OrderTypeQuest orderBreadIngredient;
    [SerializeField] private OrderQuest orderProductQuest;

    public override void Order(InputAction.CallbackContext ctx) {
        IInputInteraction interaction = ctx.interaction;
        base.Order(ctx);

        if (interaction.ToString().Contains("Tap")) {
            if (orderBreadIngredient.CheckDeliveryType())
                tutorial.UnlockWorkstation();
            orderProductQuest?.CheckOrder(delivery);
        }
    }
}
