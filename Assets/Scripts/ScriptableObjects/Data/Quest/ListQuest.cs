using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListQuest", menuName = "Data/ListQuest")]
public class ListQuest : Data {

    [SerializeField] private int nbQuest;
    [SerializeField] private ListProduct products;
    [SerializeField] private Quest mainQuest;
    [SerializeField] private List<Quest> dailyQuests;

    public override void ResetValues() {
        //Daily Quests
        for (int i = 0; i < dailyQuests.Count; i++) 
            dailyQuests[i].SetActive(false);
        
        for (int i = 0; i < nbQuest; i++) {
            int rng = Random.Range(0, dailyQuests.Count);

            //Run until it find a disable quest
            while (dailyQuests[rng].IsActive())
                rng = Random.Range(0, dailyQuests.Count);

            //Set active when 3+ quests
            switch (dailyQuests[rng]) {
                case CreateQuest:
                    CreateQuest createQuest = (CreateQuest)dailyQuests[rng];

                    //Only pick a product player can do
                    ProductSO rngProduct = products.GetRandomProduct();
                    while (!rngProduct.CheckRequirement())
                        rngProduct = products.GetRandomProduct();

                    createQuest.Init(products.GetRandomProduct(), 2);
                    break;
                case InterractQuest:
                    InterractQuest interractQuest = (InterractQuest)dailyQuests[rng];
                    interractQuest.Init();
                    break;
                default:
                    break;
            }
        }
    }

    public List<Quest> GetDailyQuests() => dailyQuests;
}
