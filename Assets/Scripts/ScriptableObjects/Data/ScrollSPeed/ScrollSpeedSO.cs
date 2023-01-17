using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScrollSpeed", menuName = "Data/ScrollSpeed")]
public class ScrollSpeedSO : ScriptableObject {
    [SerializeField] private float scrollSpeed;

    public float GetScrollSpeed()=> scrollSpeed;
}
