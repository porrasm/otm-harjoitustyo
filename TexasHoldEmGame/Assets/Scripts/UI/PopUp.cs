using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour {

    private TexasHoldEm game;

    private RectTransform rt;

    private float progress = 15;

    [SerializeField]
    private Text textObjext;
    [SerializeField]
    private Image background;

    private Color textColor;
    private Color backColor;

    [SerializeField]
    private Vector2 startPos = new Vector3(110, 0);

    public int index;

    void Start() {
        rt = GetComponent<RectTransform>();
        textColor = textObjext.color;
        backColor = background.color;
        game = GameObject.FindGameObjectWithTag("Scripts").GetComponent<TexasHoldEm>();
    }

    void Update() {
        progress -= Time.deltaTime;

        UpdatePosition();

        if (progress < 0) {
            Destroy(gameObject);
        } else if (progress < 5) {

            float current = progress / 5;
            textObjext.color = new Color(textColor.r, textColor.g, textColor.b, current);
            background.color = new Color(backColor.r, backColor.g, backColor.b, current);
        }
    }

    private string text;

    /// <summary>
    /// Set the text of this pop up-
    /// </summary>
    /// <param name="param1">Text</param>
    public void Initialize(string text) {
        rt = GetComponent<RectTransform>();
        this.text = text;
        textObjext.text = text;
        rt.anchoredPosition = startPos;
    }
    
    private void UpdatePosition() {
        int difference = game.popUps - index;
        int height = difference * 35;
        rt.anchoredPosition = new Vector2(startPos.x, startPos.y + height);
    }
}
