﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;


public class CustomNetworkManager : NetworkManager {

    private string password;
    public int buyIn;

    /// <summary>
    /// Creates a match.
    /// </summary>
    /// <param name="param1">Name of the match</param>
    /// <param name="param1">Buy in of the match</param>
    public void CreateMatch(string name, int buyIn) {

        LoadGame();

        singleton.StopMatchMaker();

        this.buyIn = buyIn;

        singleton.StartMatchMaker();
        singleton.matchMaker.CreateMatch(name, 10, true, "", "", "", 0, 0, OnMatchCreate);
    }

    /// <summary>
    /// Joins the first available match.
    /// </summary>
    public void QuickJoin() {
        singleton.StopMatchMaker();
        singleton.StartMatchMaker();
        singleton.matchMaker.ListMatches(0, 10, "", true, 0, 0, QuickJoinCallback);
    }

    /// <summary>
    /// Joins to a private match with a password.
    /// </summary>
    /// <param name="param1">Password</param>
    public void PrivateJoin(string password) {
        this.password = password;

        singleton.StopMatchMaker();
        singleton.StartMatchMaker();
        singleton.matchMaker.ListMatches(0, 10, "", true, 0, 0, PrivateJoinCallback);
    }

    private void QuickJoinCallback(bool success, string extendedInfo, List<MatchInfoSnapshot> matches) {

        if (!success) { return; }

        foreach (MatchInfoSnapshot match in matches) {
            if (match.name.Contains("HIDDEN: ")) {
                continue;
            }
            if (match.currentSize < match.maxSize) {
                JoinMatch(match, string.Empty);
                break;
            }
        }
    }
    private void PrivateJoinCallback(bool success, string extendedInfo, List<MatchInfoSnapshot> matches) {

        if (!success) { return; }

        foreach (MatchInfoSnapshot match in matches) {

            if (match.currentSize < match.maxSize) {
                if (match.name.Equals(password)) {
                    JoinMatch(match, string.Empty);
                    break;
                }
            }
        }
    }
    private void JoinMatch(MatchInfoSnapshot match, string password) {
        LoadGame();
        singleton.matchMaker.JoinMatch(match.networkId, password, "", "", 0, 0, OnMatchJoined);
    }

    private void LoadGame() {
        SceneManager.LoadScene("Game");
    }

    // Class overrides
    public override void OnServerError(NetworkConnection conn, int errorCode) {
        SceneManager.LoadScene(0);
    }
    public override void OnClientError(NetworkConnection conn, int errorCode) {
        SceneManager.LoadScene(0);
    }
    public override void OnServerDisconnect(NetworkConnection conn) {

        print("Client disconnected, removing player");

        TexasHoldEm game = GameObject.FindGameObjectWithTag("Scripts").GetComponent<TexasHoldEm>();
        Player player = null;

        foreach (Player p in game.Players) {
            if (p.GetComponent<NetworkIdentity>().connectionToClient == conn) {
                player = p;
            }
        }

        player.Ready = true;
        player.Folded = true;
        game.RemovePlayer(player);
    }
    public override void OnServerConnect(NetworkConnection conn) {
        GameObject.FindGameObjectWithTag("Scripts").GetComponent<TexasHoldEm>().UpdatePlayers();
    }

    public void CloseServer() {
        singleton.StopMatchMaker();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
