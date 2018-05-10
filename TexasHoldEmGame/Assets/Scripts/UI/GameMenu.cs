using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour {

    [SerializeField]
    Transform menuPanel, initPanel;

    [SerializeField]
    InputField nameField;

    public void Ready() {
        transform.root.GetComponent<Player>().CmdSetReady(true);
        transform.root.GetComponent<Player>().CmdSetName(nameField.text);
    }

    public void EnableMenu(bool enable) {
        menuPanel.gameObject.SetActive(enable);
    }
    public void DisableInit() {
        initPanel.gameObject.SetActive(false);
    }
}
