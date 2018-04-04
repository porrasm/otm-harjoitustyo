using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

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

    private Turn turn;
    public Turn Turn { get { return turn; } set { turn = value; } }

    private Card[] cards;
    public Card[] Cards { get { return cards; } }

    [SerializeField]
    HoldemUI ui;

    void Update() {
        if (!isLocalPlayer) { return; }
        if (Input.GetKeyDown(KeyCode.Space)) {
            CmdSetReady(true);
        }
    }

    //Game actions
    [Command]
    public void CmdFold() {
        turn.fold = true;
        ready = true;
    }
    [Command]
    public void CmdCall() {
        turn.raise = 0;
        ready = true;
    }
    [Command]
    public void CmdRaise(int amount) {
        print("CMDraise: " + amount);
        turn.raise = amount;
        ready = true;
    }


    //Server side methods
    [Command]
    void CmdSetReady(bool ready) {
        this.ready = ready;
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

    //Other
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

}
