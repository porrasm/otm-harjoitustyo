using System.Collections.Generic;
using System;

public class Hand : IComparable {

    private int value;
    public int Value { get { return value; } }
    public void SetValue(int value) {
        this.value = value;
        this.handString = Hand.HandToString(value);
        kickerValues = new int[5];
    }

    private int[] kickerValues;
    public int[] KickerValues { get { return kickerValues; } set { kickerValues = value; } }

    private string handString;
    public string HandString { get { return handString; } set { handString = value; } }

    public Hand() {
        kickerValues = new int[5];
    }

    
    public bool Tie(Hand other) {

        if (value != other.Value) {
            return false;
        }

        for (int i = 0; i < 5; i++) {
            if (kickerValues[i] != other.KickerValues[i]) {
                return false;
            }
        }

        return true;
    }

    public override string ToString() {
        return handString;
    }

    public int CompareTo(object obj) {

        Hand other = (Hand)obj;

        if (value > other.Value) {
            return -1;
        } else if (value < other.Value) {
            return 1;
        }

        for (int i = 0; i < 5; i++) {

            if (kickerValues[i] > other.KickerValues[i]) {
                return -1;
            } else if (kickerValues[i] < other.KickerValues[i]) {
                return 1;
            }
        }

        return 0;
    }


    // Static methods
    public static Hand GetHighestHand(Card[] playerCards) {

        // Check card amount
        int n = 0;
        foreach (Card c in playerCards) {
            if (c != null) { n++; }
        }

        if (n < 5) {

            Hand hand = new Hand();

            if (playerCards[0].Number == playerCards[1].Number) {
                hand.SetValue(1);
                return hand;
            } else {
                
                hand.SetValue(0);
                return hand;
            }
        }

        // Determine the best hand out of the 7 cards
        List<Hand> hands = new List<Hand>();

        // All cards but 2
        if (n == 7) {
            for (int i = 0; i < n; i++) {
                for (int j = i + 1; j < n; j++) {

                    Card[] cardCombination = new Card[5];
                    int cardIndex = 0;

                    for (int index = 0; index < n; index++) {
                        if (index != i && index != j) {
                            cardCombination[cardIndex] = playerCards[index];
                            cardIndex++;
                        }
                    }

                    hands.Add(GetHandValue(cardCombination));
                }
            } // All cards but 1
        } else if (n == 6) {
            for (int i = 0; i < n; i++) {

                Card[] cardCombination = new Card[5];
                int cardIndex = 0;

                for (int index = 0; index < n; index++) {
                    if (index != i) {
                        cardCombination[cardIndex] = playerCards[index];
                        cardIndex++;
                    }
                }

                hands.Add(GetHandValue(cardCombination));
            }
        } else {
            Card[] cards = new Card[5];
            for (int i = 0; i < 5; i++) {
                cards[i] = playerCards[i];
            }
            return GetHandValue(cards);
        }

        hands.Sort();

        return hands[0];
    }

    static Hand GetHandValue(Card[] cards) {
        Array.Sort(cards);

        int[] amounts = NumberAmounts(cards);
        int[] suits = SuitAmounts(cards);

        bool straight = Straight(cards, amounts);
        bool flush = Flush(cards, suits);

        if (RoyalFlush(cards, straight, flush)) {
            return RoyalFlushValue(cards);
        }

        if (StraightFlush(straight, flush)) {
            return StraightFlushValue(cards);
        }

        if (FourOfAKind(cards, amounts)) {
            return FourOfAKindValue(cards);
        }

        if (FullHouse(cards, amounts)) {
            return FullHouseValue(cards);
        }

        if (Flush(cards, suits)) {
            return FlushValue(cards);
        }

        if (Straight(cards, amounts)) {
            return StraightValue(cards);
        }

        if (ThreeOfAKind(cards, amounts)) {
            return ThreeOfAKindValue(cards);
        }

        if (TwoPair(cards, amounts)) {
            return TwoPairValue(cards);
        }

        if (Pair(cards, amounts)) {
            return PairValue(cards);
        }

        return HighCardValue(cards);
    }

    // Booleans
    static bool RoyalFlush(Card[] cards, bool straight, bool flush) {

        if (cards[0].Number != 14) {
            return false;
        }

        return straight && flush;
    }
    static bool StraightFlush(bool straight, bool flush) {
        return straight && flush;
    }
    static bool FourOfAKind(Card[] cards, int[] amounts) {
        foreach (int i in amounts) {
            if (i == 4) { return true; }
        }
        return false;
    }
    static bool FullHouse(Card[] cards, int[] amounts) {

        bool three = false;
        bool two = false;

        foreach (int i in amounts) {
            if (i == 2) { two = true; }
            if (i == 3) { three = true; }
        }
        return two && three;
    }
    static bool Flush(Card[] cards, int[] suits) {

        foreach (int i in suits) {
            if (i == 5) { return true; }
            if (i != 0) { return false; }
        }
        return false;
    }
    static bool Straight(Card[] cards, int[] amounts) {

        for (int i = -1; i < 9; i++) {

            int index = i;
            if (index == -1) { index = 12; }

            if (amounts[index] == 1) {
                bool straight = true;
                for (int j = 1; j <= 4; j++) {
                    if (amounts[i + j] != 1) {
                        straight = false;
                        break; }
                }
                if (straight) {
                    return true;
                }
            }
        }
        return false;
    }
    static bool ThreeOfAKind(Card[] cards, int[] amounts) {
        foreach (int i in amounts) {
            if (i >= 3) { return true; }
        }
        return false;
    }
    static bool TwoPair(Card[] cards, int[] amounts) {
        int pairs = 0;
        foreach (int i in amounts) {
            if (i >= 2) { pairs++; }
        }
        return pairs == 2;
    }
    static bool Pair(Card[] cards, int[] amounts) {
        foreach (int i in amounts) {
            if (i >= 2) { return true; }
        }
        return false;
    }

