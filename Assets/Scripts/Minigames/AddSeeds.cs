using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class AddSeeds : Minigame
{
    public int nbClick = 7;
    public TextMeshProUGUI text;

    private int click = 0;

    new void Start() {
        base.Start();
        text.SetText("Click " + nbClick + " time");
        controller.playerInput.AddSeeds.Enable();
        controller.playerInput.AddSeeds.AddSeed.performed += AddSeed;
    }

    public void AddSeed(InputAction.CallbackContext context) {
        if (context.performed) {
            click++;
            if (click == nbClick) {
                controller.playerInput.AddSeeds.AddSeed.performed -= AddSeed;
                controller.playerInput.AddSeeds.Disable();
                End();
            }
            text.SetText("Click " + (nbClick - click) + " time");
        }
    }
}
