using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionGridScript : MonoBehaviour
{
    BoxCollider boxCollider;
    bool isMouseOver = false;

    GameManager gameManager;
    GameObject gameController;

    PlayerScript p1;
    PlayerScript p2;
    AI ai;
    GameObject p1Obj;
    GameObject p2Obj;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = this.GetComponent(typeof(BoxCollider)) as BoxCollider;
        gameController = GameObject.FindWithTag("GameController");
        gameManager = gameController.GetComponent(typeof(GameManager)) as GameManager;


        p1Obj = gameManager.Players[0];
        p2Obj = gameManager.AI;

        p1 = p1Obj.GetComponent(typeof(PlayerScript)) as PlayerScript;
        ai = p2Obj.GetComponent(typeof(AI)) as AI;
    }

    // Update is called once per frame
    void Update() {
        if (gameManager.isAIGame) {
            if (Input.GetMouseButtonDown(0) && isMouseOver && gameManager.turn == 0) {
                p1.SetPos(transform.position);
            }
        }
        else {
            if (Input.GetMouseButtonDown(0) && isMouseOver && gameManager.turn == 0) {
                p1.SetPos(transform.position);
            }
            else if (Input.GetMouseButtonDown(0) && isMouseOver && gameManager.turn == 1) {
                p2.SetPos(transform.position);
            }
        }
    }

    void OnMouseOver() {
        isMouseOver = true;
    }

    void OnMouseExit() {
        isMouseOver = false;
    }
}
