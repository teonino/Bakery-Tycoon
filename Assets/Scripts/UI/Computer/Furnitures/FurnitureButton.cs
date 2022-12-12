using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureButton : MonoBehaviour
{
    private FurnitureManager furnitureManager;
    private FurnitureSO furnitureSO;

    public void SetFurnitureManager(FurnitureManager value) => furnitureManager = value;
    public void SetFurniture(FurnitureSO value) => furnitureSO = value; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
