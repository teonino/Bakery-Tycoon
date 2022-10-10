using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;

public class AICustomer : MonoBehaviour {
    [SerializeField] private AssetReference assetProductCanvas;
    [SerializeField] private AssetReference assetPaymentCanvas;
    [SerializeField] private float waitingTime = 5f;
    [SerializeField] private Product product;

    private Vector3 spawnPosition; //Return to the exit once customer is leaving
    private Shelf shelf;
    private NavMeshAgent agent;
    private GameObject productCanvas; //Stores gameobject to release instance after
    private GameObject item; 
    private bool waiting = false;
    
    private void Awake() {
        spawnPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();

        //Check shelves
        List<Shelf> shelves = new List<Shelf>(FindObjectsOfType<Shelf>());
        foreach (Shelf shelf in shelves) {
            if (shelf.item.GetComponent<Product>().GetName() == product.GetName()) { //If the requested item is already displayed
                this.shelf = shelf;
            }
            else if (shelf.item == null && shelf == null) { //Go to an empty shelf 
                this.shelf = shelf;
            }

        }

        //Instantiate panel that display the requested product
        assetProductCanvas.InstantiateAsync(transform).Completed += (go) => {
            productCanvas = go.Result;
            productCanvas.transform.SetParent(transform);
            productCanvas.transform.position = transform.position + Vector3.up * 2;
            productCanvas.GetComponentInChildren<TextMeshProUGUI>().SetText(product.GetName());
        };
    }

    void Start() {
        agent.SetDestination(shelf.transform.position);
    }

    // Update is called once per frame
    void Update() {
        //Go to the shelf
        if (Vector3.Distance(transform.position, shelf.transform.position) < 2 && !waiting) {
            waiting = true;
            StartCoroutine(CustomerWaiting(waitingTime));
        }

        //Exit the bakery
        if (Vector3.Distance(transform.position, spawnPosition) < 2 && waiting) {
            if (item)
                Addressables.ReleaseInstance(item);
            Addressables.ReleaseInstance(gameObject);
        }

        //Buy item and leave
        if (shelf.item.GetComponent<Product>().GetName() == product.GetName() && waiting) {
            //Stop waiting
            StopAllCoroutines();

            //Pay
            DisplayPayment();

            //Take item
            item = shelf.item;
            shelf.item = null;
            item.transform.SetParent(transform);
            item.transform.localPosition = Vector3.up * 1.25f;
            Leave();
        }
    }

    private IEnumerator CustomerWaiting(float time) {
        yield return new WaitForSeconds(time);
        Leave();
    }

    //Remove product panel + exit bakery
    private void Leave() {
        waiting = false;
        Addressables.ReleaseInstance(productCanvas);
        agent.SetDestination(spawnPosition);
    }

    //Display the payement
    public void DisplayPayment() {
        assetPaymentCanvas.InstantiateAsync().Completed += (go) => {
            go.Result.transform.position = shelf.transform.position + Vector3.up * 2;
            go.Result.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "+" + product.GetPrice() + "€";
            StartCoroutine(DisplayPaymentCoroutine(go.Result));
        };
    }

    private IEnumerator DisplayPaymentCoroutine(GameObject go) {
        yield return new WaitForSeconds(2);
        Addressables.ReleaseInstance(go);
    }
}
