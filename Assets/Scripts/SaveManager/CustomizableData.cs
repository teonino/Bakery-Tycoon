using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomizableData 
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public int childCount;
    public string objectName;
    public bool typeA;
    public bool hasProduct;
    public string productKeyname;
    public int productAmount;

    public override string ToString() {
        return objectName;
    }
}
