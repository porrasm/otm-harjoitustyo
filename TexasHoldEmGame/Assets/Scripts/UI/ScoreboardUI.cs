using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class ScoreboardUI : NetworkBehaviour {

    private TexasHoldEm game;

    [SerializeField]
    private Transform playerParent;

    void Start() {
        if (!isServer) { return; }
        game = GameObject.FindGameObjectWithTag("Scripts").GetComponent<TexasHoldEm>();
    }

    public void InitializeScoreboard() {
        RpcAmountToDisable(10 - game.Players.Length);
    }

    [ClientRpc]
    void RpcAmountToDisable(int amount) {
        for (int i = 0; i < amount; i++) {
            playerParent.transform.GetChild(10 - i).gameObject.SetActive(false);
        }
    }

    public void UpdateScoreBoard() {

        print("Updating scoreboard.");

        for (int i = 0; i < game.Players.Length; i++) {
            RpcSetOne(i, game.Players[i].name, "-", 0, game.Players[i].Money);
        }
    }

    public void RevealScoreBoard() {

        print("Revealing scoreboard.");

        Player[] sorted = new Player[game.Players.Length];

        for (int i = 0; i < game.Players.Length; i++) {
            sorted[i] = game.Players[i];
        }
        Array.Sort(sorted);

        for (int i = 0; i < sorted.Length; i++) {
            if (sorted[i].Folded) {
                RpcSetOne(i, sorted[i].name, "Folded", game.TableValue, sorted[i].Money);
            } else {

                if (sorted[i].Winner) {
                    RpcSetOne(i, sorted[i].name, sorted[i].Hand.ToString(), game.TableValue, sorted[i].Money);
                } else {
                    RpcSetOne(i, sorted[i].name, sorted[i].Hand.ToString(), 0, sorted[i].Money);
                }
            }
        }
    }
	
    [ClientRpc]
    void RpcSetOne(int index, string name, string hand, int win, int money) {

        Text nameText = playerParent.GetChild(index).GetChild(0).GetChild(0).GetComponent<Text>();
        Text handText = playerParent.GetChild(index).GetChild(0).GetChild(1).GetComponent<Text>();
        Text winText = playerParent.GetChild(index).GetChild(0).GetChild(2).GetComponent<Text>();
        Text moneyText = playerParent.GetChild(index).GetChild(0).GetChild(3).GetComponent<Text>();

        nameText.text = name;
        handText.text = hand;
        winText.text = Tools.IntToMoney(win);
        moneyText.text = Tools.IntToMoney(money);
    }

    // Client actions
    public void ShowScoreBoard(bool enable) {
        transform.GetChild(0).gameObject.SetActive(enable);
    }
}
