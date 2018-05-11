using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tools {

    // UI

    /// <summary>
    /// Converts an integer to 'money'. 123 would return 1.23.
    /// </summary>
    /// <param name="param1">Amount of cents.</param>
    /// <returns>The amount as a string</returns>
    public static string IntToMoney(int amount) {
        double money = 1.0 * amount / 100;
        return DecimalPlaceNoRounding(money, 2);
    }

    private static string DecimalPlaceNoRounding(double d, int decimalPlaces = 2) {
        d = d * Math.Pow(10, decimalPlaces);
        d = Math.Truncate(d);
        d = d / Math.Pow(10, decimalPlaces);
        return string.Format("{0:N" + Math.Abs(decimalPlaces) + "}", d);
    }

    /// <summary>
    /// Converts 'money' to an integer. 1.23 would return 123.
    /// </summary>
    /// <param name="param1">Amount of money</param>
    /// <returns>The amount as an integer</returns>
    public static int MoneyToInt(string moneyString) {

        double money = Convert.ToDouble(moneyString);
        money = Math.Truncate(100 * money) / 100;
        money *= 100;

        return (int)money;
    }

    /// <summary>
    /// Checks whether a string is in a correct format to be converted into 'money'.
    /// </summary>
    /// <param name="param1">The amount as a string.</param>
    /// <returns>'true' if the string is in a correct format. </returns>
    public static bool CorrectInput(string money) {
        double a = 0.0;
        bool parse = double.TryParse(money, out a);

        if (money.Contains("-")) {
            return false;
        }

        if (!parse) {

            money = "0.00";
            return false;
        }

        return true;
    }

    /// <summary>
    /// Creates a pop up message for this game instance.
    /// </summary>
    /// <param name="param1">Text</param>
    public static void PopUp(string text, int index) {
        GameObject popUp = MonoBehaviour.Instantiate(Resources.Load("PopUp") as GameObject);
        popUp.transform.SetParent(GameObject.FindGameObjectWithTag("GlobalUI").transform);
        popUp.GetComponent<PopUp>().Initialize(text);
        popUp.GetComponent<PopUp>().index = index;
    }

    // Cards

    /// <summary>
    /// Returns the correct suit sprite for this card.
    /// </summary>
    /// <param name="param1">Card</param>
    /// <returns>Correct suit sprite</returns>
    public static Sprite GetSuitSprite(Card card) {
        return Resources.Load<Sprite>("Cards/" + card.SuitToString().ToLower());
    }

    /// <summary>
    /// Returns the correct number sprite for this card.
    /// </summary>
    /// <param name="param1">Card</param>
    /// <returns>Correct number sprite</returns>
    public static Sprite GetNumberSprite(Card card) {
        return Resources.Load<Sprite>("Cards/" + card.Number);
    }

    /// <summary>
    /// Returns the correct suit texture for this card.
    /// </summary>
    /// <param name="param1">Card</param>
    /// <returns>Correct suit texture</returns>
    public static Texture GetSuitTexture(Card card) {
        return Resources.Load<Texture>("Cards/" + card.SuitToString().ToLower());
    }

    /// <summary>
    /// Returns the correct number texture for this card.
    /// </summary>
    /// <param name="param1">Card</param>
    /// <returns>Correct number texture</returns>
    public static Texture GetNumberTexture(Card card) {
        return Resources.Load<Texture>("Cards/" + card.Number);
    }

    /// <summary>
    /// Returns the correct color for this card.
    /// </summary>
    /// <param name="param1">Card</param>
    /// <returns>Correct color</returns>
    public static Color CardColor(Card card) {

        if (card.Suit == 2 || card.Suit == 3) {
            return Color.red;
        } else {
            return Color.black;
        }
    }

    // Players

    /// <summary>
    /// Updates the UI of every player. Must be called on the server.
    /// </summary>
    /// <param name="param1">List of players</param>
    public static void UpdatePlayerUIs(Player[] players) {
        foreach (Player p in players) {
            p.UpdateHand();
            p.RpcUpdateUI();
        }
    }

    /// <summary>
    /// Updates and selects the best hand for every player. Must be called on the server.
    /// </summary>
    /// <param name="param1">List of players</param>
    public static void UpdatePlayerHands(Player[] players) {

        foreach (Player player in players) {
            if (player.Folded) { continue; }
            player.Hand = Hand.GetHighestHand(player.Cards);
        }
    }
}
