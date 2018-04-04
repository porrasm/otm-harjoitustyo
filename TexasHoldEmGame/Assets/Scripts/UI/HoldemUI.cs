using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HoldemUI : MonoBehaviour {

    Player player;
    TexasHoldEm game;

    [SerializeField]
    Image cardSuit1, cardSuit2, cardNumber1, cardNumber2;

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

        //Cards
        Card card1 = player.Cards[0];
        Card card2 = player.Cards[1];

        if (cardNumber1.sprite == null && card2 != null) {

            cardSuit1.sprite = Resources.Load<Sprite>("Cards/" + card1.SuitToString().ToLower());
            cardSuit2.sprite = Resources.Load<Sprite>("Cards/" + card2.SuitToString().ToLower());

            cardNumber1.sprite = Resources.Load<Sprite>("Cards/" + card1.Number);
            cardNumber2.sprite = Resources.Load<Sprite>("Cards/" + card2.Number);

            if (card1.Suit == 2 || card1.Suit == 3) {
                cardSuit1.color = Color.red;
                cardNumber1.color = Color.red;
            } else {
                cardSuit1.color = Color.black;
                cardNumber1.color = Color.black;
            }

            if (card2.Suit == 2 || card2.Suit == 3) {
                cardSuit2.color = Color.red;
                cardNumber2.color = Color.red;
            } else {
                cardSuit2.color = Color.black;
                cardNumber2.color = Color.black;
            }
        }

        //Buttons

        if (player.Needed == 0) {
            callButton.transform.GetChild(0).GetComponent<Text>().text = "Check";
        } else if (player.Needed >= player.Money) {
            callButton.transform.GetChild(0).GetComponent<Text>().text = "All In";
        } else {
            callButton.transform.GetChild(0).GetComponent<Text>().text = "Call";
        }

        //Text fields
        string callText;
        if (player.Needed >= player.Money) {
            callText = IntToMoney(player.Money);
        } else {
            callText = IntToMoney(player.Needed);
        }

        hand.text = "High Card";
        bet.text = "Bet: " + IntToMoney(player.Bet);
        money.text = "Money: " + IntToMoney(player.Money);
        callAmount.text = callText;
        raiseAmount.transform.GetChild(1).GetComponent<Text>().text = "0.00";
        tableAmount.text = "Table: " + IntToMoney(game.TableValue);
    }

    public void EnableUI(bool enable) {
        callButton.interactable = enable;
        raiseButton.interactable = enable;
        foldButton.interactable = enable;
        raiseAmount.interactable = enable;
    }

    public void ResetUI() {

        cardSuit1.sprite = null;
        cardSuit2.sprite = null;

        cardNumber1.sprite = null;
        cardNumber2.sprite = null;

    }

    public void PayupUI() {
        callButton.interactable = true;
        raiseButton.interactable = false;
        foldButton.interactable = true;
        raiseAmount.interactable = false;
    }

    public string IntToMoney(int amount) {
        double money = 1.0 * amount / 100;
        return DecimalPlaceNoRounding(money, 2);
    }
    string DecimalPlaceNoRounding(double d, int decimalPlaces = 2) {
        d = d * Math.Pow(10, decimalPlaces);
        d = Math.Truncate(d);
        d = d / Math.Pow(10, decimalPlaces);
        return string.Format("{0:N" + Math.Abs(decimalPlaces) + "}", d);
    }
    public int MoneyToInt(string money) {

        bool dec = false;
        string hund = "";
        string cents = "";

        foreach (char c in money) {

            if (c == '.') {
                dec = true;
                continue;
            }

            if (dec) {
                cents += c;
            } else {
                hund += c;
            }

        }

        int amount = Int32.Parse(hund) * 100;
        if (!cents.Equals("")) {
            amount += Int32.Parse(cents);
        }
        return amount;
    }

    //Actions
    public void CallCheck() {
        print("Checked.");
        player.CmdCall();
    }
    public void Raise() {

        if (!RaiseCheck()) { return; }

        int amount = MoneyToInt(raiseAmount.text);
        print("Raised: " + amount);
        player.CmdRaise(amount);
    }
    public void Fold() {
        print("Folded.");
        player.CmdFold();
    }
    public bool RaiseCheck() {
        Double a = 0.0;
        bool parse = Double.TryParse(raiseAmount.text, out a);

        if (!parse) {

            raiseAmount.text = "0.00";
            return false;
        }
        return true;
    }
}
