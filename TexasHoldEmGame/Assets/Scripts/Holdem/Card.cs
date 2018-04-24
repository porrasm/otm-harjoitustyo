using System;

public class Card : IComparable {

    int suit;
    public int Suit { get { return suit; } }
    int number;
    public int Number { get { return number; } }

    public void SetCard(int suit, int number) {
        this.suit = suit;
        this.number = number;
    }

    public override string ToString() {

        string suit = SuitToString();
        string number = NumberToString();

        return number + " of " + suit + "s";
    }

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
