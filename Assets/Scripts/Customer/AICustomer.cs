using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;

public class AICustomer : Interactable {
    [SerializeField] private AssetReference assetProductCanvas;
    [SerializeField] private AssetReference assetPaymentCanvas;
    [SerializeField] private AssetReference assetDialoguePanel;
    [SerializeField] private float waitingTime = 5f; //Time before customer leaves after ordering
    [SerializeField] private float waitingTimeSitting = 10f; //Time before customer leaves after sitting
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private bool regular = false;

    [HideInInspector] public ProductSO requestedProduct;
    [HideInInspector] public bool inQueue = false;

    private GameObject productCanvas; //Stores gameobject to release instance after
    private GameObject item;
    private SpawnCustomer spawner;
    private Vector3 spawnPosition; //Return to the exit once customer is leaving
    private MainShelf shelf;
    private Chair chair;
    private int conversationRemaining = 2;
    private bool waiting = false;
    private bool leaving = false;
    private bool sitting = false;
    private bool canInteract = false;

    private new void Awake() {
        base.Awake();
        spawner = FindObjectOfType<SpawnCustomer>();
        shelf = FindObjectOfType<MainShelf>();

        //Check Queue positions
        shelf.GetAvailableQueuePosition(this);
    }

    public void InitCustomer() {
        if (inQueue) {
            //Instantiate panel that display the requested product
            assetProductCanvas.InstantiateAsync(transform).Completed += (go) => {
                productCanvas = go.Result;
                productCanvas.transform.SetParent(transform);
                productCanvas.transform.position = transform.position + Vector3.up * 2;
                if (requestedProduct)
                    productCanvas.GetComponentInChildren<TextMeshProUGUI>().SetText(requestedProduct.name);
                else
                    print("requestedProductNull");
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
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(shelf.transform.position.x, shelf.transform.position.z)) < 2 && shelf.GetItem() && waiting && shelf.IsFirstInQueue(this)) {
            if (shelf.GetItem().GetComponent<Product>().GetName() == requestedProduct.name && shelf.GetItem().GetComponent<Product>().tag != "Paste") {
                //Stop waiting
                StopAllCoroutines();
                //Take item
                if (!item) {
                    if (shelf.GetItem().GetComponent<Product>().amount > 1) {
                        shelf.GetItem().GetComponent<Product>().productSO.asset.InstantiateAsync(transform).Completed += (go) => {
                            item = go.Result;
                            TakeItem();
                        };
                        shelf.GetItem().GetComponent<Product>().amount--;
                    }
                    else {
                        item = shelf.GetItem();
                        item.transform.SetParent(transform);
                        TakeItem();
                        shelf.RemoveItem();
                    }
                }
            }
        }
        if (sitting && chair && Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(chair.transform.position.x, chair.transform.position.z)) < 1) {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            StartCoroutine(CustomerWaiting(waitingTimeSitting));
            canInteract = true;
        }

        //Exit the bakery
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(spawnPosition.x, spawnPosition.z)) < 4 && leaving)
            DestroyCustomer();
    }

    public void TakeItem() {
        item.transform.localPosition = Vector3.up * 1.25f;
        item.GetComponent<Product>().quality = shelf.GetItem().GetComponent<Product>().quality;
        gameManager.AddProductSold(item.GetComponent<Product>().productSO);
        DisplayPayment();
        if (productCanvas)
            Addressables.ReleaseInstance(productCanvas);
        if (regular)
            Sit();
        else
            Leave();
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
                leaving = false;
                agent.SetDestination(chair.transform.position);
            }
        }
        if (inQueue)
            shelf.RemoveCustomerInQueue(this);

        if (!chair)
            Leave();
    }

    //Remove product panel + exit bakery
    private void Leave() {
        leaving = true;
        waiting = sitting = canInteract = false;

        if (inQueue)
            shelf.RemoveCustomerInQueue(this);
        if (chair)
            chair.ocuppied = false;

        agent.SetDestination(spawnPosition);
    }

    //Display the payement
    public void DisplayPayment() {
        int totalPrice = gameManager.GetProductPrice(item.GetComponent<Product>().productSO) + gameManager.GetProductPrice(item.GetComponent<Product>().productSO) * item.GetComponent<Product>().quality / 100;
        assetPaymentCanvas.InstantiateAsync().Completed += (go) => {
            go.Result.transform.position = shelf.transform.position + Vector3.up * 2;
            go.Result.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "+" + totalPrice + "€";
            StartCoroutine(DisplayPaymentCoroutine(go.Result));
            requestedProduct = null;
        };
        gameManager.AddMoney(totalPrice);
        gameManager.AddReputation(item.GetComponent<Product>().quality);
    }

    private IEnumerator DisplayPaymentCoroutine(GameObject go) {
        yield return new WaitForSeconds(2);
        Addressables.ReleaseInstance(go);
    }
    public void SetDestination(Vector3 position) => agent.SetDestination(position);

    public override void Effect() {
        if (conversationRemaining > 0 && regular && canInteract) {
            playerController.DisableInput();
            assetDialoguePanel.InstantiateAsync(GameObject.FindGameObjectWithTag("MainCanvas").transform).Completed += (go) =>
                go.Result.GetComponent<DialogueManager>().GetDialogues(1);
            Time.timeScale = 0;
            conversationRemaining--;
        }
    }
}


