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
    
    void Update() {
        if (!isLocalPlayer) { return; }
        if (Input.GetKeyDown(KeyCode.Space)) {
            CmdSetReady(true);
            print("ready");
        }
    }

    //Game actions
    [Command]
    public void CmdFold() {
        throw new System.NotSupportedException("Functionality has not been implemented yet.");
    }
    [Command]
    public void CmdCall() {
        throw new System.NotSupportedException("Functionality has not been implemented yet.");
    }
    [Command]
    public void CmdRaise() {
        throw new System.NotSupportedException("Functionality has not been implemented yet.");
    }


    //Server side methods
    [Command]
    void CmdSetReady(bool ready) {
        this.ready = ready;
    }


    public void ResetCards() {
        cards = new Card[2];
    }

    public void GiveCard(Card card) {

        if (cards[0] == null) {
            cards[0] = card;
        } else {
            cards[1] = card;
        }

    }

    //Other
    public bool CanPay(int amount) {
        return money - amount >= 0;
    }

    public void EnablePlayerTurn() {
        throw new System.NotSupportedException("Functionality has not been implemented yet.");
    }
    [ClientRpc]
    public void RpcPlayerTurn() {

        if (!isLocalPlayer) { return; }



    }
    
}
