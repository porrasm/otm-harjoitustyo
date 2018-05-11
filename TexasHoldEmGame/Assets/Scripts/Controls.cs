using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Controls : NetworkBehaviour {

    ScoreboardUI scoreboard;

    private Quaternion characterTarget;
    private Quaternion camTarget;

    void Start() {

        if (!isLocalPlayer) {
            return;
        }

        scoreboard = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<ScoreboardUI>();
    }

    void Update() {

        if (!isLocalPlayer) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Tab)) {
            scoreboard.ShowScoreBoard(true);
        }

        if (Input.GetKeyUp(KeyCode.Tab)) {
            scoreboard.ShowScoreBoard(false);
        }
    }
}