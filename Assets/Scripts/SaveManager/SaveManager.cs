using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SaveManager : MonoBehaviour {
    [SerializeField] private ListFurniture furnitures;
    private Transform mainRoom;
    private string filepath = "Assets\\Save\\Savefile.json";

    public CustomizableData JSonFileReader { get; private set; }

    private void Start() {
        mainRoom = GameObject.FindGameObjectWithTag("Level")?.transform;
    }

    public void GenerateWorld() {
        if (File.Exists(filepath))
            Load(filepath);
        else
            print("oui");
    }

    public void Save() {
        CustomizableData data = new CustomizableData();

        mainRoom = GameObject.FindGameObjectWithTag("Level").transform;
        StreamWriter w = new StreamWriter(filepath);

        w.WriteLine("{ \n \"Level\" : [");

        FetchGameObjects(w, mainRoom, true);

        w.WriteLine("] \n }");
        w.Close();

        print("Level Saved");
    }

    private void FetchGameObjects(StreamWriter w, Transform parent, bool parentNotLastChild) {
        CustomizableData data;

        foreach (Transform dataTransform in parent) {
            if (dataTransform.tag != "Outdoor") {
                data = SetCustomizableData(dataTransform);
                w.WriteLine(JsonUtility.ToJson(data));

                bool isPrefab = IsPrefab(data.objectName);

                if (parentNotLastChild && dataTransform != mainRoom.GetChild(mainRoom.childCount - 1))
                    w.WriteLine(",");
                else if (dataTransform.childCount > 0 && !isPrefab)
                    w.WriteLine(",");

                if (!isPrefab && dataTransform.childCount > 0)
                    FetchGameObjects(w, dataTransform, dataTransform != mainRoom.GetChild(mainRoom.childCount - 1));
            }
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

        if (data.objectName != "Empty") {
            if (data.objectName.EndsWith("_A")) {
                data.objectName = Regex.Replace(t.name, @"_A", string.Empty);
                data.typeA = true;
            }
            else if(data.objectName.EndsWith("_B")) {
                data.objectName = Regex.Replace(t.name, @"_B", string.Empty);
                data.typeA = false;
            }
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

            LoadObjects(dataList, level.transform);

            //NEED TO DO RECURSIVE FUNCTION
            //foreach (CustomizableData data in dataList.Level) {
            //    GameObject obj = null, instantiateObj;

            //    if (data.objectName == "Empty") {
            //        focusedChildCount = data.childCount;
            //        currentChildCount = 0;
            //        gameObjectParent = obj;
            //    }
            //    else {
            //        obj = GetAssetReference(data);
            //        currentChildCount++;
            //    }

            //    if (currentChildCount <= focusedChildCount && gameObjectParent != obj) {
            //        if (obj)
            //            instantiateObj = Instantiate(obj, gameObjectParent.transform);
            //        else {
            //            instantiateObj = new GameObject();
            //            instantiateObj.transform.parent = gameObjectParent.transform;
            //        }

            //        instantiateObj.name = data.objectName;
            //        instantiateObj.transform.position = data.position;
            //        instantiateObj.transform.rotation = data.rotation;
            //        instantiateObj.transform.localScale = data.scale;
            //    }

            //    else {
            //        if (obj)
            //            instantiateObj = Instantiate(obj);
            //        else
            //            instantiateObj = new GameObject();
            //        instantiateObj.transform.parent = level.transform;
            //        instantiateObj.name = data.objectName;
            //        instantiateObj.transform.position = data.position;
            //        instantiateObj.transform.rotation = data.rotation;
            //        instantiateObj.transform.localScale = data.scale;
            //        gameObjectParent = instantiateObj;
            //    }
            //}
        }
    }

    private void LoadObjects(CustomizableDataList dataList, Transform parent) {
        foreach (CustomizableData data in dataList.Level) {
            AssetReference asset;
            GameObject instantiateObj;

            if (data.objectName != "Empty") {
                asset = GetAssetReference(data);
                print($"Init {data.objectName} ...");
                if (asset.RuntimeKeyIsValid()) {
                    asset.InstantiateAsync(parent).Completed += (go) => {
                        instantiateObj = go.Result;
                        instantiateObj.name = data.objectName;
                        instantiateObj.transform.position = data.position;
                        instantiateObj.transform.rotation = data.rotation;
                        instantiateObj.transform.localScale = data.scale; 
                        //print($"Init {data.objectName} DONE");
                    };
                } else {
                    print($"Error, {data.objectName} not found");
                }
            }
        }
    }

    public AssetReference GetAssetReference(CustomizableData data) {
        AssetReference returnObject = null;
        foreach (FurnitureSO furniture in furnitures.GetFurnitures()) {
            if (furniture.name == data.objectName) {
                if (data.typeA)
                    returnObject = furniture.GetAssetA();
                else
                    returnObject = furniture.GetAssetB();
            }
        }
        return returnObject;
    }

    public bool IsPrefab(string name) {
        bool returnObject = false;
        foreach (FurnitureSO furniture in furnitures.GetFurnitures()) {
            if (name.Contains(furniture.name)) {
                returnObject = true;
            }
        }
        return returnObject;
    }
}