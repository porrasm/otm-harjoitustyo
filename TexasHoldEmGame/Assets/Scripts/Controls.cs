using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Controls : NetworkBehaviour {

    public float Sensitivity = 2f;
    [SerializeField]
    private Transform cam;

    ScoreboardUI scoreboard;

    private Quaternion characterTarget;
    private Quaternion camTarget;

    void Start() {

        if (!isLocalPlayer) {
            return;
        }

        scoreboard = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<ScoreboardUI>();

        transform.rotation = Quaternion.Euler(0, 0, 0);

        cam.gameObject.GetComponent<Camera>().enabled = true;

        transform.LookAt(GameObject.FindGameObjectWithTag("Table").GetComponent<Table>().Deck);
        characterTarget = transform.rotation;
        camTarget = cam.transform.localRotation;
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

        CheckCamera();
    }

    private void CheckCamera() {

        if (Input.GetKey(KeyCode.Mouse1)) {
            UpdateCamera();

            if (Cursor.lockState != CursorLockMode.Locked) {
                Cursor.lockState = CursorLockMode.Locked;
            }
            Cursor.visible = true;
        }

        if (Input.GetKeyUp(KeyCode.Mouse1)) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void UpdateCamera() {

        float yRot = Input.GetAxis("Mouse X") * Sensitivity;
        float xRot = Input.GetAxis("Mouse Y") * Sensitivity;

        characterTarget *= Quaternion.Euler(0f, yRot, 0f);
        camTarget *= Quaternion.Euler(-xRot, 0f, 0f);

        camTarget = ClampRotationAroundXAxis(camTarget);

        transform.rotation = characterTarget;
        cam.transform.localRotation = camTarget;
        transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
    }

    private Quaternion ClampRotationAroundXAxis(Quaternion q) {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, -90, 90);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}