    // Return Values
    static Hand RoyalFlushValue(Card[] cards) {

        Hand hand = new Hand();
        hand.SetValue(9);

        return hand;
    }
    static Hand StraightFlushValue(Card[] cards) {

        Hand hand = new Hand();
        hand.SetValue(8);

        hand.KickerValues[0] = cards[0].Number;

        return hand;
    }
    static Hand FourOfAKindValue(Card[] cards) {

        Hand hand = new Hand();
        hand.SetValue(7);

        if (cards[0].Number == cards[1].Number) {
            hand.KickerValues[0] = cards[0].Number;
            hand.KickerValues[1] = cards[4].Number;
        } else {
            hand.KickerValues[0] = cards[4].Number;
            hand.KickerValues[1] = cards[0].Number;
        }

        return hand;
    }
    static Hand FullHouseValue(Card[] cards) {

        Hand hand = new Hand();
        hand.SetValue(6);

        if (cards[0].Number == cards[1].Number && cards[0].Number == cards[2].Number) {
            hand.KickerValues[0] = cards[0].Number;
            hand.KickerValues[1] = cards[4].Number;
        } else {
            hand.KickerValues[0] = cards[4].Number;
            hand.KickerValues[1] = cards[0].Number;
        }

        return hand;
    }
    static Hand FlushValue(Card[] cards) {

        Hand hand = new Hand();
        hand.SetValue(5);

        for (int i = 0; i < 5; i++) {
            hand.KickerValues[i] = cards[i].Number;
        }

        return hand;
    }
    static Hand StraightValue(Card[] cards) {

        Hand hand = new Hand();
        hand.SetValue(4);

        hand.KickerValues[0] = cards[0].Number;

        return hand;
    }
    static Hand ThreeOfAKindValue(Card[] cards) {

        Hand hand = new Hand();
        hand.SetValue(3);

        int index = 1;

        for (int i = 0; i < 3; i++) {

            if (i >= 3) {
                hand.KickerValues[index] = cards[i].Number;
                index++;
                break;
            }

            if (cards[i].Number == cards[i + 1].Number && cards[i].Number == cards[i + 2].Number) {
                hand.KickerValues[0] = cards[i].Number;
                i += 2;
            } else {
                hand.KickerValues[index] = cards[i].Number;
                index++;
            }
        }

        return hand;
    }
    static Hand TwoPairValue(Card[] cards) {

        Hand hand = new Hand();
        hand.SetValue(2);

        int index = 0;

        for (int i = 0; i < 4; i++) {

            if (i == 4) {
                hand.KickerValues[index] = cards[i].Number;
                break;
            }

            if (cards[i].Number == cards[i + 1].Number) {
                hand.KickerValues[index] = cards[i].Number;
                i++;
                index++;
            } else {
                hand.KickerValues[2] = cards[i].Number;
            }
        }

        return hand;
    }
    static Hand PairValue(Card[] cards) {

        Hand hand = new Hand();
        hand.SetValue(1);

        int index = 1;

        for (int i = 0; i < 5; i++) {

            if (i == 4) {
                hand.KickerValues[index] = cards[i].Number;
                break;
            }

            if (cards[i].Number == cards[i + 1].Number) {
                hand.KickerValues[0] = cards[i].Number;
                i++;
            } else {
                hand.KickerValues[index] = cards[i].Number;
            }
        }

        return hand;
    }
    static Hand HighCardValue(Card[] cards) {

        Hand hand = new Hand();
        hand.SetValue(0);

        for (int i = 0; i < 5; i++) {
            hand.KickerValues[i] = cards[i].Number;
        }

        return hand;
    }

    // Other
    static int[] SuitAmounts(Card[] cards) {

        int[] suits = new int[4];

        foreach (Card c in cards) {
            suits[c.Suit - 1]++;
        }

        return suits;
    }
    static int[] NumberAmounts(Card[] cards) {

        int[] amounts = new int[13];

        foreach (Card c in cards) {
            amounts[c.Number - 2] = amounts[c.Number - 2] + 1;
        }

        return amounts;
    }

    public static string HandToString(double value) {

        string hand = string.Empty;
        if (value < 0) {
            hand = "Nothing";
        } else if (value < 1) {
            hand = "High Card";
        } else if (value < 2) {
            hand = "One Pair";
        } else if (value < 3) {
            hand = "Two Pairs";
        } else if (value < 4) {
            hand = "Three of a Kind";
        } else if (value < 5) {
            hand = "Straight";
        } else if (value < 6) {
            hand = "Flush";
        } else if (value < 7) {
            hand = "Full House";
        } else if (value < 8) {
            hand = "Four of a Kind";
        } else if (value < 9) {
            hand = "Straight Flush";
        } else {
            hand = "Royal Flush";
        }

        return hand;
    }
}