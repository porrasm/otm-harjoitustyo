using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Client : NetworkBehaviour {
    public void MainMenu() {
        RpcMainMenu();
    }
    [ClientRpc]
    private void RpcMainMenu() {
        print("Trying to load main menu");
        SceneManager.LoadScene(0);
    }
}
