using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TexasHoldEm : NetworkBehaviour {


    Deck deck;
    Card[] tableCards;
    int tableValue;
    int biggestBet;
    List<Player> players;
    Player currentPlayer;
    int dealer;
    
    string gameState;

    private int smallBlind;
    private int bigBlind;

	void Start () {

        if (!isServer) { return; }

        players = new List<Player>();
        deck = new Deck();
        dealer = 0;

	}
	
	void Update () {

        if (!isServer) { return; }

    }

    //Game functionality
    public void Bet() {

    }
    [Command]
    public void CmdBet() {

    }

    public void InitializeRound() {

        gameState = "Preflop";

        deck.InitializeDeck();
        deck.ShuffleDeck();
        tableCards = new Card[5];

        tableValue = 0;

        foreach (Player player in players) {
            player.Needed = 0;
            player.Bet = 0;
        }

    }

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
            StartCoroutine(HoldemRound());
        }

    }

    //Minimum players 3 at the moment
    IEnumerator HoldemRound() {

        //Blinds
        currentPlayer = PlayerByOrder(1);
        if (currentPlayer.CanPay(smallBlind)) {
            currentPlayer.Money -= smallBlind;
            currentPlayer.Bet += smallBlind;
            tableValue += smallBlind;
        }
        currentPlayer = PlayerByOrder(2);
        if (currentPlayer.CanPay(bigBlind)) {
            currentPlayer.Money -= bigBlind;
            currentPlayer.Bet += bigBlind;
            tableValue += bigBlind;
        }

        //Deal
        for (int i = 0; i < 2; i++) {
            foreach (Player p in players) {
                p.GiveCard(deck.GetCard());
                //Card animation
            }
        }

        //Bets
        StartCoroutine(Betting());
        

        //Flop, 3 cards
        gameState = "Flop";

        deck.GetCard();

        for (int i = 0; i < 3; i++) { 
            tableCards[i] = deck.GetCard();
        }

        //Bets

        //River, 4 cards
        gameState = "River";

        deck.GetCard();
        tableCards[3] = deck.GetCard();

        //Bets

        //Turn, 5 cards
        gameState = "Turn";

        deck.GetCard();
        tableCards[4] = deck.GetCard();

        //Bets

        //

        yield return null;
    }

    IEnumerator Betting() {

        for (int i = 1; i <= players.Count; i++) {

            currentPlayer = PlayerByOrder(i);
            currentPlayer.Needed = BiggestBet() - currentPlayer.Bet;

            currentPlayer.EnablePlayerTurn();

            while (!currentPlayer.Ready) {
                yield return null;
            }

            Turn turn = currentPlayer.Turn;

            if (turn.fold) {
                currentPlayer.Folded = true;
            } else {

                if (currentPlayer.CanPay(currentPlayer.Needed + turn.raise)) {
                    currentPlayer.Money -= currentPlayer.Needed + turn.raise;
                    currentPlayer.Bet += currentPlayer.Needed + turn.raise;
                    currentPlayer.Needed = BiggestBet() - currentPlayer.Bet;
                }

            }

            currentPlayer.Ready = false;

        }

    }

    //Other
    Player PlayerByOrder(int amount) {
        return players[(dealer + amount) % (players.Count)];
    }

    public void RemovePlayer(int index) {
        throw new System.NotSupportedException("Functionality has not been implemented yet.");
    }

    public int BiggestBet() {

        int biggest = players[0].Bet;
        for (int i = 1; i < players.Count; i++) {
            if (players[i].Bet > biggest) {
                biggest = players[i].Bet;
            }
        }

        return biggest;
    }

}
