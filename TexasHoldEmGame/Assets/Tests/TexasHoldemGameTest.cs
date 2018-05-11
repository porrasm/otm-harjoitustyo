using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

public class TexasHoldemGameTest {

    private System.Random rnd;

    /*
    [UnityTest]
    public IEnumerator GameProgressesCorrectly() {
        // Scene setup
        SceneManager.LoadScene("default", LoadSceneMode.Single);

        yield return new WaitForSeconds(1);

        CustomNetworkManager networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<CustomNetworkManager>();
        networkManager.StartHost();
        yield return new WaitForSeconds(1);

        Table table = GameObject.FindGameObjectWithTag("Table").GetComponent<Table>();
        TexasHoldEm game = GameObject.FindGameObjectWithTag("Scripts").GetComponent<TexasHoldEm>();

        Assert.IsNotNull(game);
        Assert.IsNotNull(table);

        yield return new WaitForSeconds(1);

        game.StartGame();

        Assert.IsTrue(game.Players.Length == 1);
        Assert.IsFalse(game.GameIsReady);

        game.Players[0].Ready = true;

        yield return new WaitForSeconds(2);

        Assert.IsTrue(game.RoundIsOn);

        // AI 
        Assert.IsTrue(game.Players.Length == 10);

        foreach (Player p in game.Players) {

            Assert.IsTrue(p.Money == game.BuyIn);
        }
    }
    */
    [UnityTest]
    public IEnumerator CardHandValuesAreCorrectWith2Cards() {

        for (int i = 0; i < 100; i++) {

            Card[] cards = new Card[2];

            int numberA = (int)Random.value * 13 + 1;
            int numberB = (int)Random.value * 13 + 1;

            Card a = new Card();
            a.SetCard(1, numberA);

            Card b = new Card();
            b.SetCard(1, numberB);

            cards[0] = a;
            cards[1] = b;

            int big = 0;
            int small = 0;
            int expectedValue = 0;

            if (numberA > numberB) {
                big = numberA;
                small = numberB;
            } else if (numberA < numberB) {
                small = numberA;
                big = numberB;
            } else if (numberA == numberB) {
                expectedValue = 1;
            }

            Hand highest = Hand.GetHighestHand(cards);

            Assert.AreEqual(expectedValue, highest.Value);
            Assert.AreEqual(big, highest.KickerValues[0]);
            Assert.AreEqual(small, highest.KickerValues[1]);

        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator RoyalFlush() {

        rnd = new System.Random();

        for (int i = 1; i < 5; i++) {
            Card[] cards = new Card[5];
            cards[0] = new Card(1, 14);
            cards[1] = new Card(1, 13);
            cards[2] = new Card(1, 12);
            cards[3] = new Card(1, 11);
            cards[4] = new Card(1, 10);

            Hand bestHand = Hand.GetHighestHand(cards);
            Assert.AreEqual(9, bestHand.Value);
        }

        for (int i = 1; i < 5; i++) {
            Card[] cards = new Card[7];
            cards[0] = new Card(1, 14);
            cards[1] = new Card(1, 13);
            cards[2] = new Card(1, 12);
            cards[3] = new Card(1, 11);
            cards[4] = new Card(1, 10);
            cards[5] = new Card(rnd.Next(1, 4), rnd.Next(2, 14));
            cards[6] = new Card(rnd.Next(1, 4), rnd.Next(2, 14));

            Hand bestHand = Hand.GetHighestHand(cards);
            Assert.AreEqual(9, bestHand.Value);
        }
        yield return null;
    }

    [UnityTest]
    public IEnumerator StraightFlush() {

        rnd = new System.Random();

        for (int i = 1; i < 5; i++) {
            Card[] cards = new Card[5];
            cards[0] = new Card(1, 9);
            cards[1] = new Card(1, 13);
            cards[2] = new Card(1, 12);
            cards[3] = new Card(1, 11);
            cards[4] = new Card(1, 10);

            Hand bestHand = Hand.GetHighestHand(cards);
            Assert.AreEqual(8, bestHand.Value);
        }

        for (int i = 1; i < 5; i++) {
            Card[] cards = new Card[7];
            cards[0] = new Card(1, 9);
            cards[1] = new Card(1, 13);
            cards[2] = new Card(1, 12);
            cards[3] = new Card(1, 11);
            cards[4] = new Card(1, 10);
            cards[5] = new Card(rnd.Next(2, 4), rnd.Next(2, 13));
            cards[6] = new Card(rnd.Next(2, 4), rnd.Next(2, 13));

            Hand bestHand = Hand.GetHighestHand(cards);
            Assert.AreEqual(8, bestHand.Value);
        }
        yield return null;
    }

    [UnityTest]
    public IEnumerator FourOfAKind() {

        rnd = new System.Random();

        for (int i = 0; i < 100; i++) {

            int number = rnd.Next(2, 14);

            Card[] cards = new Card[5];
            cards[0] = new Card(1, number);
            cards[1] = new Card(1, 5);
            cards[2] = new Card(1, number);
            cards[3] = new Card(1, number);
            cards[4] = new Card(1, number);

            Hand bestHand = Hand.GetHighestHand(cards);

            Assert.AreEqual(7, bestHand.Value);
            Assert.AreEqual(number, bestHand.KickerValues[0]);
            Assert.AreEqual(5, bestHand.KickerValues[1]);
        }

        for (int i = 0; i < 100; i++) {

            int number = rnd.Next(2, 14);

            Card[] cards = new Card[7];
            cards[0] = new Card(1, number);
            cards[1] = new Card(1, 5);
            cards[2] = new Card(1, number);
            cards[3] = new Card(1, number);
            cards[4] = new Card(1, number);
            cards[5] = new Card(1, 3);
            cards[6] = new Card(1, 4);

            Hand bestHand = Hand.GetHighestHand(cards);
            Assert.AreEqual(7, bestHand.Value);
            Assert.AreEqual(number, bestHand.KickerValues[0]);
            Assert.AreEqual(5, bestHand.KickerValues[1]);
        }



        yield return null;
    }

    [UnityTest]
    public IEnumerator FullHouse() {

        rnd = new System.Random();

        for (int i = 1; i < 5; i++) {

            int number3 = rnd.Next(2, 14);
            int number2 = rnd.Next(2, 14);
            while (number2 == number3) {
                number2 = rnd.Next(2, 14);
            }

            Card[] cards = new Card[5];
            cards[0] = new Card(1, number3);
            cards[1] = new Card(1, number3);
            cards[2] = new Card(1, number3);
            cards[3] = new Card(1, number2);
            cards[4] = new Card(1, number2);

            Hand bestHand = Hand.GetHighestHand(cards);
            Assert.AreEqual(6, bestHand.Value);
            Assert.AreEqual(number3, bestHand.KickerValues[0]);
            Assert.AreEqual(number2, bestHand.KickerValues[1]);
        }

        for (int i = 1; i < 7; i++) {

            int number3 = rnd.Next(2, 14);
            int number2 = rnd.Next(2, 14);
            while (number2 == number3) {
                number2 = rnd.Next(2, 14);
            }

            Card[] cards = new Card[7];
            cards[0] = new Card(1, number3);
            cards[1] = new Card(1, number3);
            cards[2] = new Card(1, number3);
            cards[3] = new Card(1, number2);
            cards[4] = new Card(1, number2);
            cards[5] = new Card(1, number2);

            int random = rnd.Next(2, 14);
            while (random == number2 || random == number3) {
                random = rnd.Next(2, 14);
            }

            cards[6] = new Card(1, random);

            if (number3 >= number2) {
                Hand bestHand = Hand.GetHighestHand(cards);
                Assert.AreEqual(6, bestHand.Value);
                Assert.AreEqual(number3, bestHand.KickerValues[0]);
                Assert.AreEqual(number2, bestHand.KickerValues[1]);
            } else {
                Hand bestHand = Hand.GetHighestHand(cards);
                Assert.AreEqual(6, bestHand.Value);
                Assert.AreEqual(number2, bestHand.KickerValues[0]);
                Assert.AreEqual(number3, bestHand.KickerValues[1]);
            }


        }
        yield return null;
    }

    [UnityTest]
    public IEnumerator Flush() {

        rnd = new System.Random();

        for (int i = 1; i < 5; i++) {
            Card[] cards = new Card[5];
            cards[0] = new Card(1, 2);
            cards[1] = new Card(1, 4);
            cards[2] = new Card(1, 6);
            cards[3] = new Card(1, 8);
            cards[4] = new Card(1, 10);

            Hand bestHand = Hand.GetHighestHand(cards);
            Assert.AreEqual(5, bestHand.Value);

            int j = 10;
            int index = 0;
            while (j >= 2) {

                Assert.AreEqual(j, bestHand.KickerValues[index]);
                index++;
                j -= 2;
            }

        }

        for (int i = 1; i < 7; i++) {
            Card[] cards = new Card[7];
            cards[0] = new Card(1, 2);
            cards[1] = new Card(1, 4);
            cards[2] = new Card(1, 6);
            cards[3] = new Card(1, 8);
            cards[4] = new Card(1, 10);
            cards[5] = new Card(2, 12);
            cards[6] = new Card(3, 14);

            Hand bestHand = Hand.GetHighestHand(cards);
            Assert.AreEqual(5, bestHand.Value);

            int j = 14;
            int index = 0;
            while (j >= 6) {

                Assert.AreEqual(j, bestHand.KickerValues[index]);
                index++;
                j -= 2;
            }

        }
        yield return null;
    }

}