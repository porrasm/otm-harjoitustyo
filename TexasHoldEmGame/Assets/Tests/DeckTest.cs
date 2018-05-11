using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

public class Decktest {

    private System.Random rnd;

    [UnityTest]
    public IEnumerator DeckInitializesCorrectly() {

        Deck deck = new Deck();
        deck.InitializeDeck();

        int suit = 1;

        for (int s = 0; s < 4; s++) {
            int number = 2;
            for (int n = 2; n < 15; n++) {
                Card c = deck.GetCard();
                Assert.AreEqual(suit, c.Suit);
                Assert.AreEqual(number, c.Number);
                number++;
            }
            suit++;
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator DeckShufflesCorrectly() {

        Deck testDeck = new Deck();
        testDeck.InitializeDeck();

        Deck shuffledDeck = new Deck();
        shuffledDeck.InitializeDeck();
        shuffledDeck.ShuffleDeck();

        for (int i = 0; i < 52; i++) {
            Card current = testDeck.GetCard();
            bool found = false;
            foreach (Card c in shuffledDeck.Cards) {
                if (c.Suit == current.Suit && c.Number == current.Number) {
                    found = true;
                }
            }
            Assert.IsTrue(found);
        }

        yield return null;
    }
}