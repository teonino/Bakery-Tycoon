using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DelaySpawnSO", menuName = "Data/DelaySpawn")]
public class DelaySpawnCustomer : ScriptableObject {
    private List<Vector2> delaySpawn;
    
    public Vector2 GetDelaySpawn(int index) {
        if (index >= 0 || index < delaySpawn.Count) {
            return delaySpawn[index];
        }
        return Vector2.zero;
    }
}
