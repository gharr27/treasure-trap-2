using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionGridScript : MonoBehaviour
{
    BoxCollider boxCollider;
    bool isMouseOver = false;

    GameManager gameManager;
    GameObject gameController;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = this.GetComponent(typeof(BoxCollider)) as BoxCollider;
        gameController = GameObject.FindWithTag("GameController");
        gameManager = gameController.GetComponent(typeof(GameManager)) as GameManager;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isMouseOver) {
            gameManager.SetPosition(this.transform.position);
        }
    }

    void OnMouseOver() {
        isMouseOver = true;
    }

    void OnMouseExit() {
        isMouseOver = false;
    }
}
