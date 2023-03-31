using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "name_SO", menuName = "Data/RegularCharacter")]
public class RegularSO : Data {
    [SerializeField] private string nameNPC;
    [SerializeField] private ListRegular listRegular;
    [SerializeField] private AssetReference model;
    [SerializeField] private int friendship;

    public string GetName() => nameNPC;
    public AssetReference GetModel() => model;
    public void AddFrienship(int i) {
        friendship += i;

        if (friendship < 1) 
            friendship = 1;

        listRegular.AddFriendship(i);
    }
    public override void ResetValues() {
        friendship = 1;
    }

    public int GetFriendship() => friendship;
}
