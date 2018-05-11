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
        buyIn = GameObject.Find("NetworkManager").GetComponent<CustomNetworkManager>().buyIn;

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
        smallBlind = buyIn / 200;
        bigBlind = smallBlind * 2;

        Card card;

        // Round Initialization
        gameState = "Preflop";

        deck.InitializeDeck();
        deck.ShuffleDeck();

        tableCards = new Card[5];
        tableValue = 0;

        print("GAME: Resetting players.");

        foreach (Player p in players) {
            if (p.Money <= 0) {
                p.Lost = true;
                PopUp(p.name + " is out.");
                yield return new WaitForSeconds(defaultSmallWaitTime);
            }
        }

        if (ActivePlayers() == 1) {
            EndGame();
            yield break;
        }

        ResetPlayers();

        // Blinds
        print("GAME: Paying blinds...");
        yield return new WaitForSeconds(1);
        GetNeeded(PlayerByOrder(1));
        Bet(PlayerByOrder(1), smallBlind);
        PopUp(PlayerByOrder(1).name + " paid the small blind of " + Tools.IntToMoney(smallBlind));
        Tools.UpdatePlayerUIs(players);
        yield return new WaitForSeconds(1);
        GetNeeded(PlayerByOrder(2));
        Bet(PlayerByOrder(2), bigBlind);
        PopUp(PlayerByOrder(2).name + " paid the big blind of " + Tools.IntToMoney(bigBlind));
        Tools.UpdatePlayerUIs(players);
        yield return new WaitForSeconds(1);

        scoreboard.UpdateScoreBoard();

        // Deal
        print("GAME: Dealing cards...");
        for (int i = 0; i < 2; i++) {
            for (int j = 1; j <= ActivePlayers(); j++) {
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
        PopUp("Table card 4:" + card.ToString());
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
        SettleWins();
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

        if (SkipBettingRound()) {
            canContinue = true;
            yield break;
        }

        // Betting round
        print("GAME: Betting start");
        for (int i = 1; i <= players.Length; i++) {

            // Player turn
            currentPlayer = PlayerByOrder(i);
            GetNeeded(currentPlayer);


            if (currentPlayer.Folded || currentPlayer.Lost) {
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
            yield return null;
        }

        // PayUp Round
        print("GAME: Payup start");
        for (int i = 1; i <= players.Length; i++) {

            // Player turn
            currentPlayer = PlayerByOrder(i);
            GetNeeded(currentPlayer);

            if (currentPlayer.Folded || currentPlayer.Needed == 0 || currentPlayer.Lost) {
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

    private bool SkipBettingRound() {
        if (!betting) { return false; }

        int properPlayers = 0;
        foreach (Player p in players) {
            if (!p.Lost && p.Money > 0) {
                properPlayers++;
            }
        }
        return properPlayers < 2;
    }

    private void PlayerTurn(bool payUp) {
        currentPlayer.Turn = new Turn();
        GetNeeded(currentPlayer);
        currentPlayer.Ready = false;
        currentPlayer.EnablePlayerTurn(true, payUp);
    }
    private void AnalyzeTurn() {
        Turn turn = currentPlayer.Turn;
        PopUp(turn.TurnString);

        if (turn.Fold) {
            currentPlayer.Folded = true;
            return;
        }

        Bet(currentPlayer, currentPlayer.Needed + turn.Pay);
    }

    private void SettleWins() {

        print("GAME: Results");

        currentPlayer = null;
        int winners = 0;

        Player[] sortedPlayers = new Player[ActivePlayers()];
        int index = 0;
        for (int i = 0; i < ActivePlayers(); i++) {
            if (players[i].Lost) { continue; }
            sortedPlayers[index] = players[i];
            index++;
        }
        Array.Sort(sortedPlayers);

        print("Players printed out in order of hand value");
        for (int i = 0; i < sortedPlayers.Length; i++) {
            print(sortedPlayers[i].name + ": " + sortedPlayers[i].Hand);
        }

        Hand winningHand = sortedPlayers[0].Hand;
        print("Winning hand: " + winningHand.ToString());

        foreach (Player p in sortedPlayers) {
            if (p.Hand.Equals(winningHand) && !p.Folded) {
                winners++;
                p.Winner = true;
            }
        }

        GiveWinnings(winners, sortedPlayers);

        if (winners == 1) {
            PopUp(sortedPlayers[0].name + " wins with " + Hand.HandToString(sortedPlayers[0].Hand.Value));
        } else {
            PopUp("There are " + winners  +" winners.");
        }

        scoreboard.RevealScoreBoard();
    }

    private void GiveWinnings(int winners, Player[] sortedPlayers) {
        int limit = winners;
        for (int i = 0; i < limit; i++) {

            Player current = sortedPlayers[i];

            int win = 0;
            if (current.Money <= 0) {
                win = current.Bet * ActivePlayers();
            } else {
                win = tableValue / winners;
            }
            tableValue -= win;
            current.Money += win;
            winners--;

        }
    }

    private void GivePlayersCard(Card card) {
        foreach (Player p in players) {
            p.GiveCard(card);
        }
    }

    // Other Methods
    private void SetPlayerPositions() {

        for (int i = 0; i < players.Length; i++) {

            players[i].SetPlayerPosition(table.GetPlayerPosition(i));
        }
    }

    private void ResetPlayers() {
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

        while (player == null || player.Lost) {

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

        while (player == null || player.Lost) {

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
    int ActivePlayers() {
        int playerAmount = 0;
        foreach (Player p in players) {
            if (!p.Lost) {
                playerAmount++;
            }
        }
        return playerAmount;
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

    private void Bet(Player player, int amount) {

        if (player.CanPay(amount)) {
            tableValue += amount;
            player.Bet += amount;
            player.Money -= amount;
            GetNeeded(player);
        } else {
            Bet(player, player.Money);
        }
    }

    private void PopUp(string text) {
        print("Pop Up: " + text);
        RpcPopUpAllPlayers(text);
    }
    [ClientRpc]
    private void RpcPopUpAllPlayers(string text) {
        Tools.PopUp(text);
    }

    private void EndGame() {
        StartCoroutine(EndCoroutine());
    }
    private IEnumerator EndCoroutine() {

        foreach (Player p in players) {
            if (!p.Lost) {
                PopUp("Game has ended.");
                yield return null;
                PopUp(p.name + " wins the tournament.");
                yield return null;
                PopUp("Congratulations!");
                break;
            }
        }

        yield return new WaitForSeconds(5);

        for (int i = 1; i < players.Length; i++) {
            if (players[i].GetComponent<Client>() == null) { continue; }
            players[i].GetComponent<Client>().MainMenu();
            yield return new WaitForSeconds(0.5f);
            players[i].GetComponent<NetworkIdentity>().connectionToClient.Disconnect();
            players[i].GetComponent<NetworkIdentity>().connectionToClient.Dispose();
        }

        GameObject.Find("NetworkManager").GetComponent<CustomNetworkManager>().CloseServer();
    }
}
