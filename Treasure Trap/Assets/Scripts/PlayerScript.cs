using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public bool isPlayingAI = false;
    public GameObject[] Tiles;

    GameManager gameManager;
    GameObject gameController;

    GameObject tile = null;
    Vector3 pos = Vector3.zero;
    bool isMove = false;
    bool isPosSelected = false;
    bool isPlayer1Move = false;
    bool isPlayer2Move = false;
    bool isGridSet = false;
    bool isTileSelected = false;
    bool isFirstMove = true;

    int whiteOrBlack;

    int queenCount = 1;
    int antCount = 3;
    int grasshopperCount = 3;
    int beetleCount = 2;
    int spiderCount = 2;

    // Start is called before the first frame update
    void Start() {
        gameController = GameObject.FindWithTag("GameController");
        gameManager = gameController.GetComponent(typeof(GameManager)) as GameManager;
    }

    void Update() {
        //Selected Queen
        if (Input.GetKeyDown(KeyCode.Alpha1) && queenCount > 0) {
            tile = Tiles[0];
            isMove = false;
            isTileSelected = true;
            queenCount--;
        }
        //Selected Ant
        else if (Input.GetKeyDown(KeyCode.Alpha2) && antCount > 0) {
            tile = Tiles[1];
            isMove = false;
            isTileSelected = true;
            antCount--;
        }
        //Selected Grasshopper
        else if (Input.GetKeyDown(KeyCode.Alpha3) && grasshopperCount > 0) {
            tile = Tiles[2];
            isMove = false;
            isTileSelected = true;
            grasshopperCount--;
        }
        //Selected Beetle
        else if (Input.GetKeyDown(KeyCode.Alpha4) && beetleCount > 0) {
            tile = Tiles[3];
            isMove = false;
            isTileSelected = true;
            beetleCount--;
        }
        //Selected Spider
        else if (Input.GetKeyDown(KeyCode.Alpha5) && spiderCount > 0) {
            tile = Tiles[4];
            isMove = false;
            isTileSelected = true;
            spiderCount--;
        }
    }

    public IEnumerator Move(int wOrB) {
        whiteOrBlack = wOrB;
        if(wOrB == 0) {
            Debug.Log("White Turn");
        }
        else {
            Debug.Log("Black Turn");
        }

        Debug.Log("Waiting For Tile Select");
        yield return new WaitWhile(IsTileSelected);
        Debug.Log("Tile Selected");

        if (!isFirstMove) {
            gameManager.SetMoveGrid(tile, isMove);
        }
        else {
            isPosSelected = true;
            isFirstMove = false;
        }

        Debug.Log("Waiting for Pos Select");
        yield return new WaitWhile(IsPosSelected);
        Debug.Log("Pos Selected");

        gameManager.MakeMove(tile, pos, isMove);
        isTileSelected = false;
        isPosSelected = false;
    }

    bool IsTileSelected() {
        return !isTileSelected;
    }

    bool IsPosSelected() {
        return !isPosSelected;
    }

    public void SetTile(GameObject newTile) {
        isMove = true;
        tile = newTile;
        isTileSelected = true;

        StartCoroutine(Move(whiteOrBlack));
    }

    public void SetPos(Vector3 newPos) {
        pos = newPos;
        isPosSelected = true;
    }
}
