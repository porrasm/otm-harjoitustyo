using System.Collections.Generic;
using System;

public class Hand {

    public static double GetHighestHand(Card[] playerCards) {

        //Check card amount
        int n = 0;
        foreach (Card c in playerCards) {
            if (c != null) { n++; }
        }

        if (n < 5) {
            if (playerCards[0].Number == playerCards[1].Number) {
                return 1;
            } else {
                return 0;
            }
        }

        //Determine the best hand out of the 7 cards
        List<Double> values = new List<double>();

        //All cards but 2
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

                    values.Add(GetHandValue(cardCombination));

                }
            }
            //All cards but 1
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

                values.Add(GetHandValue(cardCombination));

            }
        } else {
            Card[] cards = new Card[5];
            for (int i = 0; i < 5; i++) {
                cards[i] = playerCards[i];
            }
            return GetHandValue(cards);
        }


        double biggest = values[0];
        for (int i = 1; i < values.Count; i++) {
            if (values[i] > biggest) {
                biggest = values[i];
            }
        }

        return biggest;
    }

    static double GetHandValue(Card[] cards) {
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

    //Booleans
    static bool RoyalFlush(Card[] cards, bool straight, bool flush) {

        if (cards[0].Number != 14) {
            return false;
        }

        return straight && flush;
    }
    static bool StraightFlush(Boolean straight, bool flush) {
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
            if (i == 2) { two = true; ; }
            if (i == 3) { three = true; ; }
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

    //Return Values
    static double RoyalFlushValue(Card[] cards) {
        return 9;
    }
    static double StraightFlushValue(Card[] cards) {
        return 8;
    }
    static double FourOfAKindValue(Card[] cards) {
        return 7;
    }
    static double FullHouseValue(Card[] cards) {
        return 6;
    }
    static double FlushValue(Card[] cards) {
        return 5;
    }
    static double StraightValue(Card[] cards) {
        return 4;
    }
    static double ThreeOfAKindValue(Card[] cards) {
        return 3;
    }
    static double TwoPairValue(Card[] cards) {
        return 2;
    }
    static double PairValue(Card[] cards) {
        return 1;
    }
    static double HighCardValue(Card[] cards) {
        return 0;
    }

    //Other
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

        string hand = "";
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