using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureHolder : MonoBehaviour {
    [SerializeField] private FurnitureSO furniture;
    [SerializeField] private MandatoryFurnitureList mandatoryList;

    private void Start() {
        if (mandatoryList)
            mandatoryList.Add(gameObject);
    }

    public bool CanRemoveSelectedItem() {
        if (mandatoryList)
            if (mandatoryList.MoreThanOne())
                return mandatoryList.Remove(gameObject);

        return false;
    }

    public FurnitureSO GetFurniture() => furniture;
    public int GetFurniturePrice() => furniture.GetPrice();
}
