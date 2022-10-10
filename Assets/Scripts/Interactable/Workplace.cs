using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workplace : Interactable
{
    public GameObject workplacePanel;
    //public Product product;

    public override void Effect() {
        workplacePanel.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
