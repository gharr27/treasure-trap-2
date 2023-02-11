using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

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

    // Start is called before the first frame update
    void Start() {
        gameController = GameObject.FindWithTag("GameController");
        gameManager = gameController.GetComponent(typeof(GameManager)) as GameManager;
    }

    void Update() {
        //Selected Queen
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            tile = Tiles[0];
            isMove = false;
            isTileSelected = true;
        }
        //Selected Ant
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            tile = Tiles[1];
            isMove = false;
            isTileSelected = true;
        }
        //Selected Grasshopper
        else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            tile = Tiles[2];
            isMove = false;
            isTileSelected = true;
        }
        //Selected Beetle
        else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            tile = Tiles[3];
            isMove = false;
            isTileSelected = true;
        }
        //Selected Spider
        else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            tile = Tiles[4];
            isMove = false;
            isTileSelected = true;
        }
    }

    public IEnumerator Move(int wOrB) {
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
            gameManager.SetMoveGrid();
        }

        if (isFirstMove) {
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

    void SelectTile() {
        Debug.Log("Test Failed");
        ////Selected Queen
        //if (Input.GetKeyDown(KeyCode.Alpha1)) {
        //    tile = Tiles[0];
        //    isMove = false;
        //    isTileSelected = true;
        //}
        ////Selected Ant
        //else if (Input.GetKeyDown(KeyCode.Alpha2)) {
        //    tile = Tiles[1];
        //    isMove = false;
        //    isTileSelected = true;
        //}
        ////Selected Grasshopper
        //else if (Input.GetKeyDown(KeyCode.Alpha3)) {
        //    tile = Tiles[2];
        //    isMove = false;
        //    isTileSelected = true;
        //}
        ////Selected Beetle
        //else if (Input.GetKeyDown(KeyCode.Alpha4)) {
        //    tile = Tiles[3];
        //    isMove = false;
        //    isTileSelected = true;
        //}
        ////Selected Spider
        //else if (Input.GetKeyDown(KeyCode.Alpha5)) {
        //    tile = Tiles[4];
        //    isMove = false;
        //    isTileSelected = true;
        //}
    }

    public void SetTile(GameObject newTile) {
        isMove = true;
        tile = newTile;
        isTileSelected = true;
    }

    public void SetPos(Vector3 newPos) {
        pos = newPos;
        isPosSelected = true;
    }
}
