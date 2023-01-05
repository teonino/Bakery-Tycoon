using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchListDelivery : MonoBehaviour
{
    [SerializeField] private GameObject ingredientsList;
    [SerializeField] private GameObject productList;

    public void SwitchList() {
        if(ingredientsList.activeSelf) {
            ingredientsList.SetActive(false);
            productList.SetActive(true);
        } else {
            ingredientsList.SetActive(true);
            productList.SetActive(false);
        }
    }
}
