using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListQuest", menuName = "Data/ListQuest")]
public class ListQuest : Data {

    [SerializeField] private int nbQuest;
    [SerializeField] private ListProduct products;
    [SerializeField] private List<Quest> quests;

    public override void ResetValues() {
        for (int i = 0; i < nbQuest; i++) {
            int rng = Random.Range(0, quests.Count);

            //Run until it find a disable quest
            while (quests[rng].IsActive())
                rng = Random.Range(0, quests.Count);

            //Don't add => Randomize which quest will be randomize
            switch (quests[rng]) {
                case CreateQuest:
                    CreateQuest createQuest = (CreateQuest) quests[rng];
                    createQuest.Init(products.GetRandomProduct(), 2);
                    break;
                case InterractQuest: 
                    break;
                default: 
                    break;
            }

        }
    }

    public List<Quest> GetQuestList() => quests;
}
