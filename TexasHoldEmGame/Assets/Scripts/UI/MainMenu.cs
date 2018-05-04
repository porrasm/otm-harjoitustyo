using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    [SerializeField]
    private CustomNetworkManager networkManager;

    public Transform MainMenuPanel;
    private Transform currentMenu;

    [SerializeField]
    private Transform createMenu, joinMenu, settingsMenu;

    /*
    [SerializeField]
    private Button backBtton, singePlayerButton, createMenuButton, joinMenuButton, settingsMenuButton,
        createServerButton, joinServerButton, quickJoinButton;
    */

    [SerializeField]
    private InputField createName, createPassword, createBuyIn, joinName, joinPassword;

    void Start() {
        currentMenu = MainMenuPanel;
        currentMenu.gameObject.SetActive(true);
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
    public void ActivateSettingsMenu() {
        currentMenu.gameObject.SetActive(false);
        currentMenu = settingsMenu;
        currentMenu.gameObject.SetActive(true);
    }

    // Create

    public void CreateMatch() {

        string serverName = createName.text;
        bool usePassword = true;

        if (serverName.Equals("")) {
            return;
        }

        string serverPassword = createPassword.text;

        if (serverPassword.Equals("")) {
            usePassword = false;
        }
        
        int serverBuyIn = 20;

        if (Tools.CorrectInput(createBuyIn.text)) {
            serverBuyIn = Tools.MoneyToInt(createBuyIn.text);
        }

        networkManager.CreateMatch(serverName, serverPassword, serverBuyIn);
    }

    // Join

    public void QuickJoin() {
        networkManager.QuickJoin();
    }
}
