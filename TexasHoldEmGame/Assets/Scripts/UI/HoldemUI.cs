using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HoldemUI : MonoBehaviour {

    Player player;
    TexasHoldEm game;

    [SerializeField]
    Transform cardParent, panel;

    [SerializeField]
    Button callButton, raiseButton, foldButton;

    [SerializeField]
    Text hand, bet, money, callAmount, tableAmount;

    [SerializeField]
    InputField raiseAmount;

    void Start() {
        player = transform.root.GetComponent<Player>();
        game = GameObject.FindGameObjectWithTag("Scripts").GetComponent<TexasHoldEm>();
        EnableUI(false);
        InvokeRepeating("UpdatePlayers", 1, 0.2f);
        if (!player.isServer) { startButton.gameObject.SetActive(false); }
    }

    /// <summary>
    /// Updates the UI for this player
    /// </summary>
    public void UpdateUI() {

        // Cards

        Card[] cards = player.Cards;

        for (int i = 0; i < cards.Length; i++) {

            Card card = cards[i];
            if (card == null) { continue; }
            Transform cardObject = cardParent.GetChild(i);

            Image cardSuit = cardObject.GetChild(0).GetComponent<Image>();
            if (cardSuit.sprite != null) { continue; }
            Image cardNumber = cardObject.GetChild(1).GetComponent<Image>();

            Color cardColor = Tools.CardColor(card);

            cardSuit.sprite = Tools.GetSuitSprite(card);
            cardSuit.color = cardColor;

            cardNumber.sprite = Tools.GetNumberSprite(card);
            cardNumber.color = cardColor;
        }

        // Buttons

        if (player.Needed == 0) {
            callButton.transform.GetChild(0).GetComponent<Text>().text = "Check";
        } else if (player.Needed >= player.Money) {
            callButton.transform.GetChild(0).GetComponent<Text>().text = "All In";
        } else {
            callButton.transform.GetChild(0).GetComponent<Text>().text = "Call";
        }

        // Text fields

        string callText;
        if (player.Needed >= player.Money) {
            callText = Tools.IntToMoney(player.Money);
        } else {
            callText = Tools.IntToMoney(player.Needed);
        }

        if (player.Folded) {
            hand.text = "Folded";
            callAmount.text = "0.00";
        } else {
            hand.text = Hand.HandToString(player.Hand.Value);
            callAmount.text = callText;
        }

        
        bet.text = "Bet: " + Tools.IntToMoney(player.Bet);
        money.text = "Money: " + Tools.IntToMoney(player.Money);      
        raiseAmount.transform.GetChild(1).GetComponent<Text>().text = "0.00";
        tableAmount.text = "Table: " + Tools.IntToMoney(game.TableValue);
    }

    /// <summary>
    /// Enables the UI for this player.
    /// </summary>
    /// <param name="param1">Enable</param>
    public void EnableUI(bool enable) {
        callButton.interactable = enable;
        raiseButton.interactable = enable;
        foldButton.interactable = enable;
        raiseAmount.interactable = enable;
    }

    /// <summary>
    /// Enables the game panel UI for this player.
    /// </summary>
    /// <param name="param1">Enable</param>
    public void EnablePanel(bool enable) {
        panel.gameObject.SetActive(enable);
    }

    /// <summary>
    /// Resets the UI for this player.
    /// </summary>
    public void ResetUI() {

        for (int i = 0; i < cardParent.childCount; i++) {

            Transform card = cardParent.GetChild(i);

            Image cardSuit = card.GetChild(0).GetComponent<Image>();
            Image cardNumber = card.GetChild(1).GetComponent<Image>();

            cardSuit.sprite = null;
            cardSuit.color = new Color(0, 0, 0, 0);

            cardNumber.sprite = null;
            cardNumber.color = new Color(0, 0, 0, 0);
        }

        hand.text = "Nothing";
    }

    /// <summary>
    /// Enables the UI which is used in a payup situation for this player.
    /// </summary>
    public void PayupUI() {
        callButton.interactable = true;
        raiseButton.interactable = false;
        foldButton.interactable = true;
        raiseAmount.interactable = false;
    }


    // Actions

    /// <summary>
    /// Calls the CmdCall method on the player.
    /// </summary>
    public void CallCheck() {
        player.CmdCall();
        
    }

    /// <summary>
    /// Calls the CmdRaise method on the player.
    /// </summary>
    public void Raise() {

        if (!Tools.CorrectInput(raiseAmount.text)) { return; }

        int amount = Tools.MoneyToInt(raiseAmount.text);
        if (amount > player.Money ) {
            amount = player.Money;
        }
        player.CmdRaise(amount);
    }

    /// <summary>
    /// Calls the CmdFold method on the player.
    /// </summary>
    public void Fold() {
        player.CmdFold();
    }


    // Menu

    [SerializeField]
    Transform menuPanel, initPanel, playerListParent;

    [SerializeField]
    InputField nameField;

    [SerializeField]
    private Button readyButton, startButton;

    /// <summary>
    /// Updates the name of this player for all clients. Must be called on the client.
    /// </summary>
    public void OnNameChange() {
        ColorBlock newBlock = readyButton.colors;
        newBlock.normalColor = Color.red;
        readyButton.colors = newBlock;
        player.CmdSetReady(false);
        transform.root.GetComponent<Player>().CmdSetName(nameField.text);
    }

    /// <summary>
    /// Sets the player ready.
    /// </summary>
    public void Ready() {
        ColorBlock newBlock = readyButton.colors;
        newBlock.normalColor = Color.green;
        readyButton.colors = newBlock;
        transform.root.GetComponent<Player>().CmdSetReady(true);
        transform.root.GetComponent<Player>().CmdSetName(nameField.text);
    }

    /// <summary>
    ///Enables or disables the menu for this player.
    /// </summary>
    /// <param name="enable">Enable</param>
    public void EnableMenu(bool enable) {
        menuPanel.gameObject.SetActive(enable);
    }

    /// <summary>
    /// Disables the 'lobby menu' for this player.
    /// </summary>
    public void DisableInit() {
        initPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// Starts the game. Must be called on the server / host.
    /// </summary>
    public void StartGame() {
        game.CanContinue = true;
        CancelInvoke();
        GameObject.Find("NetworkManager").GetComponent<CustomNetworkManager>().MakePrivate();
    }

    void UpdatePlayers() {

        if (game.Players == null) {
            CancelInvoke();
        }

        for (int i = 0; i < 10; i++) {
            playerListParent.GetChild(i).gameObject.SetActive(false);
        }

        bool allReady = true;

        for (int i = 0; i < game.Players.Length; i++) {
            playerListParent.GetChild(i).gameObject.SetActive(true);
            playerListParent.GetChild(i).GetChild(0).GetComponent<Text>().text = game.Players[i].name;
            if (game.Players[i].Ready) {
                playerListParent.GetChild(i).GetComponent<Image>().color = Color.green;
            } else {
                allReady = false;
                playerListParent.GetChild(i).GetComponent<Image>().color = Color.red;
            }
        }

        if (player.isServer) {
            if (allReady && game.Players.Length > 1) {
                startButton.interactable = true;
            } else {
                startButton.interactable = false;
            }
        }

    }
}
