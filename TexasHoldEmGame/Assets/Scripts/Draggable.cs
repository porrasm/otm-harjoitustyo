using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Draggable : NetworkBehaviour {

    private Rigidbody rb;
    private Camera playerCamera;

    private Vector3 currentPos;
    private Vector3 lastPos;

    public float Distance = 5f;
    public float MinDistance = 1f;
    public float MaxDistance = 10f;
    public float RotationSens = 0.4f;
    public float TableHeight = 3.6f;

    private float height = 3.5f;
    private float zoomSens = 0.5f;
    private float vectorDistance = 0;


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
        Distance = playerCamera.transform.position.y - transform.position.y + 4.5f;
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

    // Private Methods

    private void UpdateMouse() {

        UpdateDistance();

        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Distance);
        Vector3 objectPos = playerCamera.ScreenToWorldPoint(mousePos);

        if (objectPos.y < TableHeight) {

            Distance -= TableHeight - objectPos.y;

            mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Distance);
            objectPos = playerCamera.ScreenToWorldPoint(mousePos);
            objectPos.y = TableHeight;
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

        if (objectPos.y < TableHeight) {

            Distance -= TableHeight - objectPos.y;

            mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Distance);
            objectPos = playerCamera.ScreenToWorldPoint(mousePos);
        }

        objectPos.y = height;

        currentPos = objectPos;
        transform.position = objectPos;
    }

    private void UpdateHeight() {

        height += Input.GetAxis("Mouse ScrollWheel") * zoomSens;

        if (height > MaxDistance) {
            height = MaxDistance;
        }
    }

    private void UpdateVelocity() {

        Vector3 difference = new Vector3(lastPos.x - currentPos.x, lastPos.y - currentPos.y, lastPos.z - currentPos.z);
        rb.velocity = difference / -Time.deltaTime;
    }

    private void UpdateRotation() {

        float rotX = Input.GetAxis("Vertical") * RotationSens * Time.deltaTime;
        float rotZ = Input.GetAxis("Horizontal") * RotationSens * Time.deltaTime;

        Vector3 rotSpeed = new Vector3(rb.angularVelocity.x + rotX, rb.angularVelocity.y, rb.angularVelocity.z + rotZ);

        rb.angularVelocity = rotSpeed;
    }

    private void UpdateDistance() {

        Distance += Input.GetAxis("Mouse ScrollWheel") * zoomSens;

        if (Distance < MinDistance) {
            Distance = MinDistance;
        }

        if (Distance > MaxDistance) {
            Distance = MaxDistance;
        }
    }
}
