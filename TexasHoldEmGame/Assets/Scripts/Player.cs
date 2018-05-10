using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class Player : NetworkBehaviour, IComparable {

    private Turn turn;
    public Turn Turn { get { return turn; } set { turn = value; } }

    private Card[] cards;
    public Card[] Cards { get { return cards; } }

    bool winner;
    public bool Winner { get { return winner; } set { winner = value; } }

    [SerializeField]
    HoldemUI ui;

    // Syncvars
    [SyncVar]
    private bool ready;
    public bool Ready { get { return ready; } set { ready = value; } }

    [SyncVar]
    private bool folded;
    public bool Folded { get { return folded; } set { folded = value; } }

    [SyncVar]
    private int money;
    public int Money { get { return money; } set { money = value; } }

    [SyncVar]
    private int bet;
    public int Bet { get { return bet; } set { bet = value; } }

    [SyncVar]
    private int needed;
    public int Needed { get { return needed; } set { needed = value; } }

    [SyncVar]
    private Hand hand;
    public Hand Hand { get { return hand; } set { hand = value; } }

    void Start() {
        if (!isLocalPlayer) {
            ui.gameObject.SetActive(false);
            transform.GetChild(0).GetComponent<Camera>().enabled = false;
        }
    }

    // Game actions
    [Command]
    public void CmdFold() {
        print("Player: " + transform.name + " Folded.");
        turn.TurnString = transform.name + " folds";
        turn.Fold = true;
        ready = true;
    }
    [Command]
    public void CmdCall() {
        print("Player: " + transform.name + " called.");
        turn.TurnString = transform.name + " calls " + Tools.IntToMoney(needed);
        turn.Pay = 0;
        ready = true;
    }
    [Command]
    public void CmdRaise(int amount) {
        print("Player: " + transform.name + " raised: " + amount);
        turn.TurnString = transform.name + " rases by " + Tools.IntToMoney(amount);
        turn.Pay = amount;
        ready = true;
    }


    // Server side methods
    [Command]
    public void CmdSetReady(bool ready) {
        this.ready = ready;
    }
    [Command]
    public void CmdSetName(string name) {
        RpcSetName(name);
    }
    [ClientRpc]
    public void RpcSetName(string name) {
        transform.name = name;
    }

    public void ResetCards() {
        cards = new Card[7];
        RpcResetCards();
    }
    [ClientRpc]
    void RpcResetCards() {
        cards = new Card[7];
    }


    public void GiveCard(Card card) {

        for (int i = 0; i < cards.Length; i++) {
            if (cards[i] == null) {
                cards[i] = card;
                break;
            }
        }

        RpcUpdateCards(card.Suit, card.Number);
    }
    [ClientRpc]
    public void RpcUpdateCards(int suit, int number) {

        if (!isLocalPlayer || isServer) { return; }

        Card card = new Card();
        card.SetCard(suit, number);

        print("GIVING CARD '" + card + "' FOR PLAYER: " + transform.name);

        print("Updating for local player: " + card);

        for (int i = 0; i < cards.Length; i++) {
            if (cards[i] == null) {
                cards[i] = card;
                break;
            }
        }
    }

    // Other
    public bool CanPay(int amount) {
        return money - amount >= 0;
    }
    public virtual void EnablePlayerTurn(bool enable, bool payUp) {

        if (turn == null) {
            turn = new Turn();
        }
        RpcEnablePlayerTurn(enable, payUp);
    }

    public void EnablePlayerTurn(bool enable) {

        if (turn == null) {
            turn = new Turn();
        }
        RpcEnablePlayerTurn(enable, false);
    }
    [ClientRpc]
    public void RpcEnablePlayerTurn(bool enable, bool payUp) {

        if (!isLocalPlayer) { return; }

        ui.UpdateUI();
        if (payUp) {
            ui.PayupUI();
        } else {
            ui.EnableUI(enable);
        }
    }

    [ClientRpc]
    public void RpcUpdateUI() {
        if (!isLocalPlayer) { return; }
        if (ui != null) {
            ui.UpdateUI();
        }
    }

    [ClientRpc]
    public void RpcResetUI() {
        if (!isLocalPlayer) { return; }
        ui.UpdateUI();
        ui.EnableUI(false);
        ui.ResetUI();
    }

    [ClientRpc]
    public void RpcGameUIStart() {
        if (!isLocalPlayer) { return; }
        ui.DisableInit();
        ui.EnablePanel(true);
    }

    public void SetPlayerPosition(Transform position) {

        transform.position = position.position;
        transform.eulerAngles = position.eulerAngles;

        RpcSetPlayerPosition(position.position);
        RpcSetPlayerRotation(position.eulerAngles);
    }
    [ClientRpc]
    public void RpcSetPlayerPosition(Vector3 position) {
        transform.position = position;
    }
    [ClientRpc]
    public void RpcSetPlayerRotation(Vector3 rotation) {
        transform.eulerAngles = rotation;
    }

    public int CompareTo(object obj) {
        if (folded) {
            return 1;
        }
        Player player = (Player)obj;
        return hand.CompareTo(player.Hand);
    }

    // Preset pop ups

    public string PlayerStartsTurn() {
        return transform.name + "'s turn.";
    }
}
