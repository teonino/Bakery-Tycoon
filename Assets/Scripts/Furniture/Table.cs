using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour {
    [SerializeField] private List<Chair> chairs;

    public Chair GetChairAvailable() {
        foreach (Chair chair in chairs) {
            if(!chair.ocuppied)
                return chair;
        }
        return null;
    }

    // Update is called once per frame
    void Update() {

    }
}
