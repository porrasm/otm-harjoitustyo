using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CardObject : NetworkBehaviour {

    Collider c;

    Transform suit;
    Transform number;
    Card card;
    GameObject owner;
    public GameObject Owner { get { return owner; } }
    float smoothTime = 1;
    bool initialized;
    Rigidbody rb;

    Vector3 targetPosition;
    Vector3 targetRotation;
    Vector3 posVelocity;
    Vector3 rotVelocity;
    bool arrived;
    bool kill;

    float moveSpeed;

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
    public void RpcSetOwner(GameObject player) {
        this.owner = player;
    }
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

        string suitString = "";

        switch (card.Suit) {

            case 1:
            suitString = "spade";
            break;
            case 2:
            suitString = "heart";
            break;
            case 3:
            suitString = "diamond";
            break;
            case 4:
            suitString = "club";
            break;
            default:
            suitString = "";
            break;
        }

        Texture suitTexture = Resources.Load("Cards/" + suitString) as Texture;
        Texture numberTexture = Resources.Load("Cards/" + card.Number) as Texture;

        if (suit == null) {
            print("Suit is null");
        }

        suit.GetComponent<Renderer>().material.mainTexture = suitTexture;
        number.GetComponent<Renderer>().material.mainTexture = numberTexture;

        if (card.Suit == 2 || card.Suit == 3) {
            suit.GetComponent<Renderer>().material.color = Color.red;
            number.GetComponent<Renderer>().material.color = Color.red;
        } else {
            suit.GetComponent<Renderer>().material.color = Color.black;
            number.GetComponent<Renderer>().material.color = Color.black;
        }

        initialized = true;

        //RpcSetCard(newCard);
    }
    [ClientRpc]
    public void RpcSetCard(Card newCard) {
        print("Setting card");

        card = newCard;

        string suitString = "";

        switch (card.Suit) {

            case 1:
            suitString = "spade";
            break;
            case 2:
            suitString = "heart";
            break;
            case 3:
            suitString = "diamond";
            break;
            case 4:
            suitString = "club";
            break;
            default:
            suitString = "";
            break;
        }

        print("Card: " + card.Number + " of " + suitString + "s");

        Texture suitTexture = Resources.Load("Cards/" + suitString) as Texture;
        Texture numberTexture = Resources.Load("Cards/" + card.Number) as Texture;

        suit.GetComponent<Renderer>().material.mainTexture = suitTexture;
        number.GetComponent<Renderer>().material.mainTexture = numberTexture;

        if (card.Number == 2 | card.Number == 3) {
            suit.GetComponent<Renderer>().material.color = Color.red;
            number.GetComponent<Renderer>().material.color = Color.red;
        } else {
            suit.GetComponent<Renderer>().material.color = Color.black;
            number.GetComponent<Renderer>().material.color = Color.black;
        }
    }

    public void DisableCard() {

        suit.GetComponent<Material>().color = new Color(0, 0, 0, 0);
        number.GetComponent<Material>().color = new Color(0, 0, 0, 0);

    }

}
