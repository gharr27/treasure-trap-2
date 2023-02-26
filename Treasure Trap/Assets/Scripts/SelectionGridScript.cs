using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionGridScript : MonoBehaviour
{
    BoxCollider boxCollider;
    bool isMouseOver = false;

    GameManager gameManager;
    GameObject gameController;

    PlayerScript playerWhite;
    PlayerScript playerBlack;
    GameObject[] playerObject = new GameObject[2];

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = this.GetComponent(typeof(BoxCollider)) as BoxCollider;
        gameController = GameObject.FindWithTag("GameController");
        gameManager = gameController.GetComponent(typeof(GameManager)) as GameManager;

        playerObject = GameObject.FindGameObjectsWithTag("Player");
        playerBlack = playerObject[0].GetComponent(typeof(PlayerScript)) as PlayerScript;
        playerWhite = playerObject[1].GetComponent(typeof(PlayerScript)) as PlayerScript;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isMouseOver && gameManager.GetTurn() == 0) {
            playerWhite.SetPos(this.transform.position);
        }
        else if (Input.GetMouseButtonDown(0) && isMouseOver && gameManager.GetTurn() == 1) {
            playerBlack.SetPos(this.transform.position);
        }
    }

    void OnMouseOver() {
        isMouseOver = true;
    }

    void OnMouseExit() {
        isMouseOver = false;
    }
}
