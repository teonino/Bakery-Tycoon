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
    [SerializeField] private Money money;
    [SerializeField] private Controller controller;
    [SerializeField] private GameObject computerPanel;
    [SerializeField] private GameObject buttonPanel;
    [SerializeField] private RectTransform scrollRectTransform;
    [SerializeField] private int scrollSpeed;

    private List<GameObject> furnitureButtonList;
    private List<GameObject> furnitureRackList;
    private GameManager gameManager;
    private PlayerController playerController;
    private int nbButton = 0;
    private int lenght = 0;
    private int maxButtonInRack;
    private List<FurnitureType> furnitureTypeFilter;
    private List<FurnitureStyle> furnitureStyleFilter;

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
        gameManager = FindObjectOfType<GameManager>();
        playerController = gameManager.GetPlayerController();
    }

    private void Start() {
        for (int i = 0; i < lenght; i++) {
            furnitureButtonAsset.InstantiateAsync().Completed += (go) => {
                FurnitureSO tmp = furnitures.GetFurnitures()[nbButton];

                go.Result.GetComponent<FurnitureButton>().SetFurnitureManager(this);
                go.Result.GetComponent<FurnitureButton>().SetFurniture(tmp);
                go.Result.name = "Button " + tmp.GetName();
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
                gameManager.SetEventSystemToStartButton(furnitureButtonList[0]);
            else
                gameManager.SetEventSystemToStartButton(null);
        }
    }

    //Set height of buttonPanel to be able to see all buttons
    private void SetVerticalLayoutGroup() {
        int nbRack = 0;
        for (int i = 0; i < furnitureRackList.Count; i++)
            if (furnitureRackList[i].activeSelf)
                nbRack++;

        buttonPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(buttonPanel.GetComponent<RectTransform>().rect.width, furnitureRackList[0].GetComponent<RectTransform>().rect.height * nbRack);
        buttonPanel.GetComponent<RectTransform>().localPosition = new Vector2(buttonPanel.GetComponent<RectTransform>().localPosition.x, 0);
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
                bool inFilter = false;
                FurnitureButton button = furnitureButtonList[i].GetComponent<FurnitureButton>();

                //Check if button match with style filter
                lenght = furnitureStyleFilter.Count;
                for (int j = 0; j < lenght; j++) {
                    if (button.GetFurniture().GetStyle() == furnitureStyleFilter[j]) {
                        inFilter = true;
                    }
                }

                //Check if button match with type filter
                lenght = furnitureTypeFilter.Count;

                if (!inFilter) {
                    for (int j = 0; j < lenght; j++) {
                        if (button.GetFurniture().GetType() == furnitureTypeFilter[j]) {
                            inFilter = true;
                        }
                    }
                }


                //If button is in filter, set true
                furnitureButtonList[i].SetActive(inFilter);
            }

            //Setup buttons in racks
            for (int i = 0; i < rackLenght; i++) {
                int nbActiveButton = 0;
                int childCount = furnitureRackList[i].transform.childCount;

                for (int k = 0; k < childCount; k++)
                    if (furnitureRackList[i].transform.GetChild(k).gameObject.activeSelf)
                        nbActiveButton++;


                for (int k = i * maxButtonInRack; (k < buttonLenght) && (nbActiveButton < maxButtonInRack); k++) {
                    if (furnitureButtonList[k].transform.parent.GetInstanceID() != furnitureRackList[i].transform.GetInstanceID() && furnitureRackList.IndexOf(furnitureButtonList[k].transform.parent.gameObject) > furnitureRackList.IndexOf(furnitureRackList[i]) && furnitureButtonList[k].activeSelf) {
                        furnitureButtonList[k].transform.SetParent(furnitureRackList[i].transform);
                        nbActiveButton++;
                    }
                }
            }

            //Disable racks with no child
            for (int i = 0; i < rackLenght; i++) {
                if (furnitureRackList[i].transform.childCount == 0)
                    furnitureRackList[i].SetActive(false);
                else
                    furnitureRackList[i].SetActive(true);
            }
        }
        SetVerticalLayoutGroup();
    }

    public void Quit(InputAction.CallbackContext context) {
        playerController.playerInput.UI.Quit.performed -= Quit;
        playerController.playerInput.UI.Disable();
        playerController.EnableInput();
        computerPanel.SetActive(false);
    }
}
