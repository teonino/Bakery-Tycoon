using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class OrderSumaryManager : MonoBehaviour {
    [SerializeField] private GameObject OrderSumaryGO;
    [SerializeField] private float timeDisplay;
    [SerializeField] private AssetReference notificationAsset;
    [SerializeField] private ListDeliveries listDeliveries;

    private int index = 0;
    private int lenght = 0;
    private Delivery delivery;

    private void Awake() {
        listDeliveries.DisplayOrderSumary += InitOrderSumary;
    }

    private void InitOrderSumary(Delivery delivery) {
        index = 0;
        lenght = delivery.GetIngredients().Count;
        this.delivery = delivery;

        DisplayNotification();
    }

    private void DisplayNotification() {
        if (index < lenght)
            notificationAsset.InstantiateAsync(OrderSumaryGO.transform).Completed += (go) => {
                OrderSumaryNotification notif = go.Result.GetComponent<OrderSumaryNotification>();
                StockIngredient stock = delivery.GetIngredients()[index];

                go.Result.SetActive(true);
                notif.SetText($"{stock.ingredient.name} x{stock.amount} \n");
                notif.SetTime(timeDisplay);
                notif.StartTimer();
                notif.onDestroy += DisplayNotification;

                index++;
            };
    }

    private void OnDestroy() {
        listDeliveries.DisplayOrderSumary -= InitOrderSumary;
    }
}
