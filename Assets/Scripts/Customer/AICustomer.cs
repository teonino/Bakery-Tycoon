using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;

public class AICustomer : MonoBehaviour {
    [SerializeField] private AssetReference assetProductCanvas;
    [SerializeField] private AssetReference assetPaymentCanvas;
    [SerializeField] private float waitingTime = 5f; //Time before customer leaves after ordering
    [SerializeField] private NavMeshAgent agent;

    private GameObject productCanvas; //Stores gameobject to release instance after
    private GameObject item;
    private SpawnCustomer spawner;
    private GameManager manager;
    private ProductSO product;
    private Vector3 spawnPosition; //Return to the exit once customer is leaving
    private Shelf shelf;
    private Chair chair;

    [SerializeField] private bool regular = false;
    private bool waiting = false;
    private bool leaving = false;
    private bool sitting = false;

    private void Awake() {
        spawner = FindObjectOfType<SpawnCustomer>();
        manager = FindObjectOfType<GameManager>();

        //Set product
        List<ProductSO> doableProduct = new List<ProductSO>();
        foreach (ProductSO product in manager.GetProductList()) { //Go through all product
            bool doable = true;
            foreach (IngredientSO ingredient in product.ingredients) //Go through ingredients needed
                if (manager.GetIngredientAmount(ingredient) <= 0)
                    doable = false;

            if (doable)
                doableProduct.Add(product);
        }
        if (doableProduct.Count != 0)
            product = doableProduct[Random.Range(0, doableProduct.Count)];

        //IF NO PRODUCT => CHECK SHELVES ITEM

        //Check shelves
        if (regular)
            Sit();
        else
            GetAvailableShelf();

        if (product) {
            //Instantiate panel that display the requested product
            assetProductCanvas.InstantiateAsync(transform).Completed += (go) => {

                productCanvas = go.Result;
                productCanvas.transform.SetParent(transform);
                productCanvas.transform.position = transform.position + Vector3.up * 2;
                productCanvas.GetComponentInChildren<TextMeshProUGUI>().SetText(product.name);
            };
        }
    }
    private void Sit() {
        List<Table> tables = new List<Table>(FindObjectsOfType<Table>());
        foreach (Table table in tables) {
            Chair chair = table.GetChairAvailable();
            if (chair) {
                this.chair = chair;
                this.chair.ocuppied = true;
            }
        }

        if (!this.chair)
            Leave();
    }

    private void GetAvailableShelf() {
        List<Shelf> shelves = new List<Shelf>(FindObjectsOfType<Shelf>());
        foreach (Shelf shelf in shelves) {

            if (shelf.item) { // Go to a shelf with the requested item
                if (product) {
                    if (shelf.item.GetComponent<Product>().GetName() == product.name) { //If the requested item is already displayed 
                        this.shelf = shelf;
                    }
                }
                else {
                    product = shelf.item.GetComponent<Product>().product;
                    this.shelf = shelf;
                }
            }
            else { //Go to an empty shelf 
                if (this.shelf == null && !shelf.occupied)
                    this.shelf = shelf;
            }
        }
        //Set the shelf as occupied
        shelf.occupied = true;
    }

    void Start() {
        if (product) {
            spawnPosition = transform.position;
            if (regular) {
                if (chair)
                    agent.SetDestination(chair.transform.position);
                else
                    Addressables.ReleaseInstance(gameObject);
            }

            else if (shelf)
                agent.SetDestination(shelf.transform.position);
        }
        else {
            spawner.RemoveCustomer();
            Addressables.ReleaseInstance(gameObject);
        }
    }

    void FixedUpdate() {
        if (!regular) {
            //Go to the shelf
            if (Vector3.Distance(transform.position, shelf.transform.position) < 2 && !waiting) {
                waiting = true;
                StartCoroutine(CustomerWaiting(waitingTime));
            }

            //Exit the bakery
            if (Vector3.Distance(transform.position, spawnPosition) < 2 && leaving) {
                if (item)
                    Addressables.ReleaseInstance(item);
                spawner.RemoveCustomer();
                Addressables.ReleaseInstance(gameObject);
            }

            //Buy item and leave
            if (shelf.item && waiting && !leaving) {
                if (shelf.item.GetComponent<Product>().GetName() == product.name) {
                    //Stop waiting
                    StopAllCoroutines();
                    //Take item
                    if (shelf.item.GetComponent<Product>().amount > 1) {
                        shelf.item.GetComponent<Product>().product.asset.InstantiateAsync(transform).Completed += (go) => {
                            item = go.Result;
                            item.GetComponent<Product>().quality = shelf.item.GetComponent<Product>().quality;
                            item.transform.localPosition = Vector3.up * 1.25f;
                            //Pay
                            DisplayPayment();
                            Leave();
                        };
                        shelf.item.GetComponent<Product>().amount--;
                    }
                    else {
                        item = shelf.item;
                        item.transform.SetParent(transform);
                        item.transform.localPosition = Vector3.up * 1.25f;
                        item.GetComponent<Product>().quality = shelf.item.GetComponent<Product>().quality;
                        shelf.item = null;
                        //Pay
                        DisplayPayment();
                        Leave();
                    }
                }
            }
        }
    }

    private IEnumerator CustomerWaiting(float time) {
        yield return new WaitForSeconds(time);
        Leave();
    }

    //Remove product panel + exit bakery
    private void Leave() {
        leaving = true;
        if (!regular)
            shelf.occupied = false;
        else if (chair)
            chair.ocuppied = false;

        if (productCanvas)
            Addressables.ReleaseInstance(productCanvas);

        agent.SetDestination(spawnPosition);
    }

    //Display the payement
    public void DisplayPayment() {
        float totalPrice = item.GetComponent<Product>().GetPrice() + item.GetComponent<Product>().GetPrice() * item.GetComponent<Product>().quality / 100;
        assetPaymentCanvas.InstantiateAsync().Completed += (go) => {
            go.Result.transform.position = shelf.transform.position + Vector3.up * 2;
            go.Result.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "+" + totalPrice + "€";
            StartCoroutine(DisplayPaymentCoroutine(go.Result));
            product = null;
        };
        manager.AddMoney(totalPrice);
        manager.AddReputation(item.GetComponent<Product>().quality);
    }

    private IEnumerator DisplayPaymentCoroutine(GameObject go) {
        yield return new WaitForSeconds(2);
        Addressables.ReleaseInstance(go);
    }

    public void SetRegular() => regular = true;
}
