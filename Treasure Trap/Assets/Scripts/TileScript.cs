using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileScript : MonoBehaviour {

    bool isMouseOver = false;
    bool isWhite;

    BoxCollider boxCollider;

    GameManager gameManager;
    GameObject gameController;

    PlayerScript p1;
    PlayerScript p2;
    AI ai;
    GameObject playerWhiteObj;
    GameObject playerBlackObj;

    static int queenId = 1;
    static int antId = 1;
    static int grasshopperId = 1;
    static int beetleId = 1;
    static int spiderId = 1;

    public string tileName;

    string id;

    float x;
    float y;
    float z;

    Vector3 top;

    public string GetTileColor() {
        if (isWhite) {
            return "white";
        }
        else {
            return "black";
        }
    }

    // Start is called before the first frame update
    void Start() {
        boxCollider = this.GetComponent(typeof(BoxCollider)) as BoxCollider;
        gameController = GameObject.FindWithTag("GameController");
        gameManager = gameController.GetComponent(typeof(GameManager)) as GameManager;

        playerWhiteObj = gameManager.Players[0];
        playerBlackObj = gameManager.AI;

        p1 = playerWhiteObj.GetComponent(typeof(PlayerScript)) as PlayerScript;
        ai = playerBlackObj.GetComponent(typeof(AI)) as AI;


        //Checks which turn it is to assign tile color

        if (CompareTag("White")) {
            isWhite = true;
        }
        else if (CompareTag("Black")) {
            isWhite = false;
        }

        if (tileName == "Queen") {
            id = tileName + queenId;
            queenId++;
        }
        else if (tileName == "Ant") {
            id = tileName + antId;
            antId++;
        }
        else if (tileName == "Grasshopper") {
            id = tileName + grasshopperId;
            grasshopperId++;
        }
        else if (tileName == "Beetle") {
            id = tileName + beetleId;
            beetleId++;
        }
        else if (tileName == "Spider") {
            id = tileName + spiderId;
            spiderId++;
        }

        x = transform.position.x;
        y = transform.position.y + 1;
        z = transform.position.z;

        top = new Vector3(x, y, z);
    }

    private void Update() {
        if (gameManager.isAIGame) {
            if (Input.GetMouseButtonDown(0) && isMouseOver && p1.isTurn && p1.isQueenPlaced) {
                if (!gameManager.gameGrid[top].isFilled) {
                    gameManager.SetMoveGrid(gameObject, true);
                }
            }
        }
        else {
            if (!gameManager.gameGrid[top].isFilled) {
                if (Input.GetMouseButtonDown(0) && isMouseOver && gameManager.turn == 0 && isWhite) {
                    p1.SetTile(gameObject);
                }
                else if (Input.GetMouseButtonDown(0) && isMouseOver && gameManager.turn == 1 && !isWhite) {
                    p2.SetTile(gameObject);
                }
            }
        }
    }

    public string GetId() {
        return id;
    }

    public string GetTileName() {
        return tileName;
    }

    void OnMouseOver() {
        isMouseOver = true;
    }

    void OnMouseExit() {
        isMouseOver = false;
    }
}
