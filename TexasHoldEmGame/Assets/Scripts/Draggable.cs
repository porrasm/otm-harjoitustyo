using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Draggable : NetworkBehaviour {

    public float distance = 5f;
    public float minDistance = 1f;
    public float maxDistance = 10f;
    private float zoomSens = 0.5f;
    public float rotationSens = 0.4f;
    public float tableHeight = 3.6f;
    private float throwPower = 10f;
    private float height = 3.5f;

    private float vectorDistance = 0;

    private Rigidbody rb;

    private Vector3 currentPos;
    private Vector3 lastPos;

    private Camera playerCamera;

    void Start() {

        rb = GetComponent<Rigidbody>();
        lastPos = transform.position;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void OnMouseDown() {

        if (playerCamera == null) {
            playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>();
        }

        lastPos = currentPos;
        distance = playerCamera.transform.position.y - transform.position.y + 4.5f;
        height = transform.position.y;

    }

    void OnMouseDrag() {

        lastPos = currentPos;
        UpdateRotation();
        UpdateMouse();

    }

    void OnMouseUp() {

        UpdateVelocity();
    }

    //Private Methods

    private void UpdateMouse() {

        UpdateDistance();

        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        Vector3 objectPos = playerCamera.ScreenToWorldPoint(mousePos);

        if (objectPos.y < tableHeight) {

            distance -= tableHeight - objectPos.y;

            mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
            objectPos = playerCamera.ScreenToWorldPoint(mousePos);
            objectPos.y = tableHeight;
        }

        currentPos = objectPos;
        transform.position = objectPos;

    }

    private void AlternateUpdate() {

        UpdateHeight();

        vectorDistance = transform.position.z - Input.mousePosition.z;
        vectorDistance = Mathf.Abs(vectorDistance);

        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, vectorDistance);
        Vector3 objectPos = playerCamera.ScreenToWorldPoint(mousePos);

        if (objectPos.y < tableHeight) {

            distance -= tableHeight - objectPos.y;

            mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
            objectPos = playerCamera.ScreenToWorldPoint(mousePos);
        }

        objectPos.y = height;

        currentPos = objectPos;
        transform.position = objectPos;

    }

    private void UpdateHeight() {

        height += Input.GetAxis("Mouse ScrollWheel") * zoomSens;

        if (height > maxDistance) {
            height = maxDistance;
        }

    }

    private void UpdateVelocity() {

        Vector3 difference = new Vector3(lastPos.x - currentPos.x, lastPos.y - currentPos.y, lastPos.z - currentPos.z);
        rb.velocity = difference / -Time.deltaTime;

    }

    private void UpdateRotation() {

        float rotX = Input.GetAxis("Vertical") * rotationSens * Time.deltaTime;
        float rotZ = Input.GetAxis("Horizontal") * rotationSens * Time.deltaTime;

        Vector3 rotSpeed = new Vector3(rb.angularVelocity.x + rotX, rb.angularVelocity.y, rb.angularVelocity.z + rotZ);

        rb.angularVelocity = rotSpeed;

    }

    private void UpdateDistance() {

        distance += Input.GetAxis("Mouse ScrollWheel") * zoomSens;

        if (distance < minDistance) {
            distance = minDistance;
        }

        if (distance > maxDistance) {
            distance = maxDistance;
        }

    }

}
