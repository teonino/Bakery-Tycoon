using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListFurniture", menuName = "Data/ListFurniture")]
public class ListFurniture : ScriptableObject
{
    [SerializeField] private List<FurnitureSO> listFurniture;

    public List<FurnitureSO> GetFurnitures() => listFurniture;
    public void AddFurniture(FurnitureSO furniture) => listFurniture.Add(furniture);
    public int GetFurnitureCount() => listFurniture.Count;
}
