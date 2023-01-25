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
            Load(filepath);
        else
            print("oui");
    }

    public void Save() {
        CustomizableData data = new CustomizableData();

        level = GameObject.FindGameObjectWithTag("Level").transform;
        StreamWriter w = new StreamWriter(filepath);

        w.WriteLine("{ \n \"Level\" : [");

        FetchGameObjects(w, level, true);

        w.WriteLine("] \n }");
        w.Close();

        print("Level Saved");
    }

    private void FetchGameObjects(StreamWriter w, Transform parent, bool parentNotLastChild) {
        CustomizableData data;

        foreach (Transform dataTransform in parent) {
            data = SetCustomizableData(dataTransform);
            w.WriteLine(JsonUtility.ToJson(data));


            if (parentNotLastChild && dataTransform != level.GetChild(level.childCount - 1))
                w.WriteLine(",");
            else if (dataTransform.childCount > 0)
                w.WriteLine(",");

            if (!GetGameObject(data.objectName) && dataTransform.childCount > 0)
                FetchGameObjects(w, dataTransform, dataTransform != level.GetChild(level.childCount - 1));
        }
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
        else {
            data.objectName = Regex.Replace(t.name, @" [(]\d+[)]", string.Empty);
            data.objectName = Regex.Replace(data.objectName, @"([(]Clone[)])", string.Empty);
        }
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

            //NEED TO DO RECURSIVE FUNCTION
            foreach (CustomizableData data in dataList.Level) {
                GameObject obj = null, instantiateObj;

                if (data.objectName == "Empty") {
                    focusedChildCount = data.childCount;
                    currentChildCount = 0;
                    gameObjectParent = obj;
                }
                else {
                    obj = GetGameObject(data.objectName);
                    currentChildCount++;
                }

                if (currentChildCount <= focusedChildCount && gameObjectParent != obj) {
                    if (obj)
                        instantiateObj = Instantiate(obj, gameObjectParent.transform);
                    else {
                        instantiateObj = new GameObject();
                        instantiateObj.transform.parent = gameObjectParent.transform;
                    }

                    instantiateObj.name = data.objectName;
                    instantiateObj.transform.position = data.position;
                    instantiateObj.transform.rotation = data.rotation;
                    instantiateObj.transform.localScale = data.scale;
                }

                else {
                    if (obj)
                        instantiateObj = Instantiate(obj);
                    else
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

    private void LoadObjects() {
        //foreach (CustomizableData data in dataList.Level) {

        //}
    }

    public GameObject GetGameObject(string name) {
        GameObject returnObject = null;
        foreach (FurnitureSO furniture in furnitures) {
            if (furniture.name == name) {
                //returnObject = furniture.GetAssets();
            }
        }
        return returnObject;
    }
}
