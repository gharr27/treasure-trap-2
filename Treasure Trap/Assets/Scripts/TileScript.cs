using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileScript : MonoBehaviour {

    bool isMouseOver = false;
    public bool isCovered = false;
    public string color;
    string saveColor;
    Vector3 pos;

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
        return color;
    }

    // Start is called before the first frame update
    void Start() {
        Debug.Log("Tile Instantiated");

        boxCollider = this.GetComponent(typeof(BoxCollider)) as BoxCollider;
        gameController = GameObject.FindWithTag("GameController");
        gameManager = gameController.GetComponent(typeof(GameManager)) as GameManager;

        

        if (gameManager.isAIGame) {
            playerWhiteObj = gameManager.Players[0];
            playerBlackObj = gameManager.AI;

            p1 = playerWhiteObj.GetComponent(typeof(PlayerScript)) as PlayerScript;
            ai = playerBlackObj.GetComponent(typeof(AI)) as AI;
        }
        else if (gameManager.isNetworkGame) {
            playerWhiteObj = gameManager.Players[0];
            playerBlackObj = gameManager.Players[1];

            p1 = playerWhiteObj.GetComponent(typeof(PlayerScript)) as PlayerScript;
            p2 = playerBlackObj.GetComponent(typeof(PlayerScript)) as PlayerScript;
        }

        if (CompareTag("White")) {
            color = "white";
            saveColor = color;
        }
        else if (CompareTag("Black")) {
            color = "black";
            saveColor = color;
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
        

        Debug.Log("Tile Pos = " + transform.position);
    }

    private void Update() {

        pos = transform.position;

        x = pos.x;
        y = pos.y + 1;
        z = pos.z;

        top = new Vector3(x, y, z);

        if (gameManager.gameGrid[top].isFilled) {
            TileScript tileScript = gameManager.gameGrid[top].tile.GetComponent(typeof(TileScript)) as TileScript;
            color = tileScript.GetTileColor();
            isCovered = true;
        }
        else {
            color = saveColor;
            isCovered = false;
        }

        if (gameManager.isAIGame) {
            if (Input.GetMouseButtonDown(0) && isMouseOver && p1.isTurn && p1.isQueenPlaced && color == p1.color) {
                if (!isCovered) {
                    p1.isMove = true;
                    p1.SetTile(gameObject);
                    gameManager.SetMoveGrid(gameObject, true);
                }
            }
        }
        else if (gameManager.isNetworkGame) {
            if (Input.GetMouseButtonDown(0) && isMouseOver && p1.isTurn && p1.isQueenPlaced && gameManager.isP1) {
                if (!isCovered) {
                    p1.isMove = true;
                    gameManager.SendTilePos(transform.position);
                    gameManager.SetMoveGrid(gameObject, true);
                }
            }
            else if (Input.GetMouseButtonDown(0) && isMouseOver && p2.isTurn && p2.isQueenPlaced && !gameManager.isP1) {
                if (!isCovered) {
                    p2.isMove = true;
                    gameManager.SendTilePos(transform.position);
                    gameManager.SetMoveGrid(gameObject, true);
                }
            }
        }
        else {
            if (!isCovered) {
                if (Input.GetMouseButtonDown(0) && isMouseOver && gameManager.turn == 0 && color == "white") {
                    p1.SetTile(gameObject);
                }
                else if (Input.GetMouseButtonDown(0) && isMouseOver && gameManager.turn == 1 && color == "black") {
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
