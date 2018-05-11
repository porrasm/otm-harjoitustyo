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

    private bool winner;
    public bool Winner { get { return winner; } set { winner = value; } }

    [SerializeField]
    HoldemUI ui;

    // Syncvars
    [SyncVar]
    private bool ready, folded, lost;
    public bool Ready { get { return ready; } set { ready = value; } }
    public bool Folded { get { return folded; } set { folded = value; } }
    public bool Lost { get { return lost; } set { lost = value; } }

    [SyncVar]
    private int money, bet, needed;
    public int Money { get { return money; } set { money = value; } }
    public int Bet { get { return bet; } set { bet = value; } }
    public int Needed { get { return needed; } set { needed = value; } }

    [SyncVar]
    private Hand hand;
    public Hand Hand { get { return hand; } set { hand = value; } }

    void Start() {
        if (!isLocalPlayer) {
            ui.gameObject.SetActive(false);
        }
    }

    // Game actions

    /// <summary>
    /// Folds the round.
    /// </summary>
    [Command]
    public void CmdFold() {
        turn.TurnString = transform.name + " folds";
        turn.Fold = true;
        ready = true;
    }

    /// <summary>
    /// Checks the round.
    /// </summary>
    [Command]
    public void CmdCheck() {
        turn.TurnString = transform.name + " checks. ";
        turn.Pay = 0;
        ready = true;
    }

    /// <summary>
    /// Calls the round.
    /// </summary>
    [Command]
    public void CmdCall() {
        if (needed == 0) { CmdCheck(); return; }
        turn.TurnString = transform.name + " calls " + Tools.IntToMoney(needed);
        turn.Pay = 0;
        ready = true;
    }

    /// <summary>
    /// Raises by the amount given.
    /// </summary>
    /// <param name="param1">Raise amount</param>
    [Command]
    public void CmdRaise(int amount) {
        if (amount == 0) { CmdCall(); return; }
        int fixedRaise = amount - needed;
        turn.TurnString = transform.name + " raises by " + Tools.IntToMoney(fixedRaise);
        turn.Pay = amount;
        ready = true;
    }


    // Server side methods

    /// <summary>
    /// Sets the player ready state.
    /// </summary>
    /// <param name="param1">Ready</param>
    [Command]
    public void CmdSetReady(bool ready) {
        this.ready = ready;
    }

    /// <summary>
    /// Sets the player name.
    /// </summary>
    /// <param name="param1">Name.</param>
    [Command]
    public void CmdSetName(string name) {
        RpcSetName(name);
    }

    /// <summary>
    /// Update the name to all clients. Must be called on the server.
    /// </summary>
    /// <param name="param1">Name</param>
    [ClientRpc]
    public void RpcSetName(string name) {
        transform.name = name;
    }

    /// <summary>
    /// Rrsets the player cards.
    /// </summary>
    public void ResetCards() {
        cards = new Card[7];
        RpcResetCards();
    }

    /// <summary>
    /// Resets the cards for the clients. Must be called on the server.
    /// </summary>
    [ClientRpc]
    void RpcResetCards() {
        cards = new Card[7];
    }

    /// <summary>
    /// Gives player a card.
    /// </summary>
    /// <param name="param1">Card</param>>
    public void GiveCard(Card card) {

        for (int i = 0; i < cards.Length; i++) {
            if (cards[i] == null) {
                cards[i] = card;
                break;
            }
        }

        RpcUpdateCards(card.Suit, card.Number);
    }

    /// <summary>
    /// Updates player cards for the local player. Must be called on the server.
    /// </summary>
    /// <param name="param1">Suit</param>
    /// <param name="param2">Number</param>
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

    /// <summary>
    /// Whether a player can pay a certain amount.
    /// </summary>
    /// <param name="param1">Amount</param>
    /// <returns>'true' if the player has enough money</returns>
    public bool CanPay(int amount) {
        return money - amount >= 0;
    }

    /// <summary>
    /// Enables or disables the turn of this player. Must be called on the server.
    /// </summary>
    /// <param name="param1">Enable</param>
    /// <param name="param2">PayUp round>
    public virtual void EnablePlayerTurn(bool enable, bool payUp) {

        if (turn == null) {
            turn = new Turn();
        }
        RpcEnablePlayerTurn(enable, payUp);
    }

    /// <summary>
    /// Enables or disables the turn of this player. Must be called on the server.
    /// </summary>
    /// <param name="param1">Enable</param>
    public void EnablePlayerTurn(bool enable) {

        if (turn == null) {
            turn = new Turn();
        }
        RpcEnablePlayerTurn(enable, false);
    }
    [ClientRpc]
    private void RpcEnablePlayerTurn(bool enable, bool payUp) {

        if (!isLocalPlayer) { return; }

        ui.UpdateUI();
        if (payUp) {
            ui.PayupUI();
        } else {
            ui.EnableUI(enable);
        }
    }


    /// <summary>
    /// Updates the UI for this player. Must be called on the server.
    /// </summary>
    [ClientRpc]
    public void RpcUpdateUI() {
        if (!isLocalPlayer) { return; }
        if (ui != null) {
            ui.UpdateUI();
        }
    }

    /// <summary>
    /// Resets the UI for this player. Must be called on the server.
    /// </summary>
    [ClientRpc]
    public void RpcResetUI() {
        if (!isLocalPlayer) { return; }
        ui.UpdateUI();
        ui.EnableUI(false);
        ui.ResetUI();
    }

    /// <summary>
    /// Enables the game UI for this player. Must be called on the server.
    /// </summary>
    [ClientRpc]
    public void RpcGameUIStart() {
        if (!isLocalPlayer) { return; }
        ui.DisableInit();
        ui.EnablePanel(true);
    }

    /// <summary>
    /// Updates the player hand value for the client. Must be called on the server.
    /// </summary>
    public void UpdateHand() {
        RpcUpdateHand(hand.Value);
    }
    [ClientRpc]
    private void RpcUpdateHand(int handValue) {
        if (!isLocalPlayer || isServer) { return; }
        if (hand == null) {
            hand = new Hand();
        }
        hand.SetValue(handValue);
    }

    /// <summary>
    /// Sets the player position correctly.
    /// </summary>
    /// <param name="param1">Position as Transform</param>
    public void SetPlayerPosition(Transform position) {

        transform.position = position.position;
        transform.eulerAngles = position.eulerAngles;

        RpcSetPlayerPosition(position.position);
        RpcSetPlayerRotation(position.eulerAngles);
    }
    [ClientRpc]
    private void RpcSetPlayerPosition(Vector3 position) {
        transform.position = position;
    }
    [ClientRpc]
    private void RpcSetPlayerRotation(Vector3 rotation) {
        transform.eulerAngles = rotation;
    }

    public int CompareTo(object obj) {
        if (folded) {
            return 1;
        }
        Player player = (Player)obj;
        return hand.CompareTo(player.Hand);
    }
}
