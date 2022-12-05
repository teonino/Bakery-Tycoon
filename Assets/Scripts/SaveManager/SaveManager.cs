using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SaveManager : MonoBehaviour {
    public List<FurnitureSO> furnitures;
    private Transform level;
    private string filepath = "Assets\\Save\\Savefile.json";

    public CustomizableData JSonFileReader { get; private set; }

    private void Start() {
        level = GameObject.FindGameObjectWithTag("Level")?.transform;
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

        w.WriteLine("{ \n \"Level\" : [");

        //Get all objects in level
        foreach (Transform t in level) {
            data = SetCustomizableData(t);
            w.WriteLine(JsonUtility.ToJson(data) + ",");

            if (t.childCount > 1) {
                foreach (Transform t2 in t) {
                    data = SetCustomizableData(t2);
                    w.WriteLine(JsonUtility.ToJson(data));
                    if (t2 != t.GetChild(t.childCount - 1) || t != level.GetChild(level.childCount - 1))
                        w.WriteLine(",");
                }
            }
        }
        w.WriteLine("] \n }");
        w.Close();
    }

    private CustomizableData SetCustomizableData(Transform t) {
        CustomizableData data = new CustomizableData();
        data.position = t.position;
        data.rotation = t.rotation;
        data.scale = t.localScale;
        data.childCount = t.childCount;

        //If gameObject contains only a transform => Empty gameObject that contains child
        if (t.gameObject.GetComponents<Component>().Length == 1)
            data.objectName = "Empty";
        else
            data.objectName = Regex.Replace(t.name, @" [(]\d+[)]", string.Empty);

        return data;
    }

    public class CustomizableDataList {
        public CustomizableData[] Level;
    }

    public void Load(string jsonfile) {
        if (GameObject.FindGameObjectWithTag("Level")) {
            print("Error, the level is already loaded, click 'Unload JSON' to destroy it first");
        }
        else {
            GameObject level = new GameObject();
            level.name = level.tag = "Level";

            string json = File.ReadAllText(jsonfile);
            CustomizableDataList dataList = JsonUtility.FromJson<CustomizableDataList>(json);

            int currentChildCount = 0;
            int focusedChildCount = 0;
            GameObject gameObjectParent = null;

            foreach (CustomizableData data in dataList.Level) {
                GameObject obj = null, instantiateObj;

                if (data.objectName == "Empty") {
                    focusedChildCount = data.childCount;
                    currentChildCount = 0;
                }
                else {
                    obj = GetGameObject(data.objectName);
                    currentChildCount++;
                }

                if (obj) {
                    if (currentChildCount <= focusedChildCount && gameObjectParent != obj)
                        instantiateObj = Instantiate(obj, gameObjectParent.transform);
                    else
                        instantiateObj = Instantiate(obj, level.transform);

                    instantiateObj.transform.position = data.position;
                    instantiateObj.transform.rotation = data.rotation;
                    instantiateObj.transform.localScale = data.scale;
                }
                else {
                    instantiateObj = new GameObject();
                    instantiateObj.transform.parent = level.transform;
                    instantiateObj.name = data.objectName;
                    instantiateObj.transform.position = data.position;
                    instantiateObj.transform.rotation = data.rotation;
                    instantiateObj.transform.localScale = data.scale;
                    gameObjectParent = instantiateObj;
                }
            }
        }
    }

    public GameObject GetGameObject(string name) {
        GameObject returnObject = null;
        foreach (FurnitureSO furniture in furnitures) {
            if (furniture.name == name)
                returnObject = furniture.asset;
        }
        return returnObject;
    }
}
