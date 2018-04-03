﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Table : NetworkBehaviour {

    [SerializeField]
    private Transform playerPositions;
    public Transform PlayerPositions { get { return playerPositions; } }

    [SerializeField]
    private Transform cardPositions;
    public Transform CardPositions { get { return cardPositions; } }

    [SerializeField]
    private Transform deck;
    public Transform Deck { get { return deck; } }

    GameObject cardPrefab;

    //Methods
    void Start() {
        cardPrefab = Resources.Load("Card") as GameObject;

        for (int i = 0; i < playerPositions.childCount; i++) {

            Transform current = playerPositions.GetChild(i);
            current.LookAt(transform.position);
            current.GetChild(0).transform.localPosition = new Vector3(-0.02f, 0.51f, 0.15f);
            current.GetChild(1).transform.localPosition = new Vector3(0.02f, 0.51f, 0.15f);

        }

    }

    public void SpawnCard(Player owner, Card card) {
        SpawnCard(owner, card, transform);
    }
    public void SpawnCard(Player owner, Card card, Transform target) {

        GameObject newCard = Instantiate(cardPrefab);
        newCard.transform.position = deck.position;
        newCard.GetComponent<CardObject>().SetCard(card);
        newCard.GetComponent<CardObject>().SetOwner(owner);
        
        if (target != transform) {
            newCard.GetComponent<CardObject>().SetTargetPosition(target.position, target.eulerAngles);
        }

        NetworkServer.Spawn(newCard);

    }


    public Transform GetPlayerPosition(int index) {
        return playerPositions.GetChild(index);
    }
    public Transform GetCardPosition(int index) {
        return cardPositions.GetChild(index);
    }

}
