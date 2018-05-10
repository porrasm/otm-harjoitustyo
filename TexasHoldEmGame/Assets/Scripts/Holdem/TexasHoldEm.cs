using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class TexasHoldEm : NetworkBehaviour {

    private Table table;
    private Deck deck;
    private Card[] tableCards;
    private Player currentPlayer;
    private ScoreboardUI scoreboard;
    private GameObject aiPrefab;

    private Player[] players;
    public Player[] Players { get { return players; } }

    [SyncVar]
    private int tableValue, biggestBet, dealer;
    public int TableValue { get { return tableValue; } }

    private int roundAmount;

    private int buyIn;
    public int BuyIn { get { return buyIn; } }

    private bool roundIsOn;
    public bool RoundIsOn { get { return roundIsOn; } }

    private bool gameIsReady;
    public bool GameIsReady { get { return gameIsReady; } }

    private bool canContinue;
    
    private string gameState;

    // Settings

    [SerializeField]
    private bool betting;

    [SerializeField]
    private bool fillWithAI;

    [SerializeField]
    private int smallBlind = 10, bigBlind = 20;


    void Start() {

        if (!isServer) { return; }
        aiPrefab = Resources.Load<GameObject>("AIPlayer");
        UpdatePlayers();
        deck = new Deck();
        dealer = 0;
        table = GameObject.FindGameObjectWithTag("Table").GetComponent<Table>();
        scoreboard = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<ScoreboardUI>();
        Invoke("PlaceHolderStart", 3);
    }

    void Update() {

        if (!isServer) { return; }

        if (!roundIsOn && roundAmount > 0 && GameIsReady) {
            StartHoldemRound();
        }
    }

    public void UpdatePlayers() {
        GameObject[] p = GameObject.FindGameObjectsWithTag("Player");
        players = new Player[p.Length];
        for (int i = 0; i < p.Length; i++) {
            players[i] = p[i].GetComponent<Player>();
        }
    }

    // Game
    public void PlaceHolderStart() {
        StartCoroutine(PlaceholderStartCoroutine());
    }
    IEnumerator PlaceholderStartCoroutine() {

        gameState = "Game starting...";
        PopUp(gameState);
        buyIn = 2000;

        while (true) {

            UpdatePlayers();

            bool allReady = true;
            foreach (Player player in players) {
                if (!player.Ready) {
                    print(player.name + " is not ready");
                    allReady = false;
                }
            }

            if (allReady) {
                
                UpdatePlayers();

                foreach (Player p in players) {
                    if (!p.Ready) { continue; }
                }

                if (fillWithAI) {
                    for (int i = 0; i < 10 - players.Length; i++) {
                        GameObject newAI = Instantiate(aiPrefab);
                        newAI.transform.name = "AI Player " + (i + 1);
                        NetworkServer.Spawn(newAI);
                    }
                }

                UpdatePlayers();
                SetPlayerPositions();

                foreach (Player p in players) {
                    p.Money = buyIn;
                    p.Ready = false;
                }

                break;
            }

            yield return new WaitForSeconds(1);
        }

        foreach (Player p in players) {
            p.RpcGameUIStart();
        }

        gameIsReady = true;
        roundAmount = 30;
        roundIsOn = false;
        StartHoldemRound();
    }

   public void StartHoldemRound() {
        if (roundIsOn) {
            return;
        }

        PopUp("Starting round ");
        roundIsOn = true;
        StartCoroutine(StartHoldemRoundCoroutine());
    }
    IEnumerator StartHoldemRoundCoroutine() {

        roundIsOn = true;
        print("GAME: Starting round.");

        // Game rules
        float defaultSmallWaitTime = 0.1f;
        smallBlind = 10;
        bigBlind = 20;

        // int buyIn = 2000;

        Card card;

        // Round Initialization
        gameState = "Preflop";

        deck.InitializeDeck();
        deck.ShuffleDeck();
        deck.ShuffleDeck();
        deck.ShuffleDeck();

        tableCards = new Card[5];
        tableValue = 0;

        print("GAME: Resetting players.");
        ResetPlayers();

        // Blinds
        print("GAME: Paying blinds...");
        yield return new WaitForSeconds(1);
        GetNeeded(PlayerByOrder(1));
        Bet(PlayerByOrder(1), smallBlind);
        Tools.UpdatePlayerUIs(players);
        yield return new WaitForSeconds(1);
        GetNeeded(PlayerByOrder(2));
        Bet(PlayerByOrder(2), bigBlind);
        Tools.UpdatePlayerUIs(players);
        yield return new WaitForSeconds(1);

        scoreboard.UpdateScoreBoard();

        // Deal
        print("GAME: Dealing cards...");
        for (int i = 0; i < 2; i++) {
            for (int j = 1; j <= players.Length; j++) {
                Player p = PlayerByOrder(j);
                card = deck.GetCard();

                p.GiveCard(card);
                Transform cardPos = table.GetPlayerPosition(IndexByOrder(j)).GetChild(i);
                table.SpawnCard(p.gameObject, card, cardPos);
                yield return new WaitForSeconds(defaultSmallWaitTime);
            }
        }

        Tools.UpdatePlayerHands(players);
        yield return new WaitForSeconds(0.5f);
        Tools.UpdatePlayerUIs(players);
        yield return new WaitForSeconds(1);

        // Betting round 1
        print("GAME: Betting round 1 starts.");
        canContinue = false;
        StartCoroutine(BetRound());

        while (!canContinue) {
            yield return new WaitForSeconds(1);
        }

        // Flop, first 3 cards
        print("GAME: The flop, first 3 cards");
        for (int i = 0; i < 3; i++) {   
            deck.GetCard();
            card = deck.GetCard();
            GivePlayersCard(card);
            table.SpawnCard(gameObject, card, table.GetCardPosition(i));
            Tools.UpdatePlayerUIs(players);
            PopUp("Table card " + (i + 1) + ": " + card.ToString());
            yield return new WaitForSeconds(1);
        }

        Tools.UpdatePlayerHands(players);
        yield return new WaitForSeconds(0.5f);
        Tools.UpdatePlayerUIs(players);
        yield return new WaitForSeconds(1);

        // Betting round 2
        print("GAME: Betting round 2 starts.");
        canContinue = false;
        StartCoroutine(BetRound());

        while (!canContinue) {
            yield return new WaitForSeconds(1);
        }

        // River, 4 cards
        print("GAME: River, first 4 cards.");
        deck.GetCard();
        card = deck.GetCard();
        GivePlayersCard(card);
        table.SpawnCard(gameObject, card, table.GetCardPosition(3));
        Tools.UpdatePlayerHands(players);
        PopUp("Table card 4:"  + card.ToString());
        yield return new WaitForSeconds(0.5f);
        Tools.UpdatePlayerUIs(players);
        yield return new WaitForSeconds(2);

        // Betting round 3
        print("GAME: Betting round 3 starts.");
        canContinue = false;
        StartCoroutine(BetRound());

        while (!canContinue) {
            yield return new WaitForSeconds(1);
        }

        // Turn, 5 cards
        print("GAME: Turn, all 5 cards.");
        deck.GetCard();
        card = deck.GetCard();
        GivePlayersCard(card);
        table.SpawnCard(gameObject, card, table.GetCardPosition(4));
        Tools.UpdatePlayerHands(players);
        PopUp("Table card 5:" + card.ToString());
        yield return new WaitForSeconds(0.5f);
        Tools.UpdatePlayerUIs(players);
        yield return new WaitForSeconds(2);

        // Betting round 4
        print("GAME: Betting round 4 starts.");
        canContinue = false;
        StartCoroutine(BetRound());

        while (!canContinue) {
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(2);

        Tools.UpdatePlayerUIs(players);

        // Reveal cards
        print("GAME: Revealing cards.");
        foreach (GameObject c in GameObject.FindGameObjectsWithTag("Card")) {

            CardObject cardObject = c.GetComponent<CardObject>();

            if (cardObject.Owner == gameObject) { continue; }
            if (cardObject.Owner.GetComponent<Player>().Folded) { continue; }
            cardObject.TurnCard();
            yield return new WaitForSeconds(defaultSmallWaitTime);
        }

        // Winning and money
        Winner();
        PopUp(currentPlayer.name + " wins with " + Hand.HandToString(currentPlayer.Hand.Value));
        scoreboard.RevealScoreBoard();
        yield return new WaitForSeconds(10);

        // End round
        print("GAME: Killing cards...");
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Card")) {
            g.GetComponent<CardObject>().KillCard();
            yield return new WaitForSeconds(defaultSmallWaitTime);
        }

        scoreboard.UpdateScoreBoard();

        print("GAME: Round end.");
        roundAmount--;
        roundIsOn = false;
    }
 
    // Betting
    IEnumerator BetRound() {

        if (!betting) {
            canContinue = true;
            yield break;
        }

        // Betting round
        print("GAME: Betting start");
        for (int i = 1; i <= players.Length; i++) {
            
            // Player turn
            currentPlayer = PlayerByOrder(i);
            GetNeeded(currentPlayer);
            

            if (currentPlayer.Folded) {
                continue;
            }

            PopUp(currentPlayer.name + " s turn");

            Tools.UpdatePlayerUIs(players);
            scoreboard.UpdateScoreBoard();
            yield return new WaitForSeconds(0.5f);

            PlayerTurn(false);

            while (!currentPlayer.Ready && currentPlayer != null) {
                yield return null;
            }

            if (currentPlayer != null) {
                currentPlayer.EnablePlayerTurn(false);
            }

            // Analyzing turn
            AnalyzeTurn();
        }

        // PayUp Round
        print("GAME: Payup start");
        for (int i = 1; i <= players.Length; i++) {

            // Player turn
            currentPlayer = PlayerByOrder(i);
            GetNeeded(currentPlayer);

            if (currentPlayer.Folded || currentPlayer.Needed == 0) {
                continue;
            }

            Tools.UpdatePlayerUIs(players);
            scoreboard.UpdateScoreBoard();
            yield return new WaitForSeconds(0.5f);

            PlayerTurn(true);

            while (!currentPlayer.Ready && currentPlayer != null) {
                print(currentPlayer);
                yield return null;
            }
            currentPlayer.EnablePlayerTurn(false);

            // Analyzing turn
            AnalyzeTurn();
        }

        yield return new WaitForSeconds(1);
        canContinue = true;
    }

    public void PlayerTurn(bool payUp) {
        currentPlayer.Turn = new Turn();
        GetNeeded(currentPlayer);
        currentPlayer.Ready = false;
        currentPlayer.EnablePlayerTurn(true, payUp);
    }
    public void AnalyzeTurn() {
        Turn turn = currentPlayer.Turn;
        print("Turn string: " + turn.TurnString);
        PopUp(turn.TurnString);

        if (turn.Fold) {
            currentPlayer.Folded = true;
            return;
        }

        Bet(currentPlayer, currentPlayer.Needed + turn.Pay);
    }

    public void Winner() {

        // bool tie = false;
        print("GAME: Results");

        currentPlayer = null;

        Player[] playersCopy = new Player[players.Length];

        for (int i = 0; i < players.Length; i++) {
            playersCopy[i] = players[i];
        }

        Array.Sort(playersCopy);

        currentPlayer = playersCopy[0];

        currentPlayer.Winner = true;
        currentPlayer.Money += tableValue;

        print("GAME: Winner is: " + currentPlayer.name + " with " + currentPlayer.Hand);
        return;
        /*
        int winners = 0;

        for (int i = 0; i < players.Length - 1; i++) {

            if (playersCopy[i].Folded) {
                break;
            }

            if (!playersCopy[i].Hand.Tie(playersCopy[i + 1].Hand)) {
                playersCopy[i].Winner = true;
                winners++;
                break;
            }

            playersCopy[i].Winner = true;

        }

        currentPlayer.Winner = true;
        currentPlayer.Money += tableValue;

        print("GAME: Winner is: " + currentPlayer.name + " with " + currentPlayer.Hand);
        */
    }

    void GivePlayersCard(Card card) {
        foreach (Player p in players) {
            p.GiveCard(card);
        }
    }

    // Other Methods
    public void SetPlayerPositions() {

        for (int i = 0; i < players.Length; i++) {

            players[i].SetPlayerPosition(table.GetPlayerPosition(i));
        }
    }

    public void ResetPlayers() {
        foreach (Player p in players) {
            p.Folded = false;
            p.Bet = 0;
            p.Needed = 0;
            p.ResetCards();
            p.Hand = new Hand();
            p.Hand.SetValue(-1);
            p.RpcResetUI();
            p.Winner = false; 
        }
    }

    void Blinds() {
        currentPlayer = PlayerByOrder(1);
        if (currentPlayer.CanPay(smallBlind)) {
            Bet(currentPlayer, smallBlind);
            print("Player paid small blind. tableValue: " + tableValue);
        }
        currentPlayer = PlayerByOrder(2);
        if (currentPlayer.CanPay(bigBlind)) {
            Bet(currentPlayer, bigBlind);
            print("Player paid big blind. tableValue: " + tableValue);
        }
    }

    public void RemovePlayer(Player player) {

        Player[] newPlayers = new Player[players.Length - 1];

        int index = 0;

        foreach (Player p in players) {
            if (p != player) {
                newPlayers[index] = p;
                index++;
            }
        }

    }
    
    // Tools
    Player PlayerByOrder(int amount) {

        int index = (dealer + amount) % players.Length;
        Player player = players[index];

        while (player == null) {

            index++;
            if (index == players.Length) {
                index = 0;
            }
            player = players[index];
        }

        return player;
    }
    int IndexByOrder(int amount) {

        int index = (dealer + amount) % players.Length;
        Player player = players[index];

        while (player == null) {

            index++;
            if (index == players.Length) {
                index = 0;
            }
            player = players[index];
        }

        return index;
    }
    int GetNeeded(Player player) {
        player.Needed = BiggestBet() - player.Bet;
        return player.Needed;
    }

    public int BiggestBet() {

        int biggest = players[0].Bet;
        for (int i = 1; i < players.Length; i++) {

            if (players[i].Bet > biggest) {
                biggest = players[i].Bet;
            }
        }
        biggestBet = biggest;

        if (biggestBet < bigBlind) {
            biggestBet = bigBlind;
        }

        return biggest;
    }

    public void Bet(Player player, int amount) {

        if (player.CanPay(amount)) {
            tableValue += amount;
            player.Bet += amount;
            player.Money -= amount;
            GetNeeded(player);
        } else {
            Bet(player, player.Money);
        }
    }

    void PopUp(string text) {
        RpcPopUpAllPlayers(text);
    }
    [ClientRpc]
    public void RpcPopUpAllPlayers(string text) {
        Tools.PopUp(text);
    }
}
