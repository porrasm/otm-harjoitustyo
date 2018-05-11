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

    /// <summary>
    /// Loads the main menu.
    /// </summary>
    public void ActivateMainMenu() {
        currentMenu.gameObject.SetActive(false);
        currentMenu = MainMenuPanel;
        currentMenu.gameObject.SetActive(true);
    }

    /// <summary>
    /// Loads the create menu.
    /// </summary>
    public void ActivateCreateMenu() {
        currentMenu.gameObject.SetActive(false);
        currentMenu = createMenu;
        currentMenu.gameObject.SetActive(true);
    }

    /// <summary>
    /// Loads the join menu.
    /// </summary>
    public void ActivateJoinMenu() {
        currentMenu.gameObject.SetActive(false);
        currentMenu = joinMenu;
        currentMenu.gameObject.SetActive(true);
    }

    /// <summary>
    /// Quits the application.
    /// </summary>
    public void Quit() {
        Application.Quit();
    }

    // Create

    /// <summary>
    /// Creates a match based on the settings.
    /// </summary>
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

    /// <summary>
    /// Calls the quick join method on the NetworkManager
    /// </summary>
    public void QuickJoin() {
        GetNetworkManager().QuickJoin();
    }

    /// <summary>
    /// Calls the private join method on the NetworkManager
    /// </summary>
    public void JoinPrivate() {
        string password = "HIDDEN: " + joinPassword.text;
        GetNetworkManager().PrivateJoin(password);
    }
}
