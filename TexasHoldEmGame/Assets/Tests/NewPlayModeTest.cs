using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

public class NewPlayModeTest {



    [UnityTest]
    public IEnumerator GameInitializesCorrectly() {

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

        game.PlaceHolderStart();

        Assert.IsTrue(game.Players.Length == 1);
        Assert.IsFalse(game.GameIsReady);

        game.Players[0].Ready = true;

        yield return new WaitForSeconds(2);

        Assert.IsTrue(game.RoundIsOn);
        //AI 
        Assert.IsTrue(game.Players.Length == 10);

        foreach (Player p in game.Players) {

            Assert.IsTrue(p.Money == game.BuyIn);

        }

        // Use the Assert class to test conditions.
        // yield to skip a frame
        yield return null;
    }
}
