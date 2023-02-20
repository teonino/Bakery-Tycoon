using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "_MandatoryList", menuName = "Data/MandatoryList")]
public class MandatoryFurnitureList : Data {
    [SerializeField] List<GameObject> list;

    public bool MoreThanOne() => list.Count > 1;
    public bool Remove(GameObject go) => list.Remove(go);
    public void Add(GameObject go) => list.Add(go);
    public override void ResetValues() => list.Clear();
}
