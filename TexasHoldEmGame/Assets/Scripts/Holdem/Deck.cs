using System.Collections.Generic;

public class Deck {

    Card[] cards;
    public Card[] Cards { get { return cards; } }

    int cardIndex;
    public int CardIndex { get { return cardIndex; } set { cardIndex = value; } }

    /// <summary>
    /// Initializes the deck with the regular 52 cards.
    /// </summary>
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

    /// <summary>
    /// Shuffles the deck.
    /// </summary>
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

    /// <summary>
    /// Returns the top card of this deck.
    /// </summary>
    /// <returns>Top card</returns>
    public Card GetCard() {
        Card card = cards[cardIndex];
        cardIndex++;
        return card;
    }
}