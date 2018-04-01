using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TexasHoldEm : NetworkBehaviour {

    Deck deck;
    Table table;
    List<Player> players;

    string gameState;

	void Start () {

        if (!isServer) { return; }

        players = new List<Player>();
        deck = new Deck();
	}
	
	void Update () {

        if (!isServer) { return; }

    }

    //Game functionality

    //Game progression
    IEnumerator GameStart() {

        gameState = "Game starting...";

        while (true) {

            bool allReady = true;
            foreach (Player player in players) {
                if (!player.Ready) {
                    allReady = false;
                }
            }

            if (allReady) {
                break;
            }

            yield return new WaitForSeconds(1);
        }

    }

    IEnumerator HoldemRound() {

        gameState = "Preflop";

        table.InitializeTable();
        deck.InitializeDeck();
        deck.ShuffleDeck();

        //Blinds

        //Bets

        for (int i = 0; i < 2; i++) {
            foreach (Player player in players) {
                player.GiveCard(deck.GetCard());
                //Card animation
            }
        }

        //Flop, 3 cards
        gameState = "Flop";

        deck.GetCard();

        table.DealCard(deck.GetCard());
        table.DealCard(deck.GetCard());
        table.DealCard(deck.GetCard());

        //Bets

        //River, 4 cards
        gameState = "River";

        deck.GetCard();
        table.DealCard(deck.GetCard());

        //Bets

        //Turn, 5 cards
        gameState = "Turn";

        deck.GetCard();
        table.DealCard(deck.GetCard());

        //Bets

        //

        yield return null;
    }

}
