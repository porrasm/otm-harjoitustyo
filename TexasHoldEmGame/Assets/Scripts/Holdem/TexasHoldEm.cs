using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TexasHoldEm : NetworkBehaviour {

    Table table;
    Deck deck;
    Card[] tableCards;
    [SyncVar]
    int tableValue, biggestBet, dealer;
    public int TableValue { get { return tableValue; } }
    Player[] players;
    public Player[] Players { get { return players; } }

    Player currentPlayer;

    bool roundIsOn;
    public bool RoundIsOn { get { return roundIsOn; } }

    GameObject AIPrefab;

    int buyIn;
    public int BuyIn { get { return buyIn; } }

    ScoreboardUI scoreboard;

    bool gameIsReady;
    public bool GameIsReady { get { return gameIsReady; } }

    bool canContinue;
    int roundAmount;

    string gameState;

    //Setting

    [SerializeField]
    bool betting;

    [SerializeField]
    bool fillWithAI;

    [SerializeField]
    private int smallBlind = 10, bigBlind = 20;



    void Start() {

        if (!isServer) { return; }
        AIPrefab = Resources.Load<GameObject>("AIPlayer");
        UpdatePlayers();
        deck = new Deck();
        dealer = 0;
        table = GameObject.FindGameObjectWithTag("Table").GetComponent<Table>();
        scoreboard = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<ScoreboardUI>();

    }

    void Update() {

        if (!isServer) { return; }

        if (Input.GetKeyDown(KeyCode.P)) {
            PlaceHolderStart();
        }

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

    //Game functionality
    public void Bet() {

    }
    [Command]
    public void CmdBet() {

    }


    //Game
    public void PlaceHolderStart() {
        UpdatePlayers();
        StartCoroutine(PlaceholderStartCoroutine());
    }
    IEnumerator PlaceholderStartCoroutine() {

        gameState = "Game starting...";
        buyIn = 2000;

        while (true) {

            bool allReady = true;
            foreach (Player player in players) {
                if (!player.Ready) {
                    allReady = false;
                }
            }

            if (allReady) {

                UpdatePlayers();

                if (fillWithAI) {
                    for (int i = 0; i < 10 - players.Length; i++) {
                        GameObject newAI = Instantiate(AIPrefab);
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

        gameIsReady = true;
        roundAmount = 30;
        roundIsOn = false;

    }

   public void StartHoldemRound() {
        roundIsOn = true;
        StartCoroutine(StartHoldemRoundCoroutine());
    }
    IEnumerator StartHoldemRoundCoroutine() {

        if (roundIsOn) {
            yield break;
        }

        roundIsOn = true;
        print("GAME: Starting round.");

        //Game rules
        float defaultSmallWaitTime = 0.1f;
        smallBlind = 10;
        bigBlind = 20;
        int buyIn = 2000;

        Card card;

        //Round Initialization
        gameState = "Preflop";

        deck.InitializeDeck();
        deck.ShuffleDeck();
        deck.ShuffleDeck();
        deck.ShuffleDeck();

        tableCards = new Card[5];
        tableValue = 0;

        print("GAME: Resetting players.");
        ResetPlayers();

        //Blinds
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

        //Deal
        print("GAME: Dealing cards...");
        for (int i = 0; i < 2; i++) {
            for (int j = 1; j <= players.Length; j++) {
                Player p = PlayerByOrder(j);
                card = deck.GetCard();

                p.GiveCard(card);
                Transform cardPos = table.GetPlayerPosition(indexByOrder(j)).GetChild(i);
                table.SpawnCard(p.gameObject, card, cardPos);
                yield return new WaitForSeconds(defaultSmallWaitTime);
            }
        }

        Tools.UpdatePlayerHands(players);
        yield return new WaitForSeconds(0.5f);
        Tools.UpdatePlayerUIs(players);
        yield return new WaitForSeconds(1);

        //Betting round 1
        print("GAME: Betting round 1 starts.");
        canContinue = false;
        StartCoroutine(BetRound());

        while (!canContinue) {
            yield return new WaitForSeconds(1);
        }

        //Flop, first 3 cards
        print("GAME: The flop, first 3 cards");
        for (int i = 0; i < 3; i++) {
            deck.GetCard();
            card = deck.GetCard();
            GivePlayersCard(card);
            table.SpawnCard(gameObject, card, table.GetCardPosition(i));
            Tools.UpdatePlayerUIs(players);
            yield return new WaitForSeconds(1);
        }

        Tools.UpdatePlayerHands(players);
        yield return new WaitForSeconds(0.5f);
        Tools.UpdatePlayerUIs(players);
        yield return new WaitForSeconds(1);

        //Betting round 2
        print("GAME: Betting round 2 starts.");
        canContinue = false;
        StartCoroutine(BetRound());

        while (!canContinue) {
            yield return new WaitForSeconds(1);
        }

        //River, 4 cards
        print("GAME: River, first 4 cards.");
        deck.GetCard();
        card = deck.GetCard();
        GivePlayersCard(card);
        table.SpawnCard(gameObject, card, table.GetCardPosition(3));
        Tools.UpdatePlayerHands(players);
        yield return new WaitForSeconds(0.5f);
        Tools.UpdatePlayerUIs(players);
        yield return new WaitForSeconds(2);

        //Betting round 3
        print("GAME: Betting round 3 starts.");
        canContinue = false;
        StartCoroutine(BetRound());

        while (!canContinue) {
            yield return new WaitForSeconds(1);
        }

        //Turn, 5 cards
        print("GAME: Turn, all 5 cards.");
        deck.GetCard();
        card = deck.GetCard();
        GivePlayersCard(card);
        table.SpawnCard(gameObject, card, table.GetCardPosition(4));
        Tools.UpdatePlayerHands(players);
        yield return new WaitForSeconds(0.5f);
        Tools.UpdatePlayerUIs(players);
        yield return new WaitForSeconds(2);

        //Betting round 4
        print("GAME: Betting round 4 starts.");
        canContinue = false;
        StartCoroutine(BetRound());

        while (!canContinue) {
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(2);

        Tools.UpdatePlayerUIs(players);

        //Reveal cards
        print("GAME: Revealing cards.");
        foreach (GameObject c in GameObject.FindGameObjectsWithTag("Card")) {

            CardObject cardObject = c.GetComponent<CardObject>();

            if (cardObject.Owner == gameObject) { continue; }
            if (cardObject.Owner.GetComponent<Player>().Folded) { continue; }
            cardObject.TurnCard();
            yield return new WaitForSeconds(defaultSmallWaitTime);

        }

        scoreboard.RevealScoreBoard();

        //Winning and money
        Winner();
        yield return new WaitForSeconds(5);
        //End round
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
 
    //Betting
    IEnumerator BetRound() {

        if (!betting) {
            canContinue = true;
        }

        //Betting round
        print("GAME: Betting start");
        for (int i = 1; i <= players.Length; i++) {
            
            //Player turn
            currentPlayer = PlayerByOrder(i);
            GetNeeded(currentPlayer);

            if (currentPlayer.Folded) {
                continue;
            }

            Tools.UpdatePlayerUIs(players);
            scoreboard.UpdateScoreBoard();
            yield return new WaitForSeconds(0.5f);

            PlayerTurn(false);

            while (!currentPlayer.Ready) {
                yield return null;
            }
            currentPlayer.EnablePlayerTurn(false);

            //Analyzing turn
            AnalyzeTurn();

        }

        //PayUp Round
        print("GAME: Payup start");
        for (int i = 1; i <= players.Length; i++) {

            //Player turn
            currentPlayer = PlayerByOrder(i);
            GetNeeded(currentPlayer);

            if (currentPlayer.Folded || currentPlayer.Needed == 0) {
                continue;
            }

            Tools.UpdatePlayerUIs(players);
            scoreboard.UpdateScoreBoard();
            yield return new WaitForSeconds(0.5f);

            PlayerTurn(true);

            while (!currentPlayer.Ready) {
                yield return null;
            }
            currentPlayer.EnablePlayerTurn(false);

            //Analyzing turn
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

        if (turn.fold) {
            currentPlayer.Folded = true;
            return;
        }

        Bet(currentPlayer, currentPlayer.Needed + turn.raise);
    }

    public void Winner() {
        bool tie = false;
        print("GAME: Results");

        currentPlayer = null;

        foreach (Player p in players) {

            if (p.Folded) {
                continue;
            }

            if (currentPlayer == null) {
                currentPlayer = p;
            }


            if (p.Hand == currentPlayer.Hand) {
                tie = true;
            }

            if (p.Hand > currentPlayer.Hand) {
                currentPlayer = p;
            }
        }

        if (currentPlayer == null) {
            print("GAME: NO WINNER");
            return;
        }

        currentPlayer.Winner = true;
        currentPlayer.Money += tableValue;
        
        print("GAME: Winner is: " + currentPlayer.name + " with " + Hand.HandToString(currentPlayer.Hand));
    }

    void GivePlayersCard(Card card) {
        foreach (Player p in players) {
            p.GiveCard(card);
        }
    }

    //Other Methods
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
            p.Hand = -1;
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

    
    //Toolds
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

    void ForceScoreBoard(Player[] players, bool force) {
        foreach (Player p in players) {

        }
    }

}
