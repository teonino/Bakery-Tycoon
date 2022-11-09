using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VersionUI : MonoBehaviour
{
    private void Start() {
        GetComponent<TextMeshProUGUI>().SetText("Version : " + Application.version);
    }
}
