using System;

public class Card : IComparable {

    public Card(int suit, int number) {
        SetCard(suit, number);
    }
    public Card() {
    }

    int suit;
    public int Suit { get { return suit; } }
    int number;
    public int Number { get { return number; } }

    /// <summary>
    /// Initializes this card with a suit and a number.
    /// </summary>
    /// <param name="param1">Suit as an integer</param>
    /// <param name="param2">Number as an integer</param>
    public void SetCard(int suit, int number) {
        this.suit = suit;
        this.number = number;
    }

    public override string ToString() {

        string suit = SuitToString();
        string number = NumberToString();

        return number + " of " + suit + "s";
    }

    /// <summary>
    /// Converts an integer to a card suit. 1 would return "spade".
    /// </summary>
    /// <param name="param1">Suit as an integer</param>
    /// <returns>Suit as a string</returns>
    public string SuitToString() {

        string suitString;

        switch (Suit) {

            case 1:
            suitString = "Spade";
            break;
            case 2:
            suitString = "Heart";
            break;
            case 3:
            suitString = "Diamond";
            break;
            case 4:
            suitString = "Club";
            break;
            default:
            suitString = string.Empty;
            break;
        }

        return suitString;
    }

    /// <summary>
    /// Converts an integer to a card number. 2 would return "2" and 14 would return "Ace".
    /// </summary>
    /// <param name="param1">Number as an integer</param>
    /// <returns>Number as a string</returns>
    public string NumberToString() {

        string numberString;

        if (number < 11) {
            numberString = string.Empty + number;
        } else {
            switch (number) {
                case 11:
                numberString = "Jack";
                break;
                case 12:
                numberString = "Queen";
                break;
                case 13:
                numberString = "King";
                break;
                case 14:
                numberString = "Ace";
                break;
                default:
                numberString = string.Empty;
                break;
            }
        }

        return numberString;
    }

    public int CompareTo(object obj) {
        Card other = (Card)obj;
        return other.Number - number;
    }
}
