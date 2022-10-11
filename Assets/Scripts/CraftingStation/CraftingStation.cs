using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingStation : Interactable {

    public CraftingStationType type;
    private int dirty = 0;
    
    public override void Effect() {
        //Check cleanness
        if (dirty < 20)
            print("Clean");
        else if (dirty < 40)
            print("A bit dirty");
        else if (dirty < 60)
            print("Dirty");
        else 
            print("Very Dirty");

        //Minigame to clean the crafting station
    }
}
