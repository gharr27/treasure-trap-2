using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    public float panSpeed = 5f;
    public float scrollSpeed = 15f;

    // Update is called once per frame
    void Update() {
        if (Input.GetKey("space")) {
            transform.position = new Vector3(0, 8, 0);
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)) {
            transform.Translate(Input.GetAxisRaw("Horizontal") * panSpeed * Time.deltaTime,
                Input.GetAxisRaw("Vertical") * panSpeed * Time.deltaTime,
                0);
        }
    }
}
