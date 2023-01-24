using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SpawnCustomer : MonoBehaviour {
    [Header("References")]
    [SerializeField] private AssetReference customerAsset;
    [SerializeField] private AssetReference regularCustomerAsset;
    [SerializeField] private ListProduct products;
    [SerializeField] private ListIngredient ingredients;
    [SerializeField] private Day day;
    [SerializeField] private Reputation reputation;
    [SerializeField] private NotificationEvent notifEvent;
    [SerializeField] private NotificationType notifType;

    [Header("Spawn Variables")]
    [SerializeField] private bool enableSpawn;
    [SerializeField] private bool enableSpawnRegularCustomer;
    [SerializeField] private int nbCustomerPerDay;
    [SerializeField] private int nbCustomerRegulierPerDay;
    [SerializeField] private Vector2 delaySpawn;
    [SerializeField] private int nbCustomerMax;
    [Tooltip("1 chance out of X to spawn")]
    [SerializeField] private int spawnChanceRegularCustomer;

    private int nbCustomer = 0;
    private int nbCustomerSpawned = 0;
    private int nbCustomerRegularSpawned = 0;
    private Chair currentChair;
    private Table currentTable;
    private int indexChair = -1;
    private List<ProductSO> doableProduct;
    private List<ProductSO> availableProduct;
    private List<Table> tables;
    private float randomTime;

    void Start() {
        tables = new List<Table>(FindObjectsOfType<Table>());
        doableProduct = new List<ProductSO>();
        availableProduct = new List<ProductSO>();

        StartCoroutine(SpawnDelay());
    }

    private IEnumerator SpawnDelay() {
        randomTime = Random.Range(delaySpawn.x, delaySpawn.y);

        yield return new WaitForSeconds(randomTime);
        InstantiateCustomer();

        if (nbCustomerSpawned + nbCustomerRegularSpawned < nbCustomerPerDay + nbCustomerRegulierPerDay)
            StartCoroutine(SpawnDelay());
        else
            FindObjectOfType<DayManager>().UpdateDay();
    }

    private void InstantiateCustomer() {
        //Spawn a customer
        if (enableSpawn && nbCustomer < nbCustomerMax && day.GetDayTime() == DayTime.Day && CheckProducts()) {
            nbCustomer++;
            if (enableSpawnRegularCustomer) {
                if (Random.Range(0, spawnChanceRegularCustomer) == 0) {
                    if (nbCustomerRegularSpawned < nbCustomerRegulierPerDay)
                        SpawnCustomerAsset(true);
                }
                else if (nbCustomerSpawned < nbCustomerPerDay)
                    SpawnCustomerAsset(false);
                else if (nbCustomerRegularSpawned < nbCustomerRegulierPerDay)
                    SpawnCustomerAsset(true);
            }
        }
    }

    public void SpawnCustomerAsset(bool regular, ProductSO product = null) {
        if (regular && CheckChairs()) {
            regularCustomerAsset.InstantiateAsync(transform).Completed += (go) => {
                go.Result.name = "RegularCustomer " + nbCustomerSpawned;
                SetRegularCustomer(go.Result.GetComponent<AIRegularCustomer>(), product);
                nbCustomerRegularSpawned++;
            };
        }
        else {
            customerAsset.InstantiateAsync(transform).Completed += (go) => {
                go.Result.name = "Customer " + nbCustomerSpawned;
                SetCustomer(go.Result.GetComponent<AIRandomCustomer>(), product);
                nbCustomerSpawned++;
            };
        }

        notifEvent.Invoke(notifType);
    }

    private void SetCustomer(AIRandomCustomer customer, ProductSO product = null) {

        if (product)
            customer.requestedProduct = product;
        else
            customer.requestedProduct = GetRandomProduct();
        customer.InitCustomer();
        doableProduct.Clear();
        availableProduct.Clear();
    }

    private void SetRegularCustomer(AIRegularCustomer customer, ProductSO product = null) {
        customer.chair = currentChair;
        customer.chair.ocuppied = true;
        customer.chair.customer = customer;
        customer.indexChair = indexChair;
        customer.table = currentTable;
        indexChair = -1;
        currentChair = null;
        currentTable = null;

        if (product) {
            customer.requestedProduct = product;
            customer.SetWaitingTime(9999);
        }
        else
            customer.requestedProduct = GetRandomProduct();

        customer.InitCustomer();
        doableProduct.Clear();
        availableProduct.Clear();
    }
    private bool CheckChairs() {
        foreach (Table table in tables) {
            indexChair = table.GetChairAvailable();
            if (indexChair >= 0)
                currentChair = table.chairs[indexChair];
        }
        if (currentChair) {
            currentTable = currentChair.table;
            return true;
        }
        return false;
    }

    public bool CheckProducts() {
        foreach (ProductSO product in products.GetProductList()) { //Go through all product
            if (product.unlocked) {
                doableProduct.Add(product);
            }
        }

        List<Shelf> shelves = new List<Shelf>(FindObjectsOfType<Shelf>());
        foreach (Shelf shelf in shelves) {
            if (shelf.GetItem() && shelf.GetItem().tag != "Plate") {
                availableProduct.Add(shelf.GetItem().GetComponent<ProductHolder>().product.productSO);
            }
        }

        return doableProduct.Count > 0 || availableProduct.Count > 0;
    }

    public ProductSO GetRandomProduct() {
        if (doableProduct.Count > 0)
            return doableProduct[Random.Range(0, doableProduct.Count)];
        else
            return availableProduct[Random.Range(0, availableProduct.Count)];
    }
    public void RemoveCustomer() => nbCustomer--;
}
