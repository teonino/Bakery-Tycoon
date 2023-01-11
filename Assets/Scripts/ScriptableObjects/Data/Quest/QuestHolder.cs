using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHolder : MonoBehaviour
{
    [SerializeField] private InterractQuest interractQuest;

    public InterractQuest GetInterractQuest() => interractQuest;
}
