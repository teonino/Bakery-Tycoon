using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestContainer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private LocalizedStringComponent localizedString;
    [SerializeField] private TextMeshProUGUI number;

    public TextMeshProUGUI GetTitle() => title;
    public TextMeshProUGUI GetNumber() => number;
    public LocalizedStringComponent GetLocalizedString() => localizedString;
}
