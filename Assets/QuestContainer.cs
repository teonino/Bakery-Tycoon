using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestContainer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI number;

    public TextMeshProUGUI GetTitle() => title;
    public TextMeshProUGUI GetNumber() => number;
}
