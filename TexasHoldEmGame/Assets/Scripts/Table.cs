using System.Collections;
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

    // Methods
    void Start() {
        cardPrefab = Resources.Load("Card") as GameObject;

        for (int i = 0; i < playerPositions.childCount; i++) {

            Transform current = playerPositions.GetChild(i);
            current.LookAt(transform.position);

            current.Translate(0, 0.6f, -0.5f);

            current.GetChild(0).transform.localPosition = new Vector3(-0.02f, 0.51f, 0.15f);
            current.GetChild(1).transform.localPosition = new Vector3(0.02f, 0.51f, 0.15f);

            current.GetChild(0).transform.Translate(0, -0.6f, 0.5f);
            current.GetChild(1).transform.Translate(0, -0.6f, 0.5f);
        }
    }

    /// <summary>
    /// Creates a card object for all clients. Must be called on the server.
    /// </summary>
    /// <param name="param1">The owner of the card</param>
    /// <param name="param2">Card</param>
    /// <returns>What this method returns.</returns>
    public void SpawnCard(GameObject owner, Card card) {
        SpawnCard(owner, card, transform);
    }

    /// <summary>
    /// Creates a card object for all clients and moves it to the target position. Must be called on the server.
    /// </summary>
    /// <param name="param1">The owner of the card</param>
    /// <param name="param2">Card</param>
    /// <param name="param3">Target position</param>
    /// <returns>What this method returns.</returns>
    public void SpawnCard(GameObject owner, Card card, Transform target) {

        GameObject newCard = Instantiate(cardPrefab);
        newCard.transform.position = deck.position;
        newCard.GetComponent<CardObject>().Initialize(owner, card);
        
        if (target != transform) {
            newCard.GetComponent<CardObject>().SetTargetPosition(target.position, target.eulerAngles);
        }

        NetworkServer.Spawn(newCard);
    }

    /// <summary>
    /// Returns the position of the player by index.
    /// </summary>
    /// <param name="param1">Index of the player</param>
    /// <returns>Position of the player</returns>
    public Transform GetPlayerPosition(int index) {
        return playerPositions.GetChild(index);
    }

    /// <summary>
    /// Returns the position of the table card by index.
    /// </summary>
    /// <param name="param1">Index of the card</param>
    /// <returns>Position of the card</returns>
    public Transform GetCardPosition(int index) {
        return cardPositions.GetChild(index);
    }
}
