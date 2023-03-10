using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SaveManager : MonoBehaviour {
    [SerializeField] private ListFurniture furnitures;
    [SerializeField] private GameObject defaultMainRoom;
    private Transform mainRoom;
    private string filepath = "Assets\\Save\\Savefile.json";

    public CustomizableData JSonFileReader { get; private set; }

    private void Awake() {
        GenerateWorld();
    }

    private void Start() {
        mainRoom = GameObject.FindGameObjectWithTag("Level")?.transform;
    }

    public void GenerateWorld() {
        if (File.Exists(filepath))
            Load(filepath);
        else {
            defaultMainRoom.SetActive(true);
            Save();
        }
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
                data.typeA = true;
            }
            else if(data.objectName.EndsWith("_B")) {
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

            LoadObjects(dataList, level.transform);
        }
    }

    private void LoadObjects(CustomizableDataList dataList, Transform parent) {
        foreach (CustomizableData data in dataList.Level) {
            AssetReference asset;
            GameObject instantiateObj;

            if (data.objectName != "Empty") {
                asset = GetAssetReference(data);
                print($"Init {data.objectName} ...");
                if (asset != null && asset.RuntimeKeyIsValid()) {
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
            if (data.objectName.Contains(furniture.name)) {
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