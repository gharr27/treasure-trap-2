using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public GameObject[] Tiles;
    public string color;
    public bool isNetworkGame = false;
    public bool isTurn;
    public bool isP1;

    GameManager gameManager;
    GameObject gameController;

    PhotonView photonView;

    GameObject tile = null;
    Vector3 pos = Vector3.zero;
    bool isMove = false;
    bool isFirstMove = true;
    public bool isQueenPlaced = false;

    public int queenCount = 1;
    public int antCount = 3;
    public int grasshopperCount = 3;
    public int beetleCount = 2;
    public int spiderCount = 2;


    // Start is called before the first frame update
    void Start() {
        gameController = GameObject.FindWithTag("GameController");
        gameManager = gameController.GetComponent(typeof(GameManager)) as GameManager;
    }

    public void DecrementTile(string name) {

        switch (name) {
            case "Queen":
                queenCount--;
                isQueenPlaced = true;
                break;
            case "Ant":
                antCount--;
                break;
            case "Grasshopper":
                grasshopperCount--;
                break;
            case "Beetle":
                beetleCount--;
                break;
            case "Spider":
                spiderCount--;
                break;
        }

    }

    public void selectedQueen() {
        if (gameManager.round > 1) {
            if (queenCount > 0 && isTurn && isP1 == gameManager.isP1) {
                tile = Tiles[0];
                isMove = false;

                if (!isFirstMove || gameManager.turn == 1) {
                    gameManager.SetMoveGrid(tile, isMove);
                }
                else {
                    pos = Vector3.zero;
                    Move();
                }
            }
        }
    }
    public void selectedAnt() {
        if (gameManager.round < 4 || isQueenPlaced) {
            if (antCount > 0 && isTurn && isP1 == gameManager.isP1) {
                tile = Tiles[1];
                isMove = false;

                if (!isFirstMove || gameManager.turn == 1) {
                    gameManager.SetMoveGrid(tile, isMove);
                }
                else {
                    isFirstMove = false;
                    pos = Vector3.zero;
                    Move();
                }
            }
        }
    }
    public void selectedGrasshopper() {
        if (gameManager.round < 4 || isQueenPlaced) {
            if (grasshopperCount > 0 && isTurn && isP1 == gameManager.isP1) {
                tile = Tiles[2];
                isMove = false;

                if (!isFirstMove || gameManager.turn == 1) {
                    gameManager.SetMoveGrid(tile, isMove);
                }
                else {
                    isFirstMove = false;
                    pos = Vector3.zero;
                    Move();
                }
            }
        }
    }
    public void selectedBeetle() {
        if (gameManager.round < 4 || isQueenPlaced) {
            if (beetleCount > 0 && isTurn && isP1 == gameManager.isP1) {
                tile = Tiles[3];
                isMove = false;

                if (!isFirstMove || gameManager.turn == 1) {
                    gameManager.SetMoveGrid(tile, isMove);
                }
                else {
                    isFirstMove = false;
                    pos = Vector3.zero;
                    Move();
                }
            }
        }
    }
    public void selectedSpider() {
        if (gameManager.round < 4 || isQueenPlaced) {
            if (spiderCount > 0 && isTurn && isP1 == gameManager.isP1) {
                tile = Tiles[4];
                isMove = false;

                if (!isFirstMove || gameManager.turn == 1) {
                    gameManager.SetMoveGrid(tile, isMove);
                }
                else {
                    isFirstMove = false;
                    pos = Vector3.zero;
                    Move();
                }
            }
        }
    }

    public void Move() {
        if (gameManager.isNetworkGame) {
            gameManager.SendMove(tile, pos, isMove, color);
        }
        else {
            gameManager.MakeMove(tile, pos, isMove);
        }
    }

    public void SetPos(Vector3 newPos) {
        pos = newPos;
        Move();
    }

    public void SetTile(GameObject newTile) {
        tile = newTile;
    }
}