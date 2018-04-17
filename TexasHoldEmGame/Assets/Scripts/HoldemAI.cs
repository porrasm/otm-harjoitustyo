using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HoldemAI : Player {

    public bool aiEnabled;

    void Start() {
        Ready = true;
        aiEnabled = true;
    }

    public override void EnablePlayerTurn(bool enable, bool payUp) {

        if (!aiEnabled) {
            return;
        }

        if (!enable) {
            return;
        }

        

        if (Turn == null) {
            Turn = new Turn();
        }
        Turn.NewTurn();

        if (Money < Needed) {
            Money = Needed * 2;
        }
        Turn.raise = 0;
        Ready = true;
    }

}
