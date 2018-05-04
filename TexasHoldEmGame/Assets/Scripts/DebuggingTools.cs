using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggingTools : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.H)) {
            Tools.PopUp("test: " + Random.value);
        }
	}
}
