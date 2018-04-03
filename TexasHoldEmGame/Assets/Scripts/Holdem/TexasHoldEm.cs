using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TexasHoldEm : NetworkBehaviour {

    Table table;
    Deck deck;
    Card[] tableCards;
    int tableValue;
    int biggestBet;
    Player[] players;
    Player currentPlayer;
    int dealer;
    bool roundIsOn;

    string gameState;

    private int smallBlind;
    private int bigBlind;

    void Start() {

        if (!isServer) { return; }

        UpdatePlayers();
        deck = new Deck();
        dealer = 0;
        table = GameObject.FindGameObjectWithTag("Table").GetComponent<Table>();

    }

    void Update() {

        if (!isServer) { return; }

        if (Input.GetKeyDown(KeyCode.Return)) {
            UpdatePlayers();
            StartCoroutine(GameStart());
            print("Starting game");
        }

    }

    void UpdatePlayers() {
        GameObject[] p = GameObject.FindGameObjectsWithTag("Player");
        players = new Player[p.Length];
        for (int i = 0; i < p.Length; i++) {
            players[i] = p[i].GetComponent<Player>();
        }
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
            player.ResetCards();
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
                print("All are ready");
                StartCoroutine(HoldemRound());
                break;
            }

            yield return new WaitForSeconds(1);
        }

    }

    //Minimum players 3 at the moment
    IEnumerator HoldemRound() {

        print("Starting round");

        roundIsOn = true;
        InitializeRound();

        Blinds();

        //Deal
        for (int i = 0; i < 2; i++) {
            for (int j = 1; j <= players.Length; j++) {
                Player p = PlayerByOrder(j);
                Card card = deck.GetCard();
                p.GiveCard(card);
                Transform cardPos = table.GetPlayerPosition(indexByOrder(j)).GetChild(i);
                table.SpawnCard(p, card, cardPos);
                yield return new WaitForSeconds(0.1f);
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

    void Blinds() {
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
    }

    IEnumerator Betting() {

        for (int i = 1; i <= players.Length; i++) {

            currentPlayer = PlayerByOrder(i);
            GetNeeded(currentPlayer);

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

        for (int i = 1; i <= players.Length; i++) {

            currentPlayer = PlayerByOrder(i);

            if (GetNeeded(currentPlayer) == 0) {
                continue;
            }

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
        return players[(dealer + amount) % (players.Length)];
    }
    int indexByOrder(int amount) {
        return (dealer + amount) % (players.Length);
    }
    int GetNeeded(Player player) {
        player.Needed = BiggestBet() - player.Bet;
        return player.Needed;
    }

    public void RemovePlayer(int index) {
        throw new System.NotSupportedException("Functionality has not been implemented yet.");
    }

    public int BiggestBet() {

        int biggest = players[0].Bet;
        for (int i = 1; i < players.Length; i++) {
            if (players[i].Bet > biggest) {
                biggest = players[i].Bet;
            }
        }

        return biggest;
    }

}
