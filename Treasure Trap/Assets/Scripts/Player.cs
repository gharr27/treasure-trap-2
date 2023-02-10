using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public bool isPlayingAI = false;
    public GameObject[] Tiles;

    GameManager gameManager;
    GameObject gameController;

    GameObject tile = null;
    Vector3 pos = null;
    bool isMove = false;

    // Start is called before the first frame update
    void Start() {
        gameController = GameObject.FindWithTag("GameController");
        gameManager = gameController.GetComponent(typeof(GameManager)) as GameManager;
    }

    void Update() {
        if(tile == null) {
            SelectTile();

            if(pos != null) {
                GameManager.MakeMove(tile, pos, isMove);
            }
        }
    }

    void Move(int wOrB) {
        if(wOrB == 0) {
            //White Move
        }
        else {
            //Black Move
        }
    }

    void SelectTile() {
        //Selected Queen
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            tile = Tiles[0];
            isMove = false;
        }
        //Selected Ant
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            tile = Tiles[1];
            isMove = false;
        }
        //Selected Grasshopper
        else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            tile = Tiles[2];
            isMove = false;
        }
        //Selected Beetle
        else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            tile = Tiles[3];
            isMove = false;
        }
        //Selected Spider
        else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            tile = Tiles[4];
            isMove = false;
        }
    }

    public void SetTile(GameObject newTile) {
        isMove = true;
        tile = newTile;
        gameManager.SetMoveGrid();
    }

    public void SetPos(Vector3 newPos) {
        pos = newPos;
    }
}
