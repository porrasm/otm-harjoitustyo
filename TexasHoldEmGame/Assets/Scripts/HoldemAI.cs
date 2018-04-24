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

    public override void EnablePlayerTurn(bool enable, bool payUp) {

        if (!AiEnabled) {
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
        Turn.Raise = 0;
        Ready = true;
    }
}
