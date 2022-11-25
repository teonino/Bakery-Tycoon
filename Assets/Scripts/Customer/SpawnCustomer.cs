using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SpawnCustomer : MonoBehaviour {
    [Header("Spawn attributes")]
    [SerializeField] private bool enableSpawn;
    [SerializeField] private bool enableSpawnRegularCustomer;
    [SerializeField] private int minDelaySpawn;
    [SerializeField] private int maxDelaySpawn;
    [SerializeField] private int nbCustomer = 0;
    [SerializeField] private int nbCustomerMax = 5;
    [SerializeField] private int spawnRateRegularCustomer = 10;
    [SerializeField] private AssetReference customerAsset;
    [SerializeField] private AssetReference regularCustomerAsset;

    private int nbCustomerSpawned = 0;
    private Chair currentChair;
    private Table currentTable;
    private int indexChair = -1;
    private GameManager gameManager;
    private List<ProductSO> doableProduct;
    private List<ProductSO> availableProduct;
    private List<Table> tables;

    void Start() {
        tables = new List<Table>(FindObjectsOfType<Table>());
        doableProduct = new List<ProductSO>();
        availableProduct = new List<ProductSO>();
        gameManager = FindObjectOfType<GameManager>();
        StartCoroutine(SpawnDelay(Random.Range(minDelaySpawn, maxDelaySpawn)));
    }

    private IEnumerator SpawnDelay(int time) {
        yield return new WaitForSeconds(time);
        InstantiateCustomer();
        StartCoroutine(SpawnDelay(Random.Range(minDelaySpawn, maxDelaySpawn)));
    }

    private void InstantiateCustomer() {
        //Spawn a customer
        if (enableSpawn && nbCustomer < nbCustomerMax && gameManager.dayTime == DayTime.Day && CheckProducts()) {
            nbCustomer++;
            if (enableSpawnRegularCustomer && Random.Range(0, spawnRateRegularCustomer) == 0 && CheckChairs()) {
                regularCustomerAsset.InstantiateAsync(transform).Completed += (go) => {
                    go.Result.name = "RegularCustomer " + nbCustomerSpawned;
                    SetRegularCustomer(go.Result.GetComponent<AIRegularCustomer>());
                    nbCustomerSpawned++;
                };
            }
            else {
                customerAsset.InstantiateAsync(transform).Completed += (go) => {
                    go.Result.name = "Customer " + nbCustomerSpawned;
                    SetCustomer(go.Result.GetComponent<AIRandomCustomer>());
                    nbCustomerSpawned++;
                };
            }
        }
    }

    private bool CheckChairs() {
        foreach(Table table in tables) {
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

    private void SetCustomer(AIRandomCustomer customer) {
        customer.requestedProduct = GetRandomProduct();
        customer.InitCustomer();
        doableProduct.Clear();
        availableProduct.Clear();
    }

    private void SetRegularCustomer(AIRegularCustomer customer) {
        customer.chair = currentChair;
        customer.chair.ocuppied = true;
        customer.chair.customer = customer;
        customer.indexChair = indexChair;
        customer.table = currentTable;
        indexChair = -1;
        currentChair = null;
        currentTable = null;
        customer.requestedProduct = GetRandomProduct();
        customer.InitCustomer();
        doableProduct.Clear();
        availableProduct.Clear();
    }

    public bool CheckProducts() {
        bool doable;
        foreach (ProductSO product in gameManager.GetProductList()) { //Go through all product
            doable = true;
            foreach (IngredientSO ingredient in product.ingredients) //Go through ingredients needed
                if (gameManager.GetIngredientAmount(ingredient) <= 0)
                    doable = false;

            if (doable)
                doableProduct.Add(product);
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
