using System.Collections.Generic;

public class DayStatistics {
    Dictionary<ProductSO, int> productsSold;
    GameManager gameManager;
    int moneySpent;
    int moneyEarned;

    public DayStatistics(GameManager gameManager) {
        productsSold = new Dictionary<ProductSO, int>();

        this.gameManager = gameManager;
        foreach(ProductSO product in gameManager.GetProductList()) {
            productsSold.Add(product, 0);
        }

        moneyEarned = moneySpent = 0;
    }

    public KeyValuePair<ProductSO,int> GetMostProductSold() {
        ProductSO product = null;
        int productAmount = 0;
        foreach (KeyValuePair<ProductSO, int> productSold in productsSold) {
            if (!product) {
                product = productSold.Key;
                productAmount = productSold.Value;
            }

            if(productAmount < productSold.Value) {
                product = productSold.Key;
                productAmount = productSold.Value;
            }
        }
        productAmount = productAmount / GetProductsSold();

        return new KeyValuePair<ProductSO, int>(product, productAmount);
    }

    public int GetProductsSold() {
        int total = 0;
        foreach (KeyValuePair<ProductSO, int> productSold in productsSold) {
            total += productSold.Value;
        }
        return total;
    }

    public void AddProductSold(ProductSO product) {
        productsSold[product]++;
    }

    public StockIngredient GetLowestIngredient() {
        List<StockIngredient> ingredients = gameManager.GetIngredientList();
        StockIngredient lowestIngredient = null;

        foreach (StockIngredient ingredient in ingredients) {
            if (lowestIngredient == null) lowestIngredient = ingredient;
            if (ingredient.amount < lowestIngredient.amount) lowestIngredient = ingredient;
        }
        return lowestIngredient;
    }

    public void AddMoney(int value) {
        moneyEarned += value;
    }

    public void RemoveMoney(int value) {
        moneySpent -= value;
    }
}