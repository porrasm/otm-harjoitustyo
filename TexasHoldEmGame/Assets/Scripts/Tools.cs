using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tools {

    // UI
    public static string IntToMoney(int amount) {
        double money = 1.0 * amount / 100;
        return DecimalPlaceNoRounding(money, 2);
    }
    public static string DecimalPlaceNoRounding(double d, int decimalPlaces = 2) {
        d = d * Math.Pow(10, decimalPlaces);
        d = Math.Truncate(d);
        d = d / Math.Pow(10, decimalPlaces);
        return string.Format("{0:N" + Math.Abs(decimalPlaces) + "}", d);
    }
    public static int MoneyToInt(string moneyString) {

        double money = Convert.ToDouble(moneyString);
        money = Math.Truncate(100 * money) / 100;
        money *= 100;

        return (int)money;
    }
    public static bool CorrectInput(string money) {
        double a = 0.0;
        bool parse = double.TryParse(money, out a);

        if (!parse) {

            money = "0.00";
            return false;
        }
        return true;
    }

    public static void PopUp(string text) {
        GameObject popUp = MonoBehaviour.Instantiate(Resources.Load("PopUp") as GameObject);
        popUp.transform.parent = GameObject.FindGameObjectWithTag("GlobalUI").transform;
        popUp.GetComponent<PopUp>().Initialize(text);
    }

    // Cards
    public static Sprite GetSuitSprite(Card card) {
        return Resources.Load<Sprite>("Cards/" + card.SuitToString().ToLower());
    }
    public static Sprite GetNumberSprite(Card card) {
        return Resources.Load<Sprite>("Cards/" + card.Number);
    }
    public static Texture GetSuitTexture(Card card) {
        return Resources.Load<Texture>("Cards/" + card.SuitToString().ToLower());
    }
    public static Texture GetNumberTexture(Card card) {
        return Resources.Load<Texture>("Cards/" + card.Number);
    }
    public static Color CardColor(Card card) {

        if (card.Suit == 2 || card.Suit == 3) {
            return Color.red;
        } else {
            return Color.black;
        }
    }

    // Players
    public static void UpdatePlayerUIs(Player[] players) {
        foreach (Player p in players) {
            p.RpcUpdateUI();
        }
    }
    public static void UpdatePlayerHands(Player[] players) {

        foreach (Player player in players) {
            if (player.Folded) { continue; }
            player.Hand = Hand.GetHighestHand(player.Cards);
        }
    }
}
