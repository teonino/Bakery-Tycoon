using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [Header("Global variables")]
    public DayTime dayTime;
    public float reputation = 0;
    public float money = 0;

    [Space(10)]
    public List<ProductSO> productsList;

    [Header("Stocks")]
    public int maxStock;
    public int currentStock;
    public List<Stock> stocks;

    public int GetLenghtProducts() => productsList.Count;
}
