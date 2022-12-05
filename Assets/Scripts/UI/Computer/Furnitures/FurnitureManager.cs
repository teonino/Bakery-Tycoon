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

        lenght = furnitures.GetFurnitureCount();
        gameManager = FindObjectOfType<GameManager>();
        playerController = gameManager.GetPlayerController();
    }

    private void Start() {
        for (int i = 0; i < lenght; i++) {
            furnitureButtonAsset.InstantiateAsync().Completed += (go) => {
                go.Result.GetComponent<FurnitureButton>().SetFurnitureManager(this);
                go.Result.GetComponent<FurnitureButton>().SetFurniture(furnitures.GetFurnitures()[nbButton]);
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
                        furnitureRackList.Add(go.Result);
                        SetupButton();
                    };
                }
            }
        }
    }
    private void SetupButton() {
        if (furnitureRackList.Count * maxButtonInRack >= furnitureButtonList.Count) {
            //Set height of buttonPanel to be able to see all buttons
            buttonPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(buttonPanel.GetComponent<RectTransform>().rect.width, furnitureRackList[0].GetComponent<RectTransform>().rect.height * furnitureRackList.Count);

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

    public void Quit(InputAction.CallbackContext context) {
        playerController.playerInput.UI.Quit.performed -= Quit;
        playerController.playerInput.UI.Disable();
        playerController.EnableInput();
        computerPanel.SetActive(false);
    }
}
