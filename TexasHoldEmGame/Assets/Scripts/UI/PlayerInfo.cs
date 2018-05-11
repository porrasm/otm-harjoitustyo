using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {
    private float updateInterval = 1;
    private float currentTime;
	void Update () {
        currentTime -= Time.deltaTime;
        transform.eulerAngles = new Vector3(90, 0, 0);
        if (currentTime < 0) {
            UpdateInfo();
            currentTime = updateInterval;
        }
	}
    void UpdateInfo() {
        transform.GetChild(0).GetComponent<TextMesh>().text = transform.root.name;
        transform.GetChild(1).GetComponent<TextMesh>().text = string.Empty + Tools.IntToMoney(transform.root.GetComponent<Player>().Money);
    }
}
