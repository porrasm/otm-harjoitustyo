using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    [SerializeField]
    private CustomNetworkManager networkManager;

    public Transform MainMenuPanel;
    private Transform currentMenu;

    [SerializeField]
    private Transform createMenu, joinMenu;

    /*
    [SerializeField]
    private Button backBtton, singePlayerButton, createMenuButton, joinMenuButton, settingsMenuButton,
        createServerButton, joinServerButton, quickJoinButton;
    */

    [SerializeField]
    private InputField createPassword, createBuyIn, joinPassword;

    void Start() {
        currentMenu = MainMenuPanel;
        currentMenu.gameObject.SetActive(true);
    }

    private CustomNetworkManager GetNetworkManager() {
        if (networkManager == null) {
            networkManager = GameObject.Find("NetworkManager").GetComponent<CustomNetworkManager>();
        }
        return networkManager;
    }

    // Main

    public void ActivateMainMenu() {
        currentMenu.gameObject.SetActive(false);
        currentMenu = MainMenuPanel;
        currentMenu.gameObject.SetActive(true);
    }

    public void ActivateCreateMenu() {
        currentMenu.gameObject.SetActive(false);
        currentMenu = createMenu;
        currentMenu.gameObject.SetActive(true);
    }
    public void ActivateJoinMenu() {
        currentMenu.gameObject.SetActive(false);
        currentMenu = joinMenu;
        currentMenu.gameObject.SetActive(true);
    }
    public void Quit() {
        Application.Quit();
    }

    // Create

    public void CreateMatch() {

        string serverName = createPassword.text;
        int serverBuyIn;

        if (serverName.Equals(string.Empty)) {
            serverName = string.Empty + Random.value;
        } else {
            serverName = "HIDDEN: " + createPassword.text;
        }
        
        if (createBuyIn.text.Equals(string.Empty)) {
            serverBuyIn = 2000;
        } else if (!Tools.CorrectInput(createBuyIn.text)) {
            return;
        } else {
            serverBuyIn = Tools.MoneyToInt(createBuyIn.text);
            if (serverBuyIn < 200) {
                return;
            }
        }    

        GetNetworkManager().CreateMatch(serverName, serverBuyIn);
    }

    // Join

    public void QuickJoin() {
        GetNetworkManager().QuickJoin();
    }
    public void JoinPrivate() {
        string password = "HIDDEN: " + joinPassword.text;
        GetNetworkManager().PrivateJoin(password);
    }
}
