using System.Collections.Generic;

public class Deck {

    Card[] cards;
    public Card[] Cards { get { return cards; } }

    int cardIndex;
    public int CardIndex { get { return cardIndex; } set { cardIndex = value; } }

	public void InitializeDeck() {

        cards = new Card[52];
        cardIndex = 0;
        int suit = 0;

        for (int i = 0; i < 52; i++) {

            int number = (i % 13) + 2;
            if (number == 2) {
                suit++;
            }

            cards[i] = new Card();
            cards[i].SetCard(suit, number);
        }
    }

    public void ShuffleDeck() {

        List<Card> copy = new List<Card>();
        System.Random rnd = new System.Random();

        foreach (Card c in cards) {
            copy.Add(c);
        }

        for (int i = 0; i < 52; i++) {

            int randomIndex = rnd.Next(0, copy.Count);

            cards[i] = copy[randomIndex];
            copy.RemoveAt(randomIndex);
        }
    }

    public Card GetCard() {
        Card card = cards[cardIndex];
        cardIndex++;
        return card;
    }
}