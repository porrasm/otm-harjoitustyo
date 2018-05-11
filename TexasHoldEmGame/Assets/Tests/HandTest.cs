using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

public class HandTest {

    private System.Random rnd;

    [UnityTest]
    public IEnumerator A_CardHandValuesAreCorrectWith2Cards() {

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
    public IEnumerator B_RoyalFlush() {

        rnd = new System.Random();

        for (int i = 1; i < 5; i++) {
            Card[] cards = new Card[5];
            cards[0] = new Card(i, 14);
            cards[1] = new Card(i, 13);
            cards[2] = new Card(i, 12);
            cards[3] = new Card(i, 11);
            cards[4] = new Card(i, 10);

            Hand bestHand = Hand.GetHighestHand(cards);
            Assert.AreEqual(9, bestHand.Value);
        }

        for (int i = 1; i < 5; i++) {
            Card[] cards = new Card[7];
            cards[0] = new Card(i, 14);
            cards[1] = new Card(i, 13);
            cards[2] = new Card(i, 12);
            cards[3] = new Card(i, 11);
            cards[4] = new Card(i, 10);
            cards[5] = new Card(rnd.Next(1, 4), rnd.Next(2, 14));
            cards[6] = new Card(rnd.Next(1, 4), rnd.Next(2, 14));

            Hand bestHand = Hand.GetHighestHand(cards);
            Assert.AreEqual(9, bestHand.Value);
        }
        yield return null;
    }

    [UnityTest]
    public IEnumerator C_StraightFlush() {

        rnd = new System.Random();

        for (int i = 1; i < 5; i++) {
            Card[] cards = new Card[5];
            cards[0] = new Card(i, 9);
            cards[1] = new Card(i, 13);
            cards[2] = new Card(i, 12);
            cards[3] = new Card(i, 11);
            cards[4] = new Card(i, 10);

            Hand bestHand = Hand.GetHighestHand(cards);
            Assert.AreEqual(8, bestHand.Value);
        }

        for (int i = 1; i < 5; i++) {
            Card[] cards = new Card[7];
            cards[0] = new Card(i, 9);
            cards[1] = new Card(i, 13);
            cards[2] = new Card(i, 12);
            cards[3] = new Card(i, 11);
            cards[4] = new Card(i, 10);
            cards[5] = new Card(rnd.Next(2, 4), rnd.Next(2, 13));
            cards[6] = new Card(rnd.Next(2, 4), rnd.Next(2, 13));

            Hand bestHand = Hand.GetHighestHand(cards);
            Assert.AreEqual(8, bestHand.Value);
        }
        yield return null;
    }

    [UnityTest]
    public IEnumerator D_FourOfAKind() {

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
    public IEnumerator E_FullHouse() {

        rnd = new System.Random();

        for (int i = 0; i < 100; i++) {

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

        for (int i = 0; i < 100; i++) {

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
    public IEnumerator F_Flush() {

        rnd = new System.Random();

        for (int i = 0; i < 100; i++) {
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
            cards[5] = new Card(1, 12);
            cards[6] = new Card(2, 14);

            Hand bestHand = Hand.GetHighestHand(cards);
            Assert.AreEqual(5, bestHand.Value);

            int j = 12;
            int index = 0;
            while (j >= 4) {

                Assert.AreEqual(j, bestHand.KickerValues[index]);
                index++;
                j -= 2;
            }
        }
        yield return null;
    }

    [UnityTest]
    public IEnumerator G_Straight() {

        rnd = new System.Random();

        for (int i = 0; i < 100; i++) {

            int number = rnd.Next(2, 10);

            Card[] cards = new Card[5];
            cards[0] = new Card(1, number);
            cards[1] = new Card(2, number + 1);
            cards[2] = new Card(3, number + 2);
            cards[3] = new Card(4, number + 3);
            cards[4] = new Card(1, number + 4);

            Hand bestHand = Hand.GetHighestHand(cards);

            Assert.AreEqual(4, bestHand.Value);
            Assert.AreEqual(number + 4, bestHand.KickerValues[0]);
        }

        for (int i = 0; i < 100; i++) {

            int number = rnd.Next(4, 10);

            Card[] cards = new Card[7];
            cards[0] = new Card(1, number);
            cards[1] = new Card(2, number + 1);
            cards[2] = new Card(3, number + 2);
            cards[3] = new Card(4, number + 3);
            cards[4] = new Card(1, number + 4);
            cards[5] = new Card(1, number - 1);
            cards[6] = new Card(1, number - 2);

            Hand bestHand = Hand.GetHighestHand(cards);

            Assert.AreEqual(4, bestHand.Value);
            Assert.AreEqual(number + 4, bestHand.KickerValues[0]);
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator H_ThreeOfAKind() {

        rnd = new System.Random();

        for (int i = 0; i < 100; i++) {

            int number = rnd.Next(4, 14);

            Card[] cards = new Card[5];
            cards[0] = new Card(1, number);
            cards[1] = new Card(2, 2);
            cards[2] = new Card(3, number);
            cards[3] = new Card(4, number);
            cards[4] = new Card(1, 3);

            Hand bestHand = Hand.GetHighestHand(cards);

            Assert.AreEqual(3, bestHand.Value);
            Assert.AreEqual(number, bestHand.KickerValues[0]);
            Assert.AreEqual(3, bestHand.KickerValues[1]);
            Assert.AreEqual(2, bestHand.KickerValues[2]);
        }

        for (int i = 0; i < 100; i++) {

            int number = rnd.Next(7, 14);

            Card[] cards = new Card[7];
            cards[0] = new Card(1, number);
            cards[1] = new Card(2, 2);
            cards[2] = new Card(3, number);
            cards[3] = new Card(4, number);
            cards[4] = new Card(1, 3);
            cards[4] = new Card(2, 4);
            cards[4] = new Card(3, 5);

            Hand bestHand = Hand.GetHighestHand(cards);

            Assert.AreEqual(3, bestHand.Value);
            Assert.AreEqual(number, bestHand.KickerValues[0]);
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator I_TwoPairs() {

        rnd = new System.Random();

        for (int i = 0; i < 100; i++) {

            int pairA = rnd.Next(2, 14);
            int pairB = rnd.Next(2, 14);
            while (pairB == pairA) {
                pairB = rnd.Next(2, 14);
            }

            int remain = rnd.Next(2, 14);
            while (remain == pairA || remain == pairB) {
                remain = rnd.Next(2, 14);
            }

            int big = 0;
            int small = 0;

            if (pairA > pairB) {
                big = pairA;
                small = pairB;
            } else {
                big = pairB;
                small = pairA;
            }

            Card[] cards = new Card[5];
            cards[0] = new Card(1, pairA);
            cards[1] = new Card(2, pairB);
            cards[2] = new Card(3, pairA);
            cards[3] = new Card(4, pairB);
            cards[4] = new Card(1, remain);

            Hand bestHand = Hand.GetHighestHand(cards);
            Assert.AreEqual(2, bestHand.Value);
            Assert.AreEqual(big, bestHand.KickerValues[0]);
            Assert.AreEqual(small, bestHand.KickerValues[1]);
            Assert.AreEqual(remain, bestHand.KickerValues[2]);
        }

        for (int i = 0; i < 100; i++) {

            int pairA = rnd.Next(6, 14);
            int pairB = rnd.Next(6, 14);
            while (pairB == pairA) {
                pairB = rnd.Next(6, 14);
            }

            int big = 0;
            int small = 0;

            if (pairA > pairB) {
                big = pairA;
                small = pairB;
            } else {
                big = pairB;
                small = pairA;
            }

            Card[] cards = new Card[5];
            cards[0] = new Card(1, pairA);
            cards[1] = new Card(2, pairB);
            cards[2] = new Card(3, pairA);
            cards[3] = new Card(4, pairB);
            cards[4] = new Card(1, 2);
            cards[4] = new Card(2, 2);
            cards[4] = new Card(3, 3);

            Hand bestHand = Hand.GetHighestHand(cards);
            Assert.AreEqual(2, bestHand.Value);
            Assert.AreEqual(big, bestHand.KickerValues[0]);
            Assert.AreEqual(small, bestHand.KickerValues[1]);
        }
        yield return null;
    }

    [UnityTest]
    public IEnumerator J_OnePair() {

        rnd = new System.Random();

        for (int i = 0; i < 100; i++) {

            int pair = rnd.Next(5, 14);

            Card[] cards = new Card[5];
            cards[0] = new Card(1, pair);
            cards[1] = new Card(2, pair);
            cards[2] = new Card(3, 2);
            cards[3] = new Card(4, 3);
            cards[4] = new Card(1, 4);

            Hand bestHand = Hand.GetHighestHand(cards);
            Assert.AreEqual(1, bestHand.Value);
            Assert.AreEqual(pair, bestHand.KickerValues[0]);
            Assert.AreEqual(4, bestHand.KickerValues[1]);
            Assert.AreEqual(3, bestHand.KickerValues[2]);
        }

        for (int i = 0; i < 100; i++) {

            int pair = rnd.Next(8, 14);

            Card[] cards = new Card[7];
            cards[0] = new Card(1, pair);
            cards[1] = new Card(2, pair);
            cards[2] = new Card(3, 2);
            cards[3] = new Card(4, 3);
            cards[4] = new Card(1, 4);
            cards[5] = new Card(1, 5);
            cards[6] = new Card(1, 7);

            Hand bestHand = Hand.GetHighestHand(cards);
            Assert.AreEqual(1, bestHand.Value);
            Assert.AreEqual(pair, bestHand.KickerValues[0]);
            Assert.AreEqual(7, bestHand.KickerValues[1]);
            Assert.AreEqual(5, bestHand.KickerValues[2]);
            Assert.AreEqual(4, bestHand.KickerValues[3]);
        }
        yield return null;
    }

    [UnityTest]
    public IEnumerator K_HighCard() {

        Card[] cards = new Card[5];
        cards[0] = new Card(1, 2);
        cards[1] = new Card(2, 3);
        cards[2] = new Card(3, 4);
        cards[3] = new Card(4, 5);
        cards[4] = new Card(1, 7);

        Hand bestHand = Hand.GetHighestHand(cards);
        Assert.AreEqual(0, bestHand.Value);
        Assert.AreEqual(7, bestHand.KickerValues[0]);
        Assert.AreEqual(5, bestHand.KickerValues[1]);
        Assert.AreEqual(4, bestHand.KickerValues[2]);


        cards = new Card[7];
        cards[0] = new Card(1, 2);
        cards[1] = new Card(2, 3);
        cards[2] = new Card(3, 4);
        cards[3] = new Card(4, 5);
        cards[4] = new Card(1, 7);
        cards[5] = new Card(1, 9);
        cards[6] = new Card(1, 11);

        bestHand = Hand.GetHighestHand(cards);
        Assert.AreEqual(0, bestHand.Value);
        Assert.AreEqual(11, bestHand.KickerValues[0]);
        Assert.AreEqual(9, bestHand.KickerValues[1]);
        Assert.AreEqual(7, bestHand.KickerValues[2]);

        yield return null;
    }

    [UnityTest]
    public IEnumerator L_CompareToTo() {

        Card[] playerAcards = new Card[5];
        playerAcards[0] = new Card(1, 2);
        playerAcards[1] = new Card(1, 3);
        playerAcards[2] = new Card(1, 4);
        playerAcards[3] = new Card(1, 2);
        playerAcards[4] = new Card(1, 3);

        Hand playerA = Hand.GetHighestHand(playerAcards);

        Card[] playerBcards = new Card[5];
        playerBcards[0] = new Card(1, 3);
        playerBcards[1] = new Card(2, 3);
        playerBcards[2] = new Card(3, 3);
        playerBcards[3] = new Card(4, 2);
        playerBcards[4] = new Card(1, 2);

        Hand playerB = Hand.GetHighestHand(playerBcards);

        Card[] playerCcards = new Card[5];
        playerCcards[0] = new Card(2, 2);
        playerCcards[1] = new Card(2, 3);
        playerCcards[2] = new Card(2, 4);
        playerCcards[3] = new Card(2, 2);
        playerCcards[4] = new Card(2, 3);

        Hand playerC = Hand.GetHighestHand(playerCcards);

        Assert.AreEqual(1, playerA.CompareTo(playerB));
        Assert.AreEqual(-1, playerB.CompareTo(playerA));
        Assert.AreEqual(0, playerA.CompareTo(playerA));
        Assert.IsTrue(playerA.Equals(playerC));

        yield return null;
    }


}