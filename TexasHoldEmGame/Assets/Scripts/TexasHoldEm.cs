using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TexasHoldEm : NetworkBehaviour {

    Card[] deck;
    int deckIndex;

	void Start () {

        if (!isServer) { return; }

        deck = new Card[52];
	}
	
	void Update () {

        if (!isServer) { return; }

    }
}
