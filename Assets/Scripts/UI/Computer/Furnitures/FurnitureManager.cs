using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FurnitureManager : MonoBehaviour {
    [SerializeField] private AssetReference furnitureButtonAsset;
    [SerializeField] private AssetReference furnitureRackAsset;
    [SerializeField] private ListFurniture furnitures;
    [SerializeField] private ListFurniture ownedFurnitures;
    [SerializeField] private PlayerControllerSO playerControllerSO;
    [SerializeField] private Money money;
    [SerializeField] private Controller controller;
    [SerializeField] private GameObject assetChoicePanel;
    [SerializeField] private GameObject buttonPanel;
    [SerializeField] private RectTransform scrollRectTransform;
    [SerializeField] private ScrollSpeedSO scrollSpeed;

    private List<GameObject> furnitureButtonList;
    private List<GameObject> furnitureRackList;
    private PlayerController playerController;
    private int nbButton = 0;
    private int lenght = 0;
    private int maxButtonInRack;
    private List<FurnitureType> furnitureTypeFilter;
    private List<FurnitureStyle> furnitureStyleFilter;
    private BuildingMode buildingMode;

    private void OnEnable() {
        if (gameObject.activeSelf) {
            //Manage Inputs
            playerController.DisableInput();
            playerController.playerInput.UI.Enable();
            playerController.playerInput.UI.Quit.performed += Quit;
        }
    }

    // Start is called before the first frame update
    void Awake() {
        furnitureButtonList = new List<GameObject>();
        furnitureRackList = new List<GameObject>();

        furnitureTypeFilter = new List<FurnitureType>();
        furnitureStyleFilter = new List<FurnitureStyle>();

        lenght = furnitures.GetFurnitureCount();
        playerController = playerControllerSO.GetPlayerController();
    }

    public void SetBuildingMode(BuildingMode building) => buildingMode = building;
    public BuildingMode GetBuildingMode() => buildingMode;

    private void Start() {
        for (int i = 0; i < lenght; i++) {
            furnitureButtonAsset.InstantiateAsync().Completed += (go) => {
                FurnitureSO tmp = furnitures.GetFurnitures()[nbButton];

                go.Result.GetComponent<FurnitureButton>().SetFurnitureManager(this);
                go.Result.GetComponent<FurnitureButton>().SetFurniture(tmp);
                go.Result.name = tmp.GetName() + "Button";
                furnitureButtonList.Add(go.Result);
                nbButton++;
                SetupRacks();
            };
        }
    }
    private void SetupRacks() {
        if (furnitureButtonList.Count > 0 && nbButton == lenght) {
            maxButtonInRack = (int)Math.Floor(buttonPanel.GetComponent<RectTransform>().rect.width / furnitureButtonList[0].GetComponent<RectTransform>().sizeDelta.x);
            for (int i = 0; i < furnitureButtonList.Count; i++) {
                if (i % maxButtonInRack == 0) {
                    furnitureRackAsset.InstantiateAsync(buttonPanel.transform).Completed += (go) => {
                        go.Result.name = "Rack " + furnitureRackList.Count;
                        furnitureRackList.Add(go.Result);
                        SetupButton();
                    };
                }
            }
        }
    }
    private void SetupButton() {
        if (furnitureRackList.Count * maxButtonInRack >= furnitureButtonList.Count) {
            SetVerticalLayoutGroup();

            for (int i = 0; i < lenght; i++) {
                if (i / maxButtonInRack < furnitureRackList.Count) {
                    furnitureButtonList[i].transform.SetParent(furnitureRackList[i / maxButtonInRack].transform);
                    furnitureButtonList[i].transform.localScale = Vector3.one;
                }
            }

            if (controller.IsGamepad())
                controller.SetEventSystemToStartButton(furnitureButtonList[0]);
            else
                controller.SetEventSystemToStartButton(null);
        }
    }

    //Set height of buttonPanel to be able to see all buttons
    private void SetVerticalLayoutGroup() {
        int nbRack = 0;
        for (int i = 0; i < furnitureRackList.Count; i++)
            if (furnitureRackList[i].activeSelf)
                nbRack++;

        buttonPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(buttonPanel.GetComponent<RectTransform>().rect.width / 2, furnitureRackList[0].GetComponent<RectTransform>().rect.height * nbRack);
        buttonPanel.GetComponent<RectTransform>().localPosition = new Vector3(buttonPanel.GetComponent<RectTransform>().localPosition.x, 0, 0);
    }

    private void Update() {
        if (controller.IsGamepad()) {
            scrollRectTransform.position -= new Vector3(0, playerControllerSO.GetPlayerController().playerInput.UI.ScrollWheel.ReadValue<Vector2>().y * scrollSpeed.GetScrollSpeed(), 0);
        }
    }

    public void SetStyleFilter(int filter) {
        FurnitureStyle style = 0;
        switch (filter) {
            case 1: style = FurnitureStyle.Basic; break;
            case 2: style = FurnitureStyle.Rustic; break;
            case 3: style = FurnitureStyle.Modern; break;
            case 4: style = FurnitureStyle.Haussmanian; break;
        }

        if (furnitureStyleFilter.Contains(style))
            furnitureStyleFilter.Remove(style);
        else
            furnitureStyleFilter.Add(style);

        ApplyFilter();
    }

    public void SetTypeFilter(int filter) {
        FurnitureType type = 0;
        switch (filter) {
            case 1: type = FurnitureType.Utility; break;
            case 2: type = FurnitureType.Table; break;
            case 3: type = FurnitureType.Chair; break;
            case 4: type = FurnitureType.Shelf; break;
            case 5: type = FurnitureType.Decoration; break;
            case 6: type = FurnitureType.Floor; break;
            case 7: type = FurnitureType.Wall; break;
        }

        if (furnitureTypeFilter.Contains(type))
            furnitureTypeFilter.Remove(type);
        else
            furnitureTypeFilter.Add(type);

        ApplyFilter();
    }

    private void ApplyFilter() {
        int buttonLenght = furnitureButtonList.Count;
        int rackLenght = furnitureRackList.Count;
        int lenght = furnitureTypeFilter.Count + furnitureStyleFilter.Count;
        List<GameObject> activeButtons = new List<GameObject>();

        if (lenght == 0) {
            for (int i = 0; i < buttonLenght; i++) {
                furnitureButtonList[i].SetActive(true);
            }

            for (int i = 0; i < rackLenght; i++) {
                if (furnitureRackList[i].transform.childCount > maxButtonInRack) {
                    int childCount = furnitureRackList[i].transform.childCount;
                    for (int j = 0; j < childCount - maxButtonInRack; j++) {
                        furnitureRackList[i].transform.GetChild(maxButtonInRack).SetParent(furnitureRackList[i + 1].transform);
                    }
                }
                furnitureRackList[i].SetActive(true);
            }
        }
        else {
            //Set buttons in racks
            for (int i = 0; i < buttonLenght; i++) {
                bool inStyleFilter = false;
                FurnitureButton button = furnitureButtonList[i].GetComponent<FurnitureButton>();

                //Check if button match with style filter
                lenght = furnitureStyleFilter.Count;
                for (int j = 0; j < lenght; j++) {
                    if (button.GetFurniture().GetStyle() == furnitureStyleFilter[j]) {
                        inStyleFilter = true;
                    }
                }

                //If no style filter, then true
                if (lenght == 0)
                    inStyleFilter = true;

                //Check if button match with type filter
                bool inTypeFilter = false;
                lenght = furnitureTypeFilter.Count;

                for (int j = 0; j < lenght; j++) {
                    if (button.GetFurniture().GetType() == furnitureTypeFilter[j]) {
                        inTypeFilter = true;
                    }
                }

                //If no type filter, then true
                if (lenght == 0)
                    inTypeFilter = true;

                //If button is in filter, set true
                if (inStyleFilter && inTypeFilter)
                    activeButtons.Add(furnitureButtonList[i]);

                furnitureButtonList[i].SetActive(inStyleFilter && inTypeFilter);
            }


            //Setup buttons in racks
            for (int i = 0; i < rackLenght; i++) {
                for (int j = 0; (j < maxButtonInRack) && ((j + i * maxButtonInRack) < activeButtons.Count); j++) {
                    activeButtons[j + i * maxButtonInRack].transform.SetParent(furnitureRackList[i].transform);
                }
            }

            activeButtons.Clear();

            //Disable racks with no child active
            for (int i = 0; i < rackLenght; i++) {
                bool hasOneChildActif = false;
                int childCount = furnitureRackList[i].transform.childCount;
                for (int j = 0; j < childCount; j++)
                    if (furnitureRackList[i].transform.GetChild(j).gameObject.activeSelf)
                        hasOneChildActif = true;

                if (hasOneChildActif)
                    furnitureRackList[i].SetActive(true);
                else
                    furnitureRackList[i].SetActive(false);
            }
        }
        SetVerticalLayoutGroup();
    }

    public void DisplayAssetChoice(FurnitureSO furniture) {
        assetChoicePanel.SetActive(true);
        AssetChoiceManager choiceManager = assetChoicePanel.GetComponent<AssetChoiceManager>();

        choiceManager.SetFurnitureSO(furniture);
        choiceManager.SetFurnitureManager(this);
    }

    public void AddOwnedFurniture(FurnitureSO furniture) {
        if (furniture.GetPrice() <= money.GetMoney()) {
            ownedFurnitures.AddFurniture(furniture);
            print($"{furniture.GetName()} bought");
        }
    }

    private void Quit(InputAction.CallbackContext context) {
        Quit();
    }

    public void Quit() {
        playerController.playerInput.UI.Quit.performed -= Quit;
        playerController.playerInput.UI.Disable();
        buildingMode.Effect();
        gameObject.SetActive(false);
    }
}
