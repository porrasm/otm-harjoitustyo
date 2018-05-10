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
    }

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
            hand.text = player.Hand.HandString;
            callAmount.text = callText;
        }

        
        bet.text = "Bet: " + Tools.IntToMoney(player.Bet);
        money.text = "Money: " + Tools.IntToMoney(player.Money);      
        raiseAmount.transform.GetChild(1).GetComponent<Text>().text = "0.00";
        tableAmount.text = "Table: " + Tools.IntToMoney(game.TableValue);
    }

    public void EnableUI(bool enable) {
        callButton.interactable = enable;
        raiseButton.interactable = enable;
        foldButton.interactable = enable;
        raiseAmount.interactable = enable;
    }
    public void EnablePanel(bool enable) {
        panel.gameObject.SetActive(enable);
    }

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

    public void PayupUI() {
        callButton.interactable = true;
        raiseButton.interactable = false;
        foldButton.interactable = true;
        raiseAmount.interactable = false;
    }


    // Actions
    public void CallCheck() {
        print("Checked.");
        player.CmdCall();
    }
    public void Raise() {

        if (!Tools.CorrectInput(raiseAmount.text)) { return; }

        int amount = Tools.MoneyToInt(raiseAmount.text);
        print("Raised: " + amount);
        player.CmdRaise(amount);
    }
    public void Fold() {
        print("Folded.");
        player.CmdFold();
    }


    // Menu

    [SerializeField]
    Transform menuPanel, initPanel;

    [SerializeField]
    InputField nameField;


    public void Ready() {
        transform.root.GetComponent<Player>().CmdSetReady(true);
        transform.root.GetComponent<Player>().CmdSetName(nameField.text);
    }

    public void EnableMenu(bool enable) {
        menuPanel.gameObject.SetActive(enable);
    }
    public void DisableInit() {
        initPanel.gameObject.SetActive(false);
    }

}
