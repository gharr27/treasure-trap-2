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

    public GameObject tile = null;
    public Vector3 pos = Vector3.zero;
    public bool isMove = false;
    bool isPosSelected = false;
    bool isGridSet = false;
    bool isTileSelected = false;
    bool isFirstMove = true;
    bool isPlaying = false;
    bool canPlace = true;
    bool isWhite;

    int queenCount = 1;
    int antCount = 3;
    int grasshopperCount = 3;
    int beetleCount = 2;
    int spiderCount = 2;


    // Start is called before the first frame update
    void Start() {
        gameController = GameObject.FindWithTag("GameController");
        gameManager = gameController.GetComponent(typeof(GameManager)) as GameManager;

        photonView = GetComponent<PhotonView>();
    }

    void Update() {
        if (gameManager.GetRound() == 4 && !gameManager.AreQueensOnBoard()) {
            canPlace = false;
        }
        else {
            canPlace = true;
        }

        if (isPlaying) {
            //Selected Queen
            if (Input.GetKeyDown(KeyCode.Alpha1) && queenCount > 0 && gameManager.GetRound() != 1) {
                tile = Tiles[0];
                isMove = false;
                isTileSelected = true;
                queenCount--;
            }
            //Selected Ant
            else if (Input.GetKeyDown(KeyCode.Alpha2) && antCount > 0 && canPlace) {
                tile = Tiles[1];
                isMove = false;
                isTileSelected = true;
                antCount--;
            }
            //Selected Grasshopper
            else if (Input.GetKeyDown(KeyCode.Alpha3) && grasshopperCount > 0 && canPlace) {
                tile = Tiles[2];
                isMove = false;
                isTileSelected = true;
                grasshopperCount--;
            }
            //Selected Beetle
            else if (Input.GetKeyDown(KeyCode.Alpha4) && beetleCount > 0 && canPlace) {
                tile = Tiles[3];
                isMove = false;
                isTileSelected = true;
                beetleCount--;
            }
            //Selected Spider
            else if (Input.GetKeyDown(KeyCode.Alpha5) && spiderCount > 0 && canPlace) {
                tile = Tiles[4];
                isMove = false;
                isTileSelected = true;
                spiderCount--;
            }
        }
    }
    public IEnumerator Move(bool isPlaying) {
        if (isTurn) {
            Debug.Log(isWhite);

            this.isPlaying = isPlaying;
            gameManager.isPlaying = isPlaying;

            Debug.Log("Waiting For Tile Select");
            yield return new WaitWhile(IsTileSelected);
            Debug.Log("Tile Selected");

            if (!isFirstMove || gameManager.GetTurn() == 1) {
                gameManager.SetMoveGrid(tile, isMove);
            }
            else {
                isPosSelected = true;
                isFirstMove = false;
            }

            Debug.Log("Waiting for Pos Select");
            yield return new WaitWhile(IsPosSelected);
            Debug.Log("Pos Selected");

            if (gameManager.turn == 0) {
                Debug.Log("Copied piece values to P2");
                gameManager.p2.tile = tile;
                gameManager.p2.pos = pos;
                gameManager.p2.isMove = isMove;
            }
            else {
                Debug.Log("Copied piece values to P1");
                gameManager.p1.tile = tile;
                gameManager.p1.pos = pos;
                gameManager.p1.isMove = isMove;
            }

            photonView.RPC("SendMove", RpcTarget.All);
            isTileSelected = false;
            isPosSelected = false;
            this.isPlaying = false;
            gameManager.isPlaying = isPlaying;
        }
    }

    [PunRPC]
    private void SendMove() {
        Debug.Log("Move Sent");
        Debug.Log(isTurn);
        gameManager.NetworkMakeMove(tile, pos, isMove);
    }

    [PunRPC]
    private void MakeMove(GameObject tile, Vector3 pos, bool isMove) {
        gameManager.MakeMove(tile, pos, isMove);
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
        
        StartCoroutine(Move(true));
    }

    public void SetPos(Vector3 newPos) {
        pos = newPos;
        isPosSelected = true;
    }
}
