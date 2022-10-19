using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "Ingredient", order = 2)]
public class IngredientSO : ScriptableObject {

    public new string name;
    public string description;
    public int price;
    public Texture image;
    public float weight;
}
