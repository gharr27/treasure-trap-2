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

        PhotonView photonView = this.GetComponent<PhotonView>();
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
                //photonView.RPC("SetGameTile", RpcTarget.All, 0);
                SetGameTile(0);
            }
            //Selected Ant
            else if (Input.GetKeyDown(KeyCode.Alpha2) && antCount > 0 && canPlace) {
                //photonView.RPC("SetGameTile", RpcTarget.All, 1);
                SetGameTile(1);
            }
            //Selected Grasshopper
            else if (Input.GetKeyDown(KeyCode.Alpha3) && grasshopperCount > 0 && canPlace) {
                //photonView.RPC("SetGameTile", RpcTarget.All, 2);
                SetGameTile(2);
            }
            //Selected Beetle
            else if (Input.GetKeyDown(KeyCode.Alpha4) && beetleCount > 0 && canPlace) {
                //photonView.RPC("SetGameTile", RpcTarget.All, 3);
                SetGameTile(3);
            }
            //Selected Spider
            else if (Input.GetKeyDown(KeyCode.Alpha5) && spiderCount > 0 && canPlace) {
                //photonView.RPC("SetGameTile", RpcTarget.All, 4);
                SetGameTile(4);
            }
        }
    }

    [PunRPC]
    void SetGameTile(int index) {
        tile = Tiles[index];
        isMove = false;
        isTileSelected = true;
        
        switch(index) {
            case 0:
                queenCount--;
                break;
            case 1:
                antCount--;
                break;
            case 2:
                grasshopperCount--;
                break;
            case 3:
                beetleCount--;
                break;
            case 4:
                spiderCount--;
                break;
        }
    }

    public IEnumerator Move(bool isPlaying) {
        if (isTurn) {
            Debug.Log(isWhite);

            this.isPlaying = isPlaying;
            //gameManager.isPlaying = isPlaying;

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

            //gameManager.NetWorkMakeMove(tile, pos, isMove);
            gameManager.MakeMove(tile, pos, isMove);
            isTileSelected = false;
            isPosSelected = false;
            this.isPlaying = false;
            //gameManager.isPlaying = isPlaying;
        }

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
