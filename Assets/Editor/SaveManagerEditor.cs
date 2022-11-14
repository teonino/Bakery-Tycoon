using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveManager))]
public class SaveManagerEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        SaveManager saveManager = (SaveManager)target;
        if (GUILayout.Button("Generate JSON"))
            saveManager.Save();
        if (GUILayout.Button("Load JSON"))
            saveManager.Load("Asset/Save/Savefile");
    }
}
