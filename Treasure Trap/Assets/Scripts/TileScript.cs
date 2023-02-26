using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileScript : MonoBehaviour {

    BoxCollider boxCollider;
    bool isMouseOver = false;
    bool isWhite;

    GameManager gameManager;
    GameObject gameController;

    PlayerScript playerWhite;
    PlayerScript playerBlack;
    GameObject playerWhiteObj;
    GameObject playerBlackObj;

    static int queenId = 1;
    static int antId = 1;
    static int grasshopperId = 1;
    static int beetleId = 1;
    static int spiderId = 1;

    public string tileName;

    string id;

    public bool GetTileColor() {
        return isWhite;
    }

    // Start is called before the first frame update
    void Start() {
        boxCollider = this.GetComponent(typeof(BoxCollider)) as BoxCollider;
        gameController = GameObject.FindWithTag("GameController");
        gameManager = gameController.GetComponent(typeof(GameManager)) as GameManager;

        playerWhiteObj = GameObject.FindWithTag("White");
        playerBlackObj = GameObject.FindWithTag("Black");

        playerWhite = playerWhiteObj.GetComponent(typeof(PlayerScript)) as PlayerScript;
        playerBlack = playerBlackObj.GetComponent(typeof(PlayerScript)) as PlayerScript;

        //Weird Logic, check if it is currently blacks turn, if so this piece that has just been created is white
        if (gameManager.GetTurn() == 1) {
            isWhite = true;
        }
        else {
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
    }

    void Update() {
        if (Input.GetMouseButtonDown(0) && isMouseOver && gameManager.GetTurn() == 0 && isWhite) {
            playerWhite.SetTile(this.gameObject);
        }
        else if (Input.GetMouseButtonDown(0) && isMouseOver && gameManager.GetTurn() == 1 && !isWhite) {
            playerBlack.SetTile(this.gameObject);
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
