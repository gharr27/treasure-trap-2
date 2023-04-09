using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public GameObject[] Tiles;
    public string color;
    public bool isNetworkGame = false;
    public bool isTurn;

    GameManager gameManager;
    GameObject gameController;

    PhotonView photonView;

    GameObject tile = null;
    Vector3 pos = Vector3.zero;
    bool isMove = false;
    bool isFirstMove = true;
    bool isWhite;
    bool isQueenPlaced = false;

    public int queenCount = 1;
    public int antCount = 3;
    public int grasshopperCount = 3;
    public int beetleCount = 2;
    public int spiderCount = 2;


    // Start is called before the first frame update
    void Start() {
        gameController = GameObject.FindWithTag("GameController");
        gameManager = gameController.GetComponent(typeof(GameManager)) as GameManager;

        PhotonView photonView = this.GetComponent<PhotonView>();
    }

    public void selectedQueen()
    {
        if (queenCount > 0 && gameManager.GetRound() != 1 && isTurn)
        {
            tile = Tiles[0];
            isMove = false;
            queenCount--;

            if (!isFirstMove || gameManager.GetTurn() == 1) {
                gameManager.SetMoveGrid(tile, isMove);
            }
            else {
                pos = Vector3.zero;
                Move();
            }
        }
    }
    public void selectedAnt()
    {
        if (gameManager.GetRound() < 4 || isQueenPlaced) {
            if (antCount > 0 && isTurn) {
                tile = Tiles[1];
                isMove = false;
                antCount--;

                if (!isFirstMove || gameManager.GetTurn() == 1) {
                    gameManager.SetMoveGrid(tile, isMove);
                }
                else {
                    pos = Vector3.zero;
                    Move();
                }
            }
        }
    }
    public void selectedGrasshopper()
    {
        if (gameManager.GetRound() < 4 || isQueenPlaced) {
            if (grasshopperCount > 0 && isTurn) {
                tile = Tiles[2];
                isMove = false;
                grasshopperCount--;

                if (!isFirstMove || gameManager.GetTurn() == 1) {
                    gameManager.SetMoveGrid(tile, isMove);
                }
                else {
                    pos = Vector3.zero;
                    Move();
                }
            }
        }
    }
    public void selectedBeetle()
    {
        if (gameManager.GetRound() < 4 || isQueenPlaced) {
            if (beetleCount > 0 && isTurn) {
                tile = Tiles[3];
                isMove = false;
                beetleCount--;

                if (!isFirstMove || gameManager.GetTurn() == 1) {
                    gameManager.SetMoveGrid(tile, isMove);
                }
                else {
                    pos = Vector3.zero;
                    Move();
                }
            }
        }
    }
    public void selectedSpider()
    {
        if (gameManager.GetRound() < 4 || isQueenPlaced) {
            if (spiderCount > 0 && isTurn) {
                tile = Tiles[4];
                isMove = false;
                spiderCount--;

                if (!isFirstMove || gameManager.GetTurn() == 1) {
                    gameManager.SetMoveGrid(tile, isMove);
                }
                else {
                    pos = Vector3.zero;
                    Move();
                }
            }
        }
    }

    public void Move() {
        gameManager.MakeMove(tile, pos, isMove);
    }

    public void SetPos(Vector3 newPos) {
        pos = newPos;
        Move();
    }

    public void SetTile(GameObject newTile) {
        tile = newTile;
    }

    public void QueenPlaced() {
        isQueenPlaced = true;
    }
}
