using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SpawnCustomer : MonoBehaviour {
    [Header("References")]
    [SerializeField] private AssetReference regularCustomerAsset;
    [SerializeField] private ListProduct products;
    [SerializeField] private ListIngredient ingredients;
    [SerializeField] private Day day;
    [SerializeField] private CustomersSO customer;
    [SerializeField] private Reputation reputation;
    [SerializeField] private NotificationEvent notifEvent;
    [SerializeField] private NotificationType notifType;
    [SerializeField] private CommandListManager commandList;
    [SerializeField] private Tutorial tutorial;
    [SerializeField] private RandomCustomerList randomCustomerList;
    [SerializeField] private List<RegularSO> regularCustomersDayOne;
    [SerializeField] private List<RegularSO> regularCustomersDayTwo;
    [SerializeField] private List<RegularSO> regularCustomersDayThree;
    [SerializeField] private List<RegularSO> regularCustomersDayFour;

    [Header("Spawn Variables")]
    [SerializeField] private bool enableSpawn;
    [SerializeField] private bool enableSpawnRegularCustomer;
    [SerializeField] private int nbCustomerMax;
    [SerializeField] private CustomersSO firstPackCustomer;
    [Tooltip("1 chance out of X to spawn")]
    [SerializeField] private int spawnChanceRegularCustomer;

    [Header("Tutorial Variables")]
    [SerializeField] private List<Quest> triggerSpawnOnCompletion;
    [SerializeField] private List<ProductSO> tutoProduct;
    private int indexProduct = 0;

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
    private float nbCurrentCustomerFirstPack = 0;

    void Start() {
        tables = new List<Table>(FindObjectsOfType<Table>());
        doableProduct = new List<ProductSO>();
        availableProduct = new List<ProductSO>();

        foreach (Quest quest in triggerSpawnOnCompletion)
            quest.OnCompletedAction += SpawnTutorialCustomer;

        if (!tutorial.GetTutorial())
            if (firstPackCustomer)
                StartCoroutine(SpawnDelayFirstPack());
            else
                StartCoroutine(SpawnDelay());
    }

    private IEnumerator SpawnDelayFirstPack() {
        randomTime = Random.Range(firstPackCustomer.GetDelaySpawn().x, firstPackCustomer.GetDelaySpawn().y); 
        yield return new WaitForSeconds(randomTime);
        enableSpawnRegularCustomer = false;
        InstantiateCustomer(); 
        nbCurrentCustomerFirstPack++;

        if (nbCurrentCustomerFirstPack == firstPackCustomer.GetNbRandomCustomer()) {
            enableSpawnRegularCustomer = true;
            StartCoroutine(SpawnDelay());
        }
        else
            StartCoroutine(SpawnDelayFirstPack());
    }

    private IEnumerator SpawnDelay() {
        randomTime = Random.Range(customer.GetDelaySpawn().x, customer.GetDelaySpawn().y);

        yield return new WaitForSeconds(randomTime);
        InstantiateCustomer();

        if (nbCustomerSpawned + nbCustomerRegularSpawned < customer.GetNbRegularCustomer() + customer.GetNbRandomCustomer())
            StartCoroutine(SpawnDelay());
        else
            FindObjectOfType<DayManager>().UpdateDay();
    }

    private void InstantiateCustomer() {
        //Spawn a customer
        if (enableSpawn && nbCustomer < nbCustomerMax && day.GetDayTime() == DayTime.Day && CheckProducts()) {
            nbCustomer++;
            if (nbCustomer <= nbCustomerMax) {
                if (enableSpawnRegularCustomer && Random.Range(0, spawnChanceRegularCustomer) == 0) {
                    if (nbCustomerRegularSpawned < customer.GetNbRegularCustomer())
                        SpawnCustomerAsset(true);
                }
                else if (nbCustomerSpawned < customer.GetNbRandomCustomer())
                    SpawnCustomerAsset(false);

                //if all Random customer has been spawned => spawn a regular
                else if (enableSpawnRegularCustomer && nbCustomerRegularSpawned < customer.GetNbRegularCustomer())
                    SpawnCustomerAsset(true);
            }
        }
    }

    public void SpawnCustomerAsset(bool regular, ProductSO product = null) {
        if (regular && CheckChairs()) {
            AssetReference customerAsset = null;
            switch (day.GetDayCount() % 4) {
                case 0:
                    customerAsset = regularCustomersDayOne[nbCustomerRegularSpawned].GetModel();
                    break;
                case 1:
                    customerAsset = regularCustomersDayTwo[nbCustomerRegularSpawned].GetModel();
                    break;
                case 2:
                    customerAsset = regularCustomersDayThree[nbCustomerRegularSpawned].GetModel();
                    break;
                case 3:
                    customerAsset = regularCustomersDayFour[nbCustomerRegularSpawned].GetModel();
                    break;
            }

            if (customerAsset.RuntimeKeyIsValid())
                customerAsset.InstantiateAsync(transform).Completed += (go) => {
                    go.Result.name = "RegularCustomer " + nbCustomerSpawned;
                    SetRegularCustomer(go.Result.GetComponent<AIRegularCustomer>(), product);
                    nbCustomerRegularSpawned++;
                };
            else
                regularCustomerAsset.InstantiateAsync(transform).Completed += (go) => {
                    go.Result.name = "RegularCustomer " + nbCustomerSpawned;
                    SetRegularCustomer(go.Result.GetComponent<AIRegularCustomer>(), product);
                    nbCustomerRegularSpawned++;
                };

            notifEvent.Invoke(notifType);
        }
        else {
            SpawnRandomCustomer(product);
            notifEvent.Invoke(notifType);
        }

    }

    private void SpawnTutorialCustomer() {
        for (int i = 0; i < tutoProduct[indexProduct].nbCreated; i++)
            SpawnRandomCustomer(tutoProduct[indexProduct]);
        indexProduct++;
    }

    private void SpawnRandomCustomer(ProductSO product) {
        randomCustomerList.RandomCustomer().InstantiateAsync(transform).Completed += (go) => {
            go.Result.name = "Customer " + nbCustomerSpawned;
            SetCustomer(go.Result.GetComponent<AIRandomCustomer>(), product);
            nbCustomerSpawned++;
        };
    }

    private void SetCustomer(AIRandomCustomer customer, ProductSO product = null) {
        if (product)
            customer.requestedProduct = product;
        else
            customer.requestedProduct = GetRandomProduct();

        customer.SetSpawner(this);
        customer.InitCustomer();
        doableProduct.Clear();
        availableProduct.Clear();
    }

    private void SetRegularCustomer(AIRegularCustomer customer, ProductSO product = null) {
        customer.chair = currentChair;
        customer.chair.ocuppied = true;
        customer.chair.customer = customer;
        customer.SetIndexChair(indexChair);
        customer.table = currentTable;
        indexChair = -1;
        currentChair = null;
        currentTable = null;

        if (product) {
            customer.requestedProduct = product;
            customer.SetWaitingTime(120);
        }
        else
            customer.requestedProduct = GetRandomProduct();

        customer.SetSpawner(this);
        customer.InitCustomer();
        doableProduct.Clear();
        availableProduct.Clear();
    }
    private bool CheckChairs() {
        for (int i = 0; i < tables.Count && !currentChair; i++) {
            indexChair = tables[i].GetChairAvailable();
            if (indexChair >= 0)
                currentChair = tables[i].chairs[indexChair];
        }

        if (currentChair) {
            currentTable = currentChair.table;
            return true;
        }
        return false;
    }

    public void LaunchCommandRecap(AICustomer customer) {
        commandList.AddCommand(customer);
    }

    public void RemoveCommandRecap(AICustomer customer) {
        commandList.RemoveCommand(customer);
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
