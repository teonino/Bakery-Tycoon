using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayTimeUI : MonoBehaviour {

    public void SetDayTime(string day) {
        GetComponent<TextMeshProUGUI>().SetText(day);
    }
}
