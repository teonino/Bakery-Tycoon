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
    [SerializeField] private bool regular = false;

    private GameObject productCanvas; //Stores gameobject to release instance after
    private GameObject item;
    private SpawnCustomer spawner;
    private GameManager manager;
    public ProductSO requestedProduct;
    private Vector3 spawnPosition; //Return to the exit once customer is leaving
    private Shelf shelf;
    private Chair chair;
    public bool inQueue = false;
    private bool waiting = false;
    private bool leaving = false;
    private bool sitting = false;

    private void Awake() {
        spawner = FindObjectOfType<SpawnCustomer>();
        manager = FindObjectOfType<GameManager>();
        shelf = FindObjectOfType<Shelf>();

        //Check Queue positions
        shelf.GetAvailableQueuePosition(this);
    }

    void Start() {
        if (inQueue) {
            //Instantiate panel that display the requested product
            assetProductCanvas.InstantiateAsync(transform).Completed += (go) => {
                productCanvas = go.Result;
                productCanvas.transform.SetParent(transform);
                productCanvas.transform.position = transform.position + Vector3.up * 2;
                productCanvas.GetComponentInChildren<TextMeshProUGUI>().SetText(requestedProduct.name);
            };
            spawnPosition = transform.position;
        }
        else
            DestroyCustomer();
    }

    void FixedUpdate() {
        //Go to the Queue
        if (agent.remainingDistance < 1 && !waiting) {
            waiting = true;
            StartCoroutine(CustomerWaiting(waitingTime));
        }

        //Buy item and leave
        if (agent.remainingDistance < 1 && shelf.item && waiting && shelf.IsFirstInQueue(this)) {
            if (shelf.item.GetComponent<Product>().GetName() == requestedProduct.name) {
                //Stop waiting
                StopAllCoroutines();
                //Take item
                if (shelf.item.GetComponent<Product>().amount > 1) {
                    shelf.item.GetComponent<Product>().product.asset.InstantiateAsync(transform).Completed += (go) => {
                        item = go.Result;
                        item.GetComponent<Product>().quality = shelf.item.GetComponent<Product>().quality;
                        item.transform.localPosition = Vector3.up * 1.25f;

                        DisplayPayment();
                        if (productCanvas)
                            Addressables.ReleaseInstance(productCanvas);

                        if (regular)
                            Sit();
                        else
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

                    DisplayPayment();
                    if (productCanvas)
                        Addressables.ReleaseInstance(productCanvas);

                    if (regular)
                        Sit();
                    else
                        Leave();
                }
            }
        }

        //Exit the bakery
        if (Vector3.Distance(transform.position, spawnPosition) < 4 && leaving)
            DestroyCustomer();
    }

    private void DestroyCustomer() {
        if (item)
            Addressables.ReleaseInstance(item);
        spawner.RemoveCustomer();
        Addressables.ReleaseInstance(gameObject);
    }

    private IEnumerator CustomerWaiting(float time) {
        yield return new WaitForSeconds(time);
        Leave();
    }
    private void Sit() {
        List<Table> tables = new List<Table>(FindObjectsOfType<Table>());
        foreach (Table table in tables) {
            Chair chair = table.GetChairAvailable();
            if (chair) {
                this.chair = chair;
                this.chair.ocuppied = true;
                sitting = true;
                waiting = leaving = false;
                agent.SetDestination(chair.transform.position);
            }
        }
        if (inQueue)
            shelf.RemoveCustomerInQueue(this);

        if (!this.chair)
            Leave();
    }

    //Remove product panel + exit bakery
    private void Leave() {
        leaving = true;
        waiting = sitting = false;

        if (inQueue)
            shelf.RemoveCustomerInQueue(this);
        if (chair)
            chair.ocuppied = false;

        agent.SetDestination(spawnPosition);
    }

    //Display the payement
    public void DisplayPayment() {
        float totalPrice = item.GetComponent<Product>().GetPrice() + item.GetComponent<Product>().GetPrice() * item.GetComponent<Product>().quality / 100;
        assetPaymentCanvas.InstantiateAsync().Completed += (go) => {
            go.Result.transform.position = shelf.transform.position + Vector3.up * 2;
            go.Result.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "+" + totalPrice + "€";
            StartCoroutine(DisplayPaymentCoroutine(go.Result));
            requestedProduct = null;
        };
        manager.AddMoney(totalPrice);
        manager.AddReputation(item.GetComponent<Product>().quality);
    }

    private IEnumerator DisplayPaymentCoroutine(GameObject go) {
        yield return new WaitForSeconds(2);
        Addressables.ReleaseInstance(go);
    }
    public void SetDestination(Vector3 position) => agent.SetDestination(position);
}
