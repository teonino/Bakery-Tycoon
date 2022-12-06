using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialContent", menuName = "TutorialContent")]
public class TutorialContent : ScriptableObject
{
    [SerializeField] private string title;
    [SerializeField] private string dialogue;
    [SerializeField] private GameObject questPanel;

}
