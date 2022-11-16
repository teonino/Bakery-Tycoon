using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipMorning : MonoBehaviour
{
    DayManager dayManager;
    // Start is called before the first frame update
    void Start() => dayManager = FindObjectOfType<DayManager>();
    public void SkipMorningAction() {
        dayManager.Updateday();
        Destroy(gameObject);
    }
}
