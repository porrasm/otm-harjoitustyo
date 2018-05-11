using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HoldemAI : Player {

    public bool AiEnabled;

    void Start() {
        Ready = true;
        AiEnabled = true;
    }

    /// <summary>
    /// Enables or disables the turn of this player. Must be called on the server.
    /// </summary>
    /// <param name="param1">Enable</param>
    /// <param name="param2">PayUp round>
    public override void EnablePlayerTurn(bool enable, bool payUp) {

        if (!AiEnabled) {
            return;
        }

        if (!enable) {
            return;
        }

        Invoke("ContinueTurn", 0.1f);
    }
    void ContinueTurn() {
        if (Turn == null) {
            Turn = new Turn();
        }
        Turn.NewTurn();

        if (Money < Needed) {
            Money = Needed * 2;
        }
        Turn.TurnString = transform.name + " calls " + Tools.IntToMoney(Needed);
        Turn.Pay = 0;
        Ready = true;
    }
}
