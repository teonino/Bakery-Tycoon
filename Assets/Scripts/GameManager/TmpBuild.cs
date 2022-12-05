using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmpBuild : MonoBehaviour {
    public Day day;
    public Controller controller;
    public DebugState debugState;
    public ListIngredient ingredients;
    public ListProduct products;
    public ListFurniture furnitures;
    public Money money;
    public Reputation reputation;
    public Statistics stats;

    public static TmpBuild instance;

    private void Awake() {
        instance = this;
    }
}
