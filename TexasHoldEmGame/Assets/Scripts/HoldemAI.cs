using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HoldemAI : Player {

    void Start() {
        Ready = true;
    }

    public override void EnablePlayerTurn(bool enable, bool payUp) {

        if (!enable) {
            return;
        }

        

        if (Turn == null) {
            Turn = new Turn();
        }
        Turn.NewTurn();

        Money = Needed + 1;
        print("Holdem AI Checks: " + Needed);
        Turn.raise = 0;
        Ready = true;
    }

}
