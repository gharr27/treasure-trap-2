using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleScript : MonoBehaviour
{
    BoxCollider boxCollider;
    bool isMouseOver = false;

    GameManager gameManager;
    GameObject gameController;

    static int idNum = 1;
    public string id;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = this.GetComponent(typeof(BoxCollider)) as BoxCollider;
        gameController = GameObject.FindWithTag("GameController");
        gameManager = gameController.GetComponent(typeof(GameManager)) as GameManager;

        id = id + idNum;
        idNum++;
        //Debug.Log(id);
    }

    void Update() {
        if (Input.GetMouseButtonDown(0) && isMouseOver) {
            gameManager.SetSelectedPiece(this.gameObject);
        }
    }

    public void SetId(string newId) {
        id = newId;
    }

    public string GetId() {
        return id;
    }

    void OnMouseOver() {
        isMouseOver = true;
    }

    void OnMouseExit() {
        isMouseOver = false;
    }
}