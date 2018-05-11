using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Client : NetworkBehaviour {
    /// <summary>
    /// Disconnects the player and loads back into the main menu. Must be called on the server.
    /// </summary>
    public void MainMenu() {
        RpcMainMenu();
    }
    [ClientRpc]
    private void RpcMainMenu() {
        if (!isLocalPlayer) { return; }
        print("Trying to load main menu");
        GetComponent<NetworkIdentity>().connectionToServer.Disconnect();
        GetComponent<NetworkIdentity>().connectionToServer.Dispose();
        SceneManager.LoadScene(0);
    }
}
