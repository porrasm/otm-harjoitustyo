using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CardObject : NetworkBehaviour {

    private Collider c;

    private Transform suit;
    private Transform number;
    private Card card;

    private GameObject owner;
    public GameObject Owner { get { return owner; } }
    private Rigidbody rb;

    private Vector3 targetPosition;
    private Vector3 targetRotation;
    private Vector3 posVelocity;
    private Vector3 rotVelocity;

    private float smoothTime = 1;
    private float moveSpeed;

    private bool initialized;
    private bool arrived;
    private bool kill;

    public Card Card { get { return card; } }

    void Awake() {
        suit = transform.GetChild(2);
        number = transform.GetChild(3);
        GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        targetPosition = Vector3.zero;
        rb = GetComponent<Rigidbody>();
        c = GetComponent<Collider>();
    }

    void Start() {

        if (!isServer) { return; }

        CmdUpdateCard();
    }

	[Command]
    void CmdUpdateCard() {
        RpcSetOwner(owner);
        RpcSetCard(card.Suit, card.Number);
    }

    void Update() {


        if (!isServer) {

            if (!initialized) {
                CmdUpdateCard();
            }

            return;
        }

        if (!arrived) {

            if (c.enabled || !rb.isKinematic) {
                c.enabled = false;
                rb.isKinematic = true;
            }

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref posVelocity, smoothTime);
            transform.eulerAngles = Vector3.SmoothDamp(transform.eulerAngles, targetRotation, ref rotVelocity, smoothTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f) {
                transform.position = targetPosition;
                transform.eulerAngles = targetRotation;
                arrived = true;
                rb.isKinematic = false;
                c.enabled = true;

                if (kill) {
                    NetworkServer.Destroy(gameObject);
                }
            }
        }
    }


    public void Initialize(GameObject player, Card card) {
        this.owner = player;
        this.card = card;
    }
	[ClientRpc]
    public void RpcSetOwner(GameObject player) {
        this.owner = player;
    }
	[ClientRpc]
    public void RpcSetCard(int suit, int number) {
        card = new Card();
        card.SetCard(suit, number);
        UpdateCard();
    }

    public void SetTargetPosition(Vector3 position, Vector3 rotation) {
        if (position != Vector3.zero) {
            rb.isKinematic = true;
        }
        targetPosition = position;
        targetRotation = rotation;
    }
    public void TurnCard() {
        targetRotation += new Vector3(0, 0, 180);
        arrived = false;
    }

    public void KillCard() {
        Transform deck = GameObject.FindGameObjectWithTag("Table").GetComponent<Table>().Deck;
        targetPosition = deck.position;
        targetRotation = deck.eulerAngles;
        arrived = false;
        kill = true;
    }

    public void UpdateCard() {

        suit.GetComponent<Renderer>().material.mainTexture = Tools.GetSuitTexture(card);
        number.GetComponent<Renderer>().material.mainTexture = Tools.GetNumberTexture(card);

        suit.GetComponent<Renderer>().material.color = Tools.CardColor(card);
        number.GetComponent<Renderer>().material.color = Tools.CardColor(card);

        initialized = true;
    }

    public void DisableCard() {

        suit.GetComponent<Material>().color = new Color(0, 0, 0, 0);
        number.GetComponent<Material>().color = new Color(0, 0, 0, 0);
    }
}