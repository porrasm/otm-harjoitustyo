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
        throw new System.NotSupportedException("Functionality has not been implemented yet.");
    }

    public void GiveCard(Card card) {
        throw new System.NotSupportedException("Functionality has not been implemented yet.");
    }

    //Other
    public bool CanPay(int amount) {
        return money - amount >= 0;
    }

    public void EnablePlayerTurn() {
        throw new System.NotSupportedException("Functionality has not been implemented yet.");
    }

}
