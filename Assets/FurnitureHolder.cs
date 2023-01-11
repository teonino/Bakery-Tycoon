using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureHolder : MonoBehaviour
{
    [SerializeField] private FurnitureSO furniture;

    public FurnitureSO GetFurniture() => furniture;
    public int GetFurniturePrice() => furniture.GetPrice();
}
