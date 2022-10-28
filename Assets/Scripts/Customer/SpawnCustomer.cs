using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SpawnCustomer : MonoBehaviour {
    [Header("Spawn attributes")]
    [SerializeField] private bool enableSpawn;
    [SerializeField] private int minDelaySpawn;
    [SerializeField] private int maxDelaySpawn;
    [SerializeField] private int nbCustomer = 0;
    [SerializeField] private int nbCustomerMax = 5;
    [SerializeField] private AssetReference customerAsset;
    [SerializeField] private AssetReference regularCustomerAsset;

    private GameManager gameManager;

    void Start() {
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
        if (enableSpawn && nbCustomer < nbCustomerMax && gameManager.GetDayTime() == DayTime.Day && CheckProducts()) {
            nbCustomer++;
            if (Random.Range(0, 1) == 0) {
                regularCustomerAsset.InstantiateAsync(transform).Completed += (go) => {
                    go.Result.name = "RegularCustomer " + nbCustomer;
                    go.Result.GetComponent<AICustomer>().requestedProduct = GetRandomProduct();
                };
            }
            else {
                customerAsset.InstantiateAsync(transform).Completed += (go) => {
                    go.Result.name = "Customer " + nbCustomer;
                    go.Result.GetComponent<AICustomer>().requestedProduct = GetRandomProduct();
                };
            }
        }
    }

    public bool CheckProducts() {
        List<ProductSO> doableProduct = new List<ProductSO>();
        foreach (ProductSO product in gameManager.GetProductList()) { //Go through all product
            bool doable = true;
            foreach (IngredientSO ingredient in product.ingredients) //Go through ingredients needed
                if (gameManager.GetIngredientAmount(ingredient) <= 0)
                    doable = false;

            if (doable)
                return true;
        }
        return false;
    }

    public ProductSO GetRandomProduct() {
        List<ProductSO> doableProduct = new List<ProductSO>();
        foreach (ProductSO product in gameManager.GetProductList()) { //Go through all product
            bool doable = true;
            foreach (IngredientSO ingredient in product.ingredients) //Go through ingredients needed
                if (gameManager.GetIngredientAmount(ingredient) <= 0)
                    doable = false;

            if (doable)
                doableProduct.Add(product);
        }
        return doableProduct[Random.Range(0, doableProduct.Count)]; ;
    }

    public void RemoveCustomer() => nbCustomer--;
}
