using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListQuest", menuName = "Data/ListQuest")]
public class ListQuest : Data {

    [SerializeField] private int nbQuest;
    [SerializeField] private ListProduct products;
    [SerializeField] private List<Quest> quests;

    public override void ResetValues() {
        for (int i = 0; i < quests.Count; i++) 
            quests[i].SetActive(false);
        
        for (int i = 0; i < nbQuest; i++) {
            int rng = Random.Range(0, quests.Count);

            //Run until it find a disable quest
            while (quests[rng].IsActive())
                rng = Random.Range(0, quests.Count);

            //Set active when 3+ quests
            switch (quests[rng]) {
                case CreateQuest:
                    CreateQuest createQuest = (CreateQuest)quests[rng];

                    //Only pick a product player can do
                    ProductSO rngProduct = products.GetRandomProduct();
                    while (!rngProduct.CheckRequirement())
                        rngProduct = products.GetRandomProduct();

                    createQuest.Init(products.GetRandomProduct(), 2);
                    break;
                case InterractQuest:
                    InterractQuest interractQuest = (InterractQuest)quests[rng];
                    interractQuest.Init();
                    break;
                default:
                    break;
            }
        }
    }

    public List<Quest> GetQuestList() => quests;
}
