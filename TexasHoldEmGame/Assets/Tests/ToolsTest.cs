using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

public class ToolsTest {

    private System.Random rnd;

    [UnityTest]
    public IEnumerator ToolsDoesNotAllowIncorrectInput() {

        Assert.IsFalse(Tools.CorrectInput(""));
        Assert.IsFalse(Tools.CorrectInput("test"));
        Assert.IsFalse(Tools.CorrectInput("-123"));
        Assert.IsFalse(Tools.CorrectInput("1.2."));

        yield return null;
    }

    [UnityTest]
    public IEnumerator ToolsAllowsCorrectInput() {

        Assert.IsTrue(Tools.CorrectInput("1.23"));
        Assert.IsTrue(Tools.CorrectInput("123"));
        Assert.IsTrue(Tools.CorrectInput("0.23"));
        Assert.IsTrue(Tools.CorrectInput("1.234"));
        Assert.IsTrue(Tools.CorrectInput("1,23"));
        Assert.IsTrue(Tools.CorrectInput("0,23"));
        Assert.IsTrue(Tools.CorrectInput(".23"));
        Assert.IsTrue(Tools.CorrectInput(",23"));

        yield return null;
    }

    [UnityTest]
    public IEnumerator CorrectMoneyString() {

        Assert.AreEqual("1.23", Tools.IntToMoney(123));
        Assert.AreEqual("0.23", Tools.IntToMoney(23));
        Assert.AreEqual("0.03", Tools.IntToMoney(3));
        Assert.AreEqual("1.00", Tools.IntToMoney(100));
        Assert.AreEqual("10.00", Tools.IntToMoney(1000));

        yield return null;
    }

    [UnityTest]
    public IEnumerator CorrectMoneyInteger() {

        Assert.AreEqual(123, Tools.MoneyToInt("1.23"));
        Assert.AreEqual(12, Tools.MoneyToInt(".12"));
        Assert.AreEqual(1234, Tools.MoneyToInt("12.34"));
        Assert.AreEqual(1000, Tools.MoneyToInt("10.001"));
        Assert.AreEqual(1000, Tools.MoneyToInt("10.0099"));

        yield return null;
    }

}