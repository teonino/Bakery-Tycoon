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
    [SerializeField] private TruckDeliveryTime truckDeliveryTime;
    [SerializeField] private TruckDelivery truckDelivery;
    [SerializeField] private float timeBeforeTruckDeparture;

    private List<Delivery> deliveries = new List<Delivery>();
    private int timeDeliveryValue;

    public Action UpdateUI;
    public Action<Delivery> DisplayOrderSumary;

    private void OnEnable() {
        day.NewDay += CheckDeliveries;

        if (tutorial.GetTutorial())
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
        if (!truckDelivery)
            truckDelivery = FindObjectOfType<TruckDelivery>();

        yield return new WaitForSeconds(timeBeforeTruckDeparture);
        truckDelivery.DeliveryDeparture();
        yield return new WaitForSeconds(timeDeliveryValue - truckDeliveryTime.GetTime());
        yield return new WaitForSeconds(truckDeliveryTime.GetTime());

    }

    public Delivery GetDeliveries() {
        List<StockIngredient> stocks = new List<StockIngredient>();

        foreach (Delivery delivery in deliveries)
            foreach (StockIngredient stock in delivery.GetIngredients())
                stocks.Add(stock);


        deliveries.Clear();

        Delivery finalDelivery = new Delivery(0);
        foreach (StockIngredient stock in stocks) {
            finalDelivery.Add(stock.ingredient, stock.amount);
        }

        return finalDelivery;
    }

    private void CheckDeliveries() {
        List<Delivery> todayDeliveries = new List<Delivery>();

        //Fetch all deliveries arriving today
        foreach (Delivery delivery in deliveries)
            if (delivery.GetDay() == day.GetDayCount())
                todayDeliveries.Add(delivery);

        //Add stock and Remove them from deliveries planned
        foreach (Delivery delivery in todayDeliveries)
            DeliverOrder(delivery);

    }

    public void DeliverOrder(Delivery delivery) {
        foreach (StockIngredient stockIngredient in ingredients.GetIngredientList())
            foreach (StockIngredient deliveryIngredient in delivery.GetIngredients())
                if (stockIngredient.ingredient == deliveryIngredient.ingredient)
                    stockIngredient.amount += deliveryIngredient.amount;

        DisplayOrderSumary?.Invoke(delivery);
        UpdateUI?.Invoke();
        deliveries.Remove(delivery);
    }

    public int GetExpressOrderTime() => timeDeliveryValue;
    public int SetExpressOrderTime(int value) => timeDeliveryValue = value;
    public int SetDefaultExpressOrderTime() => timeDeliveryValue = timeDelivery;
}
