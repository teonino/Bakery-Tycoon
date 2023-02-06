using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "name_SO", menuName = "Data/RegularCharacter")]
public class RegularSO : Data
{
    [SerializeField] private string nameNPC;
    [SerializeField] private AssetReference model;
    [SerializeField] private int friendship;
    [Tooltip("Will spawn every x + 4 day")]
    [SerializeField] private int spawnDay;

    public string GetName() => nameNPC;
    public AssetReference GetModel() => model;
    public void IncreaseFrienship() => friendship++;

    public override void ResetValues() {
        friendship = 1;
    }

    public int GetFriendship() => friendship;
}
