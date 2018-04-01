using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    private bool ready;
    public bool Ready { get { return ready; } set { ready = value; } }

    Card[] cards;

    [SyncVar]
    int money;
    public int Money { get { return money; } set { money = value; } }

    [SyncVar]
    int bet;
    public int Bet { get { return bet; } set { bet = value; } }

    public void ResetCards() {
        throw new System.NotSupportedException("Functionality has not been implemented yet.");
    }

    public void GiveCard(Card card) {
        throw new System.NotSupportedException("Functionality has not been implemented yet.");
    }
}
