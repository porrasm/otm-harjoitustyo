using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour {

    private RectTransform rt;

    private float progress = 10;

    [SerializeField]
    private Text textObjext;
    [SerializeField]
    private Image background;

    private Color textColor;
    private Color backColor;

    [SerializeField]
    private Vector2 startPos = new Vector3(110, 0);

    void Start() {
        rt = GetComponent<RectTransform>();
        PushAllUp();
        textColor = textObjext.color;
        backColor = background.color;
    }

    void Update() {
        progress -= Time.deltaTime;

        if (progress < 0) {
            Destroy(gameObject);
        } else if (progress < 5) {

            float current = progress / 5;
            textObjext.color = new Color(textColor.r, textColor.g, textColor.b, current);
            background.color = new Color(backColor.r, backColor.g, backColor.b, current);
        } 
    }

    void PushAllUp() {

        GameObject[] popUps = GameObject.FindGameObjectsWithTag("PopUp");

        foreach (GameObject o in popUps) {
            if (o != gameObject) {
                o.GetComponent<PopUp>().PushUp();
            }
        }
    }

    private string text;

    public void Initialize(string text) {
        rt = GetComponent<RectTransform>();
        this.text = text;
        textObjext.text = text;
        rt.anchoredPosition = startPos;
    }
    public void PushUp() {
        rt.anchoredPosition = rt.anchoredPosition + new Vector2(0, 25);
    }
}
