using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SaveManager : MonoBehaviour {
    private Transform level;
    private string filepath = "Assets\\Save\\Savefile.json";

    private void Start() {
        level = GameObject.FindGameObjectWithTag("Level").transform;
    }

    public void GenerateWorld() {
        if (File.Exists(filepath))
            print("oui");
        else
            Load(filepath);
    }

    public void Save() {
        CustomizableData data = new CustomizableData();
        level = GameObject.FindGameObjectWithTag("Level").transform;
        StreamWriter w = new StreamWriter(filepath);

        w.WriteLine("{ \n \"Level\" : {");

        //Get all objects in level
        foreach (Transform t in level) {
            data = SetCustomizableData(t);
            w.WriteLine("\"" + t.name + "\" :" + JsonUtility.ToJson(data) + ",");

            if (t.childCount > 1) {
                foreach (Transform t2 in t) {
                    data = SetCustomizableData(t);
                    w.WriteLine("\"" + t2.name + "\" :" + JsonUtility.ToJson(data));
                    if (t2 != t.GetChild(t.childCount - 1) || t != level.GetChild(level.childCount - 1)) 
                        w.WriteLine(",");                   
                }
            }
        }
        w.WriteLine("} \n }");
        w.Close();

    }

    private CustomizableData SetCustomizableData(Transform t) {
        CustomizableData data = new CustomizableData();
        data.position = t.position;
        data.rotation = t.rotation;
        data.scale = t.localScale;
        data.childCount = t.childCount;
        return data;

        //PrefabUtility.GetCorrespondingObjectFromSource(t.gameObject);
    }

    public void Load(string json) {
        // AssetDatabase.LoadAssetAtPath("Assets/Prefab/Furniture/GameObject.prefab", typeof(GameObject));
    }    
}
