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
        playerController.playerInput.AddSeeds.Enable();
        
    }

    public void AddSeed(InputAction.CallbackContext context) {
        if (context.performed) {
            click++;
            if (click == nbClick) {
                
                playerController.playerInput.AddSeeds.Disable();
                End();
            }
            text.SetText("Click " + (nbClick - click) + " time");
        }
    }

    public override void EnableInputs() {
        playerController.playerInput.AddSeeds.AddSeed.performed += AddSeed;
    }

    public override void DisableInputs() {
        playerController.playerInput.AddSeeds.AddSeed.performed -= AddSeed;
    }
}
