using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "listDeliveries", menuName = "Data/ListDeliveries")]
public class ListDeliveries : Data {
    [SerializeField] private Day day;
    [SerializeField] private Tutorial tutorial;
    [SerializeField] private ListIngredient ingredients;
    [SerializeField] private int timeDelivery = 15;

    private List<Delivery> deliveries = new List<Delivery>();
    private int timeDeliveryValue;

    private void OnEnable() {
        day.NewDay += CheckDeliveries;

        if (tutorial)
            timeDeliveryValue = 0;
        else
            timeDeliveryValue = timeDelivery;
    }

    public override void ResetValues() {
        deliveries = new List<Delivery>();
    }

    private void OnDisable() {
        day.NewDay -= CheckDeliveries;
    }

    public void Add(Delivery delivery) {
        deliveries.Add(delivery);
    }

    public IEnumerator ExpressDelivery(Delivery delivery) {
        yield return new WaitForSeconds(timeDeliveryValue);
        DeliverOrder(delivery);
    }

    private void CheckDeliveries() {
        List<Delivery> todayDeliveries = new List<Delivery>();

        //Fetch all deliveries arriving today
        foreach (Delivery delivery in deliveries)
            if (delivery.GetDay() == day.GetCurrentDay())
                todayDeliveries.Add(delivery);

        //Add stock and Remove them from deliveries planned
        foreach (Delivery delivery in todayDeliveries)
            DeliverOrder(delivery);

    }

    private void DeliverOrder(Delivery delivery) {
        foreach (StockIngredient stockIngredient in ingredients.GetIngredientList())
            foreach (StockIngredient deliveryIngredient in delivery.GetIngredients())
                if (stockIngredient.ingredient == deliveryIngredient.ingredient)
                    stockIngredient.amount += deliveryIngredient.amount;

        deliveries.Remove(delivery);
    }

    public int GetExpressOrderTime() => timeDeliveryValue;
    public int SetExpressOrderTime(int value) => timeDeliveryValue = value;
    public int SetDefaultExpressOrderTime() => timeDeliveryValue = timeDelivery;
}
