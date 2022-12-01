using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Debug", menuName = "Data/Debug")]
public class DebugState : ScriptableObject {
    [SerializeField] bool debug;

    public bool GetDebug() => debug;
}
