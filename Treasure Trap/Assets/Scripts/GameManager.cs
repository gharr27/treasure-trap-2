using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public TextMeshProUGUI p1QueenCount;
    public TextMeshProUGUI p1AntCount;
    public TextMeshProUGUI p1GrasshopperCount;
    public TextMeshProUGUI p1BeetleCount;
    public TextMeshProUGUI p1SpiderCount;

    public TextMeshProUGUI p2QueenCount;
    public TextMeshProUGUI p2AntCount;
    public TextMeshProUGUI p2GrasshopperCount;
    public TextMeshProUGUI p2BeetleCount;
    public TextMeshProUGUI p2SpiderCount;

    public GameObject WinnerScreenPanel;
    public GameObject LoserScreenPanel;

    public GameObject p1Image;
    public GameObject p2Image;

    public class GameGridCell {
        public bool isFilled;
        public GameObject tile;
        public bool isVisited;
        public bool isGate;

        public GameGridCell() {
            isFilled = false;
            tile = null;
            isVisited = false;
            isGate = false;
        }

        public GameGridCell(bool isFilled, GameObject tile) {
            this.isFilled = isFilled;
            this.tile = tile;
            isVisited = false;
            isGate = false;
        }
    }
    private class MovePosition {
        public bool isFilled;
        public Vector3 pos;

        public MovePosition() {
            isFilled = false;
            pos = Vector3.zero;
        }

        public MovePosition(bool isFilled, Vector3 pos) {
            this.isFilled = isFilled;
            this.pos = pos;
        }
    }

    public GameObject GridTile;
    public GameObject[] Players;
    public Button[] P1Buttons;
    public Button[] P2Buttons;
    public GameObject AI;
    public GameObject NetworkManager;

    const int PIECE_COUNT = 22;

    public int tilesPlaced = 0;
    public int turn = 0;
    public int round = 1;
    public bool isPlaying = false;
    public bool isAIPlaying = false;

    Vector3 selectedTilePos;

    private GameObject[] gamePieces;
    private Stack<GameObject> selectionGrids;

    public bool isNetworkGame = false;
    public bool isAIGame;

    public PlayerScript p1;
    public PlayerScript p2;
    public AI ai;

    GameObject playerObject1;
    GameObject playerObject2;

    PhotonView photonView;

    MenusManager menuManager;
    GameObject menuManagerObject;
    NetworkManager network;

    public bool isP1;

    public Dictionary<Vector3, GameGridCell> gameGrid = new Dictionary<Vector3, GameGridCell>();

    public int GetRound() {
        return round;
    }

    // Start is called before the first frame update
    void Start() {
        selectionGrids = new Stack<GameObject>();
        gamePieces = new GameObject[PIECE_COUNT];

        menuManagerObject = GameObject.FindWithTag("Menu");
        menuManager = menuManagerObject.GetComponent(typeof(MenusManager)) as MenusManager;

        p1Image.SetActive(true);
        p2Image.SetActive(false);

        //Network Game
        if (isNetworkGame) {
            Debug.Log("Network Game");
            photonView = GetComponent(typeof(PhotonView)) as PhotonView;
            network = NetworkManager.GetComponent(typeof(NetworkManager)) as NetworkManager;

            p1 = Players[0].GetComponent(typeof(PlayerScript)) as PlayerScript;
            p2 = Players[1].GetComponent(typeof(PlayerScript)) as PlayerScript;

            p1.color = "white";
            p2.color = "black";
            p1.isTurn = true;
            p2.isTurn = false;
            p1.isP1 = true;
            p2.isP1 = false;


            if (PhotonNetwork.IsMasterClient) {
                isP1 = true;
            }
            else {
                isP1 = false;
            }

            for (int i = 0; i < 5; i++) {
                if (isP1) {
                    P2Buttons[i].enabled = false;
                }
                else {
                    P1Buttons[i].enabled = false;
                }
            }
        }
        else if (isAIGame) { //Single Player Game
            Debug.Log("AI Game");
            playerObject1 = Players[0];
            playerObject2 = AI;

            p1 = playerObject1.GetComponent(typeof(PlayerScript)) as PlayerScript;
            ai = playerObject2.GetComponent(typeof(AI)) as AI;
            p1.isTurn = true;
            p1.color = "white";
            ai.isTurn = false;
            ai.color = "black";

            isP1 = true;
            p1.isP1 = true;
        }
        else {  //One Machine PVP Game
            Debug.Log("PVP Game");
            playerObject1 = Instantiate(Players[0], Vector3.zero, Quaternion.identity);
            playerObject2 = Instantiate(Players[1], Vector3.zero, Quaternion.identity);


            p1 = playerObject1.GetComponent(typeof(PlayerScript)) as PlayerScript;
            p2 = playerObject2.GetComponent(typeof(PlayerScript)) as PlayerScript;

            p1.isTurn = true;
            p2.isTurn = false;
        }

        UpdateGUITileCount();
    }

    public void SetSelectedTilePos(Vector3 newPos) {
        selectedTilePos = newPos;
    }

    public void SendTilePos(Vector3 newPos) {
        network.ReceiveTilePos(newPos);
    }

    void UpdateGUITileCount() {
        if (isNetworkGame) {
            p1QueenCount.text = "x" + p1.queenCount.ToString();
            p1AntCount.text = "x" + p1.antCount.ToString();
            p1GrasshopperCount.text = "x" + p1.grasshopperCount.ToString();
            p1BeetleCount.text = "x" + p1.beetleCount.ToString();
            p1SpiderCount.text = "x" + p1.spiderCount.ToString();

            p2QueenCount.text = "x" + p2.queenCount.ToString();
            p2AntCount.text = "x" + p2.antCount.ToString();
            p2GrasshopperCount.text = "x" + p2.grasshopperCount.ToString();
            p2BeetleCount.text = "x" + p2.beetleCount.ToString();
            p2SpiderCount.text = "x" + p2.spiderCount.ToString();
        }
        else if (isAIGame) {
            p1QueenCount.text = "x" + p1.queenCount.ToString();
            p1AntCount.text = "x" + p1.antCount.ToString();
            p1GrasshopperCount.text = "x" + p1.grasshopperCount.ToString();
            p1BeetleCount.text = "x" + p1.beetleCount.ToString();
            p1SpiderCount.text = "x" + p1.spiderCount.ToString();

            p2QueenCount.text = "x" + ai.queenCount.ToString();
            p2AntCount.text = "x" + ai.antCount.ToString();
            p2GrasshopperCount.text = "x" + ai.grasshopperCount.ToString();
            p2BeetleCount.text = "x" + ai.beetleCount.ToString();
            p2SpiderCount.text = "x" + ai.spiderCount.ToString();
        }
        else {

        }
    }

    public void MakeMove(GameObject tile, Vector3 pos, bool isMove) {

        if (isNetworkGame) {
            Vector3 tilePos = selectedTilePos;
            selectedTilePos = new Vector3();

            if (isMove) {
                tile = gameGrid[tilePos].tile;
                gameGrid[tile.transform.position] = new GameGridCell();
                tile.transform.position = pos;

                GameGridCell gridCell = new GameGridCell(true, tile);

                if (!gameGrid.ContainsKey(pos)) {
                    gameGrid.Add(pos, gridCell);
                }
                else {
                    gameGrid[pos] = gridCell;
                }
            }
            else {
                GameObject tilePiece = Instantiate(tile, pos, Quaternion.identity) as GameObject;
                gamePieces[tilesPlaced] = tilePiece;
                GameGridCell gridCell = new GameGridCell(true, tilePiece);

                TileScript tileScript = tilePiece.GetComponent(typeof(TileScript)) as TileScript;

                if (p1.isTurn) {
                    p1.DecrementTile(tileScript.tileName);
                }
                else {
                    p2.DecrementTile(tileScript.tileName);
                }

                if (gameGrid.ContainsKey(pos)) {
                    gameGrid[pos] = gridCell;
                }
                else {
                    gameGrid.Add(pos, gridCell);
                }
                tilesPlaced++;
            }

            UpdateGameGrid(pos);
            UpdateGUITileCount();
            UpdateTurn();

            ClearMoveGrid();
            CheckForWin();
        }
        else {
            if (p1.isTurn) {
                if (isMove) {
                    Debug.Log(tile.transform.position);
                    gameGrid[tile.transform.position] = new GameGridCell();
                    tile.transform.position = pos;
                    Debug.Log(tile.transform.position);

                    GameGridCell gridCell = new GameGridCell(true, tile);

                    if (!gameGrid.ContainsKey(pos)) {
                        gameGrid.Add(pos, gridCell);
                    }
                    else {
                        gameGrid[pos] = gridCell;
                    }
                }
                else {

                    GameObject tilePiece = Instantiate(tile, pos, Quaternion.identity) as GameObject;
                    gamePieces[tilesPlaced] = tilePiece;
                    GameGridCell gridCell = new GameGridCell(true, tilePiece);

                    TileScript tileScript = tilePiece.GetComponent(typeof(TileScript)) as TileScript;
                    p1.DecrementTile(tileScript.tileName);

                    if (gameGrid.ContainsKey(pos)) {
                        gameGrid[pos] = gridCell;
                    }
                    else {
                        gameGrid.Add(pos, gridCell);
                    }
                    tilesPlaced++;
                }

                UpdateGameGrid(pos);
                UpdateGUITileCount();
                UpdateTurn();

                ClearMoveGrid();
                CheckForWin();

                if (isAIGame) {
                    StartCoroutine(ai.Move(gameGrid, round, false));
                }
            }
        }
    }

    public void AIMove(GameObject tile, Vector3 pos, bool isMove) {
        if (ai.isTurn) {
            if (isMove) {
                gameGrid[tile.transform.position] = new GameGridCell();
                tile.transform.position = pos;

                GameGridCell gridCell = new GameGridCell(true, tile);

                if (!gameGrid.ContainsKey(pos)) {
                    gameGrid.Add(pos, gridCell);
                }
                else {
                    gameGrid[pos] = gridCell;
                }
            }
            else {
                GameObject tilePiece = Instantiate(tile, pos, Quaternion.identity) as GameObject;
                gamePieces[tilesPlaced] = tilePiece;
                GameGridCell gridCell = new GameGridCell(true, tilePiece);

                TileScript tileScript = tilePiece.GetComponent(typeof(TileScript)) as TileScript;
                ai.DecrementTile(tileScript.tileName);

                if (gameGrid.ContainsKey(pos)) {
                    gameGrid[pos] = gridCell;
                }
                else {
                    gameGrid.Add(pos, gridCell);
                }
                tilesPlaced++;
            }

            Debug.Log("position = " + pos);
            UpdateGameGrid(pos);
            UpdateGUITileCount();

            ClearMoveGrid();
            CheckForWin();

            UpdateTurn();
        }
    }

    void UpdateTurn() {
        if (isNetworkGame) {
            if (p1.isTurn) {
                p1.isTurn = false;
                p2.isTurn = true;
                turn = 1;
                p2Image.SetActive(p2.isTurn);
                p1Image.SetActive(p1.isTurn);
            }
            else {
                p1.isTurn = true;
                p2.isTurn = false;
                turn = 0;
                round++;
                p2Image.SetActive(p2.isTurn);
                p1Image.SetActive(p1.isTurn);
            }
        }
        else if (isAIGame) {
            if (p1.isTurn) {
                p1.isTurn = false;
                ai.isTurn = true;
                turn = 1;
                p2Image.SetActive(ai.isTurn);
                p1Image.SetActive(p1.isTurn);
            }
            else {
                p1.isTurn = true;
                ai.isTurn = false;
                turn = 0;
                round++;
                p2Image.SetActive(ai.isTurn);
                p1Image.SetActive(p1.isTurn);
            }
        }
    }

    void UpdateGameGrid(Vector3 pos) {
        float x = pos.x;
        float y = pos.y;
        float z = pos.z;

        GameGridCell gridCell = new GameGridCell();

        //On Top
        Vector3 newPos = new Vector3(x, y + 1, z);
        if (!gameGrid.ContainsKey(newPos)) {
            gameGrid.Add(newPos, gridCell);
        }

        //Above
        newPos = new Vector3(x + 1, y, z);
        if (!gameGrid.ContainsKey(newPos)) {
            gameGrid.Add(newPos, gridCell);
        }

        //Below
        newPos = new Vector3(x - 1, y, z);
        if (!gameGrid.ContainsKey(newPos)) {
            gameGrid.Add(newPos, gridCell);
        }

        //Top Left
        newPos = new Vector3(x + .5f, y, z + 1);
        if (!gameGrid.ContainsKey(newPos)) {
            gameGrid.Add(newPos, gridCell);
        }

        //Top Right
        newPos = new Vector3(x + .5f, y, z - 1);
        if (!gameGrid.ContainsKey(newPos)) {
            gameGrid.Add(newPos, gridCell);
        }

        //Bottom Left
        newPos = new Vector3(x - .5f, y, z + 1);
        if (!gameGrid.ContainsKey(newPos)) {
            gameGrid.Add(newPos, gridCell);
        }

        //Bottom Right
        newPos = new Vector3(x - .5f, y, z - 1);
        if (!gameGrid.ContainsKey(newPos)) {
            gameGrid.Add(newPos, gridCell);
        }

        //RemoveUnnecessaryGridSpaces();

    }

    // Checks the surrounding of the selected tile and if the position is filled then it adds a true to a list at that postion.
    public Stack<Vector3> ValidateMoves(GameObject tile) {
        TileScript tileScript = tile.GetComponent(typeof(TileScript)) as TileScript;
        Stack<Vector3> validMoves = new Stack<Vector3>();
        Vector3 pos = tile.transform.position;

        if (!IsBreaksHive(pos)) {
            if (tileScript.GetTileName() == "Queen") {
                validMoves = QueenPossibleMoves(tile);
            }
            else if (tileScript.GetTileName() == "Ant") {
                validMoves = AntPossibleMoves(tile);
            }
            else if (tileScript.GetTileName() == "Grasshopper") {
                validMoves = GrasshopperPossibleMoves(tile);
            }
            else if (tileScript.GetTileName() == "Beetle") {
                validMoves = BeetlePossibleMoves(tile);
            }
            else if (tileScript.GetTileName() == "Spider") {
                //validMoves = AntPossibleMoves(tile);
                validMoves = SpiderPossibleMoves(tile);
            }
        }

        return validMoves;
    }

    Stack<Vector3> GetEmptySpaces(Vector3 pos) {
        Stack<Vector3> emptySpaces = new Stack<Vector3>();

        float x = pos.x;
        float y = pos.y;
        float z = pos.z;

        //Above
        Vector3 newPos = new Vector3(x + 1, y, z);
        if (gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                emptySpaces.Push(newPos);
            }
        }

        //Below
        newPos = new Vector3(x - 1, y, z);
        if (gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                emptySpaces.Push(newPos);
            }
        }


        //Top Left
        newPos = new Vector3(x + .5f, y, z + 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                emptySpaces.Push(newPos);
            }
        }


        //Bottom Left
        newPos = new Vector3(x - .5f, y, z + 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                emptySpaces.Push(newPos);
            }
        }


        //Top Right
        newPos = new Vector3(x + .5f, y, z - 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                emptySpaces.Push(newPos);
            }
        }


        //Bottom Right
        newPos = new Vector3(x - .5f, y, z - 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                emptySpaces.Push(newPos);
            }
        }

        return emptySpaces;
    }


    //HAS TO BE MODIFIED, trying to check if each space is occupied
    //UPDATE: may not need this 
    Vector3 CheckForOccupied(Vector3 pos, bool isFirstTime, float xDir, float zDir) {
        float x = pos.x;
        float y = pos.y;
        float z = pos.z;


        Vector3 validPos = pos;

        Vector3 newPos = new Vector3(x + xDir, y, z + zDir);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled && isFirstTime) {
                return validPos;
            }
            else if (gameGrid[newPos].isFilled && !isFirstTime) {
                return newPos;
            }
            else {
                return CheckForOccupied(newPos, false, xDir, zDir);
            }
        }

        return validPos;
    }

    //takes a stack of occupied spaces, goes through the stack and checks to see if each piece has at least one or more pieces next to it
    //start from the position where the user wants to move the piece
    //if Hive is broken then the places visited will be less than the total amount of pieces on the board
    bool IsBreaksHive(Vector3 pos) {
        float x = pos.x;
        float y = pos.y;
        float z = pos.z;

        gameGrid[pos].isVisited = true;

        Vector3 startPos = new Vector3();

        Dictionary<Vector3, int> boarderTiles = GetBoarderTiles(pos);

        foreach (Vector3 boarderPos in boarderTiles.Keys) {
            startPos = boarderPos;
            break;
        }

        TraverseHive(startPos);

        int count = 0;

        foreach (GameGridCell gridCell in gameGrid.Values) {
            if (gridCell.isVisited) {
                count++;
                gridCell.isVisited = false;
            }
        }

        if (count != tilesPlaced) {
            return true;
        }

        return false;
    }

    void TraverseHive(Vector3 pos) {
        float x = pos.x;
        float y = pos.y;
        float z = pos.z;

        gameGrid[pos].isVisited = true;

        //Check all directions for filled tile
        //Check On Top
        Vector3 newPos = new Vector3(x, y + 1, z);
        if (gameGrid.ContainsKey(newPos)) {
            if(gameGrid[newPos].isFilled) {
                if (!gameGrid[newPos].isVisited) {
                    TraverseHive(newPos);
                }
            }
        }
        //check Above
        newPos = new Vector3(x + 1, y, z);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled) {
                if (!gameGrid[newPos].isVisited) {
                    TraverseHive(newPos);
                }
            }
        }

        //check below
        newPos = new Vector3(x - 1, y, z);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled) {
                if (!gameGrid[newPos].isVisited) {
                    TraverseHive(newPos);
                }
            }
        }

        //Top Left
        newPos = new Vector3(x + .5f, y, z + 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled) {
                if (!gameGrid[newPos].isVisited) {
                    TraverseHive(newPos);
                }
            }
        }

        //Top Right
        newPos = new Vector3(x + .5f, y, z - 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled) {
                if (!gameGrid[newPos].isVisited) {
                    TraverseHive(newPos);
                }
            }
        }

        //Bottom Left
        newPos = new Vector3(x - .5f, y, z + 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled) {
                if (!gameGrid[newPos].isVisited) {
                    TraverseHive(newPos);
                }
            }
        }

        //Bottom Right
        newPos = new Vector3(x - .5f, y, z - 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled) {
                if (!gameGrid[newPos].isVisited) {
                    TraverseHive(newPos);
                }
            }
        }

    }

    Dictionary<Vector3, int> GetBoarderTiles(Vector3 pos) {
        Dictionary<Vector3, int> ret = new Dictionary<Vector3, int>();

        float x = pos.x;
        float y = pos.y;
        float z = pos.z;

        //Above
        Vector3 newPos = new Vector3(x + 1, y, z);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled) {
                ret.Add(newPos, 69);
            }
        }

        //Below
        newPos = new Vector3(x - 1, y, z);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled) {
                ret.Add(newPos, 69);
            }
        }


        //Top Left
        newPos = new Vector3(x + .5f, y, z + 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled) {
                ret.Add(newPos, 69);
            }
        }


        //Bottom Left
        newPos = new Vector3(x - .5f, y, z + 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled) {
                ret.Add(newPos, 69);
            }
        }


        //Top Right
        newPos = new Vector3(x + .5f, y, z - 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled) {
                ret.Add(newPos, 69);
            }
        }


        //Bottom Right
        newPos = new Vector3(x - .5f, y, z - 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled) {
                ret.Add(newPos, 69);
            }
        }

        return ret;
    }

    bool CheckIfValid(Vector3 tilePos, Vector3 spacePos) {
        float x = spacePos.x;
        float y = spacePos.y;
        float z = spacePos.z;

        if (y == 0) {
            //Above
            Vector3 newPos = new Vector3(x + 1, y, z);
            if (gameGrid.ContainsKey(newPos)) {
                if (gameGrid[newPos].isFilled && newPos != tilePos) {
                    return true;
                }
            }

            //Below
            newPos = new Vector3(x - 1, y, z);
            if (gameGrid.ContainsKey(newPos)) {
                if (gameGrid[newPos].isFilled && newPos != tilePos) {
                    return true;
                }
            }


            //Top Left
            newPos = new Vector3(x + .5f, y, z + 1);
            if (gameGrid.ContainsKey(newPos)) {
                if (gameGrid[newPos].isFilled && newPos != tilePos) {
                    return true;
                }
            }


            //Bottom Left
            newPos = new Vector3(x - .5f, y, z + 1);
            if (gameGrid.ContainsKey(newPos)) {
                if (gameGrid[newPos].isFilled && newPos != tilePos) {
                    return true;
                }
            }


            //Top Right
            newPos = new Vector3(x + .5f, y, z - 1);
            if (gameGrid.ContainsKey(newPos)) {
                if (gameGrid[newPos].isFilled && newPos != tilePos) {
                    return true;
                }
            }


            //Bottom Right
            newPos = new Vector3(x - .5f, y, z - 1);
            if (gameGrid.ContainsKey(newPos)) {
                if (gameGrid[newPos].isFilled && newPos != tilePos) {
                    return true;
                }
            }
        }

        return false;
    }

    bool ValidatePosition(Dictionary<Vector3, int> boarderTiles, Dictionary<Vector3, int> spaceBoarderTiles) {
        foreach (Vector3 boarderTile in boarderTiles.Keys) {
            foreach (Vector3 spaceBoarderTile in spaceBoarderTiles.Keys) {
                if (boarderTile == spaceBoarderTile) {
                    return true;
                }
            }
        }

        return false;
    }

    void CheckForGates(Vector3 pos) {
        float x = pos.x;
        float y = pos.y;
        float z = pos.z;

        gameGrid[pos].isVisited = true;

        //Case:     1,0 : -.5, -1
        Vector3 newPos1 = new Vector3(x + 1, y, z);
        Vector3 newPos2 = new Vector3(x - .5f, y, z - 1);
        Vector3 emptyPos = new Vector3(x + .5f, y, z - 1);
        Debug.Log("test1");
        if (gameGrid.ContainsKey(newPos1) && gameGrid.ContainsKey(emptyPos) && gameGrid.ContainsKey(newPos2)) {
            Debug.Log("test2");
            Debug.Log(newPos1 + " " + newPos2);
            Debug.Log(gameGrid[newPos1].isFilled + " : " + gameGrid[newPos2].isFilled);
            if (gameGrid[newPos1].isFilled && !gameGrid[emptyPos].isFilled && gameGrid[newPos2].isFilled) {
                Debug.Log(emptyPos + " is Visited");
                gameGrid[emptyPos].isVisited = true;
                gameGrid[emptyPos].isGate = true;
            }
        }
        //Case:     .5, -1 : -1, 0
        newPos1 = new Vector3(x + .5f, y, z - 1);
        newPos2 = new Vector3(x - 1, y, z);
        emptyPos = new Vector3(x - .5f, y, z - 1);
        Debug.Log("test1");
        if (gameGrid.ContainsKey(newPos1) && gameGrid.ContainsKey(emptyPos) && gameGrid.ContainsKey(newPos2)) {
            Debug.Log("test2");
            Debug.Log(newPos1 + " " + newPos2);
            Debug.Log(gameGrid[newPos1].isFilled + " : " + gameGrid[newPos2].isFilled);
            if (gameGrid[newPos1].isFilled && !gameGrid[emptyPos].isFilled && gameGrid[newPos2].isFilled) {
                Debug.Log(emptyPos + " is Visited");
                gameGrid[emptyPos].isVisited = true;
                gameGrid[emptyPos].isGate = true;
            }
        }
        //Case:     .-5, -1 : -.5, 1
        newPos1 = new Vector3(x - .5f, y, z - 1);
        newPos2 = new Vector3(x - .5f, y, z + 1);
        emptyPos = new Vector3(x - 1, y, z);
        Debug.Log("test1");
        if (gameGrid.ContainsKey(newPos1) && gameGrid.ContainsKey(emptyPos) && gameGrid.ContainsKey(newPos2)) {
            Debug.Log("test2");
            Debug.Log(newPos1 + " " + newPos2);
            Debug.Log(gameGrid[newPos1].isFilled + " : " + gameGrid[newPos2].isFilled);
            if (gameGrid[newPos1].isFilled && !gameGrid[emptyPos].isFilled && gameGrid[newPos2].isFilled) {
                Debug.Log(emptyPos + " is Visited");
                gameGrid[emptyPos].isVisited = true;
                gameGrid[emptyPos].isGate = true;
            }
        }
        //Case:     -1, 0 : .5, 1
        newPos1 = new Vector3(x - 1, y, z);
        newPos2 = new Vector3(x + .5f, y, z + 1);
        emptyPos = new Vector3(x - .5f, y, z + 1);
        Debug.Log("test1");
        if (gameGrid.ContainsKey(newPos1) && gameGrid.ContainsKey(emptyPos) && gameGrid.ContainsKey(newPos2)) {
            Debug.Log("test2");
            Debug.Log(newPos1 + " " + newPos2);
            Debug.Log(gameGrid[newPos1].isFilled + " : " + gameGrid[newPos2].isFilled);
            if (gameGrid[newPos1].isFilled && !gameGrid[emptyPos].isFilled && gameGrid[newPos2].isFilled) {
                Debug.Log(emptyPos + " is Visited");
                gameGrid[emptyPos].isVisited = true;
                gameGrid[emptyPos].isGate = true;
            }
        }
        //Case:     -.5, 1  :   1, 0
        newPos1 = new Vector3(x - .5f, y, z + 1);
        newPos2 = new Vector3(x + 1, y, z);
        emptyPos = new Vector3(x + .5f, y, z + 1);
        Debug.Log("test1");
        if (gameGrid.ContainsKey(newPos1) && gameGrid.ContainsKey(emptyPos) && gameGrid.ContainsKey(newPos2)) {
            Debug.Log("test2");
            Debug.Log(newPos1 + " " + newPos2);
            Debug.Log(gameGrid[newPos1].isFilled + " : " + gameGrid[newPos2].isFilled);
            if (gameGrid[newPos1].isFilled && !gameGrid[emptyPos].isFilled && gameGrid[newPos2].isFilled) {
                Debug.Log(emptyPos + " is Visited");
                gameGrid[emptyPos].isVisited = true;
                gameGrid[emptyPos].isGate = true;
            }
        }
        //Case:     .5,1 : .5,-1
        newPos1 = new Vector3(x + .5f, y, z + 1);
        newPos2 = new Vector3(x + .5f, y, z - 1);
        emptyPos = new Vector3(x + 1, y, z);
        Debug.Log("test1");
        if (gameGrid.ContainsKey(newPos1) && gameGrid.ContainsKey(emptyPos) && gameGrid.ContainsKey(newPos2)) {
            Debug.Log("test2");
            Debug.Log(newPos1 + " " + newPos2);
            Debug.Log(gameGrid[newPos1].isFilled + " : " + gameGrid[newPos2].isFilled);
            if (gameGrid[newPos1].isFilled && !gameGrid[emptyPos].isFilled && gameGrid[newPos2].isFilled) {
                Debug.Log(emptyPos + " is Visited");
                gameGrid[emptyPos].isVisited = true;
                gameGrid[emptyPos].isGate = true;
            }
        }
    }

    void TraverseEmptySpaces(Vector3 pos) {
        float x = pos.x;
        float y = pos.y;
        float z = pos.z;

        if (gameGrid.ContainsKey(pos)) {
            gameGrid[pos].isVisited = true;
        }

        //check Above
        Vector3 newPos = new Vector3(x + 1, y, z);
        if (gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                if (!gameGrid[newPos].isVisited) {
                    CheckForGates(pos);

                    if (!gameGrid[newPos].isGate) {
                        TraverseEmptySpaces(newPos);
                    }
                }
            }
        }

        //check below
        newPos = new Vector3(x - 1, y, z);
        if (gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                if (!gameGrid[newPos].isVisited) {
                    CheckForGates(pos);
                }
            }
        }

        //Top Left
        newPos = new Vector3(x + .5f, y, z + 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                if (!gameGrid[newPos].isVisited) {
                    CheckForGates(pos);

                    if (!gameGrid[newPos].isGate) {
                        TraverseEmptySpaces(newPos);
                    }
                }
            }
        }

        //Top Right
        newPos = new Vector3(x + .5f, y, z - 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                if (!gameGrid[newPos].isVisited) {
                    CheckForGates(pos);

                    if (!gameGrid[newPos].isGate) {
                        TraverseEmptySpaces(newPos);
                    }
                }
            }
        }

        //Bottom Left
        newPos = new Vector3(x - .5f, y, z + 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                if (!gameGrid[newPos].isVisited) {
                    CheckForGates(pos);

                    if (!gameGrid[newPos].isGate) {
                        TraverseEmptySpaces(newPos);
                    }
                }
            }
        }

        //Bottom Right
        newPos = new Vector3(x - .5f, y, z - 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                if (!gameGrid[newPos].isVisited) {
                    CheckForGates(pos);

                    if (!gameGrid[newPos].isGate) {
                        TraverseEmptySpaces(newPos);
                    }
                }
            }
        }
    }

    /* Shows available moves for the queen*/
    Stack<Vector3> QueenPossibleMoves(GameObject tile) {
        Stack<Vector3> validMovePositions = new Stack<Vector3>();
        Vector3 tilePos = tile.transform.position;
        Dictionary<Vector3, int> boarderTiles = GetBoarderTiles(tilePos);
        Stack<Vector3> emptySpaces = GetEmptySpaces(tilePos);

        //Check Sides
        while (emptySpaces.Count > 0) {
            Vector3 pos = emptySpaces.Pop();

            Dictionary<Vector3, int> spaceBoarderTiles = GetBoarderTiles(pos);

            if (CheckIfValid(tilePos, pos) && ValidatePosition(boarderTiles, spaceBoarderTiles)) {
                validMovePositions.Push(pos);
            }

        }
        return validMovePositions;
    }

    Stack<Vector3> AntPossibleMoves(GameObject tile) {
        Stack<Vector3> validMovePositions = new Stack<Vector3>();
        Vector3 tilePos = tile.transform.position;

        //Check Sides
        foreach (KeyValuePair<Vector3, GameGridCell> gridCell in gameGrid) {
            if (gridCell.Key.y == 0) {
                if (!gridCell.Value.isFilled && CheckIfValid(tilePos, gridCell.Key)) {
                    validMovePositions.Push(gridCell.Key);
                }
            }
        }

        return validMovePositions;
    }

    Stack<Vector3> GrasshopperPossibleMoves(GameObject tile) {
        Stack<Vector3> validMovePositions = new Stack<Vector3>();

        Vector3 pos = tile.transform.position;

        float x = tile.transform.position.x;
        float y = tile.transform.position.y;
        float z = tile.transform.position.z;

        //Above
        Vector3 newPos = CheckForEmpty(pos, true, 1, 0);
        if (newPos != pos) {
            validMovePositions.Push(newPos);
        }

        //Below
        newPos = CheckForEmpty(pos, true, -1, 0);
        if (newPos != pos) {
            validMovePositions.Push(newPos);
        }


        //Top Left
        newPos = CheckForEmpty(pos, true, .5f, 1);
        if (newPos != pos) {
            validMovePositions.Push(newPos);
        }


        //Bottom Left
        newPos = CheckForEmpty(pos, true, -.5f, 1);
        if (newPos != pos) {
            validMovePositions.Push(newPos);
        }


        //Top Right
        newPos = CheckForEmpty(pos, true, .5f, -1);
        if (newPos != pos) {
            validMovePositions.Push(newPos);
        }


        //Bottom Right
        newPos = CheckForEmpty(pos, true, -.5f, -1);
        if (newPos != pos) {
            validMovePositions.Push(newPos);
        }

        return validMovePositions;

    }

    Stack<Vector3> BeetlePossibleMoves(GameObject tile) {
        Stack<Vector3> validMovePositions = new Stack<Vector3>();
        Vector3 tilePos = tile.transform.position;

        if (tilePos.y > 0) {
            tilePos.y = 0;
        }

        Dictionary<Vector3, int> boarderTiles = GetBoarderTiles(tilePos);
        Stack<Vector3> emptySpaces = GetEmptySpaces(tilePos);

        //Check Sides
        while (emptySpaces.Count > 0) {
            Vector3 pos = emptySpaces.Pop();

            Dictionary<Vector3, int> spaceBoarderTiles = GetBoarderTiles(pos);

            if (CheckIfValid(tilePos, pos) && ValidatePosition(boarderTiles, spaceBoarderTiles)) {
                validMovePositions.Push(pos);
            }

            //Check for On Top
            foreach (Vector3 boarderTile in boarderTiles.Keys) {
                float x = boarderTile.x;
                float y = boarderTile.y + 1;
                float z = boarderTile.z;

                while (gameGrid[new Vector3(x, y, z)].isFilled) {
                    y++;
                }

                validMovePositions.Push(new Vector3(x, y, z));
            }
        }

        return validMovePositions;
    }

    Stack<Vector3> SpiderPossibleMoves(GameObject tile) {
        Stack<Vector3> validMovePositions = new Stack<Vector3>();
        Vector3 tilePos = tile.transform.position;
        Dictionary<Vector3, int> boarderTiles = GetBoarderTiles(tilePos);
        Stack<Vector3> emptySpaces = GetEmptySpaces(tilePos);

        Stack<Vector3> firstMoves = new Stack<Vector3>();
        Stack<Vector3> secondMoves = new Stack<Vector3>();
        Dictionary<Vector3, int> spacesVisited = new Dictionary<Vector3, int>();

        //Check Sides
        while (emptySpaces.Count > 0) {
            Vector3 pos = emptySpaces.Pop();

            Dictionary<Vector3, int> spaceBoarderTiles = GetBoarderTiles(pos);


            if (CheckIfValid(tilePos, pos) && ValidatePosition(boarderTiles, spaceBoarderTiles)) {
                firstMoves.Push(pos);
            }
        }

        while (firstMoves.Count > 0) {
            boarderTiles = GetBoarderTiles(firstMoves.Peek());
            spacesVisited.Add(firstMoves.Peek(), 7);

            emptySpaces = GetEmptySpaces(firstMoves.Pop());

            while (emptySpaces.Count > 0) {
                Vector3 pos = emptySpaces.Pop();

                Dictionary<Vector3, int> spaceBoarderTiles = GetBoarderTiles(pos);


                if (CheckIfValid(tilePos, pos) && ValidatePosition(boarderTiles, spaceBoarderTiles)) {
                    secondMoves.Push(pos);
                }
            }
        }
        while (secondMoves.Count > 0) {
            boarderTiles = GetBoarderTiles(secondMoves.Peek());

            emptySpaces = GetEmptySpaces(secondMoves.Pop());

            while (emptySpaces.Count > 0) {
                bool isValid = true;
                Vector3 pos = emptySpaces.Pop();

                foreach (Vector3 space in spacesVisited.Keys) {
                    if (pos == space) {
                        isValid = false;
                    }
                }
                Dictionary<Vector3, int> spaceBoarderTiles = GetBoarderTiles(pos);

                if (CheckIfValid(tilePos, pos) && ValidatePosition(boarderTiles, spaceBoarderTiles) && isValid) {
                    validMovePositions.Push(pos);
                }
            }
        }
        return validMovePositions;
    }

    Vector3 CheckForEmpty(Vector3 pos, bool isFirstTime, float xDir, float zDir) {
        float x = pos.x;
        float y = pos.y;
        float z = pos.z;


        Vector3 validPos = pos;

        Vector3 newPos = new Vector3(x + xDir, y, z + zDir);
        if (gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled && isFirstTime) {
                return validPos;
            }
            else if (!gameGrid[newPos].isFilled && !isFirstTime) {
                return newPos;
            }
            else {
                return CheckForEmpty(newPos, false, xDir, zDir);
            }
        }

        return validPos;
    }

    void CheckForWin() {

        foreach (GameGridCell tile in gameGrid.Values) {
            if (tile.isFilled) {
                Vector3 pos = tile.tile.transform.position;

                TileScript tileScript = tile.tile.GetComponent(typeof(TileScript)) as TileScript;

                //Check if Queen is surrounded
                if (tileScript.GetTileName() == "Queen") {
                    if (tileScript.GetTileColor() == p1.color) {
                        //Player Lose
                        if (IsSurrounded(pos)) {
                            if (isP1) {
                                GameOver(false);
                            }
                            else {
                                GameOver(true);
                            }
                        }
                }
                    else {
                        //Player Win
                        if (IsSurrounded(pos)) {
                            if (isP1) {
                                GameOver(true);
                            }
                            else {
                                GameOver(false);
                            }
                        }
                    }
                }
            }
        }
    }

    void GameOver(bool playerWin) {
        if (isAIGame){
            if (playerWin) {
                //Player Win
                menuManager.GoToWinnerScreenAI();
            }
            else {
                //Player Lose
                menuManager.GoToLoserScreen();
                
            }
        }
        else if(isNetworkGame){
            if (playerWin) {
                //Player Win
                // chatManager.Leave();
                WinnerScreenPanel.SetActive(true);
            }
            else {
                //Player Lose
                // chatManager.Leave();
                LoserScreenPanel.SetActive(true);
                
            }
        }
    }

    bool IsSurrounded(Vector3 pos) {
        float x = pos.x;
        float y = pos.y;
        float z = pos.z;

        //Above
        Vector3 newPos = new Vector3(x + 1, y, z);
        if (gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                return false;
            }
        }

        //Below
        newPos = new Vector3(x - 1, y, z);
        if (gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                return false;
            }
        }

        //Top Left
        newPos = new Vector3(x + .5f, y, z + 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                return false;
            }
        }

        //Top Right
        newPos = new Vector3(x + .5f, y, z - 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                return false;
            }
        }

        //Bottom Left
        newPos = new Vector3(x - .5f, y, z + 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                return false;
            }
        }

        //Bottom Right
        newPos = new Vector3(x - .5f, y, z - 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                return false;
            }
        }

        return true;
    }

    //Returns a vector of all the valid placement positions
    public Stack<Vector3> GetPlacePositions(bool isWhite) {
        Stack<Vector3> positions = new Stack<Vector3>();
        Stack<Vector3> ret = new Stack<Vector3>();
        Dictionary<Vector3, int> invalidPos = new Dictionary<Vector3, int>();

        //Checks for the open positions around a tile for creating potential move positions

        for (int i = 0; gamePieces[i] != null && i < tilesPlaced; i++) {
            bool canPlace = true;

            TileScript tileScript = gamePieces[i].GetComponent(typeof(TileScript)) as TileScript;
            if (round != 1) {
                if (isWhite) {
                    if (tileScript.GetTileColor() == "black") {
                        canPlace = false;
                    }
                }
                else {
                    if (tileScript.GetTileColor() == "white") {
                        canPlace = false;
                    }
                }
            }

            if (canPlace) {
                float x = gamePieces[i].transform.position.x;
                float z = gamePieces[i].transform.position.z;

                Vector3 pos;

                //North of Tile
                pos = new Vector3(x + 1, 0, z);
                if (gameGrid.ContainsKey(pos)) {
                    if (!gameGrid[pos].isFilled) {
                        positions.Push(pos);
                    }
                }
                else {
                    gameGrid.Add(pos, new GameGridCell());
                    positions.Push(pos);
                }

                //South of Tile
                pos = new Vector3(x - 1, 0, z);
                if (gameGrid.ContainsKey(pos)) {
                    if (!gameGrid[pos].isFilled) {
                        positions.Push(pos);
                    }
                }
                else {
                    gameGrid.Add(pos, new GameGridCell());
                    positions.Push(pos);
                }

                //North East of Tile
                pos = new Vector3(x + .5f, 0, z + 1);
                if (gameGrid.ContainsKey(pos)) {
                    if (!gameGrid[pos].isFilled) {
                        positions.Push(pos);
                    }
                }
                else {
                    gameGrid.Add(pos, new GameGridCell());
                    positions.Push(pos);
                }

                //North West of Tile
                pos = new Vector3(x + .5f, 0, z - 1);
                if (gameGrid.ContainsKey(pos)) {
                    if (!gameGrid[pos].isFilled) {
                        positions.Push(pos);
                    }
                }
                else {
                    gameGrid.Add(pos, new GameGridCell());
                    positions.Push(pos);
                }

                //South East of Tile
                pos = new Vector3(x - .5f, 0, z + 1);
                if (gameGrid.ContainsKey(pos)) {
                    if (!gameGrid[pos].isFilled) {
                        positions.Push(pos);
                    }
                }
                else {
                    gameGrid.Add(pos, new GameGridCell());
                    positions.Push(pos);
                }

                //South West of Tile
                pos = new Vector3(x - .5f, 0, z - 1);
                if (gameGrid.ContainsKey(pos)) {
                    if (!gameGrid[pos].isFilled) {
                        positions.Push(pos);
                    }
                }
                else {
                    gameGrid.Add(pos, new GameGridCell());
                    positions.Push(pos);
                }
            }
            else {
                float x = gamePieces[i].transform.position.x;
                float z = gamePieces[i].transform.position.z;

                Vector3 pos;

                //North of Tile

                pos = new Vector3(x + 1, 0, z);
                if (gameGrid.ContainsKey(pos)) {
                    if (!gameGrid[pos].isFilled) {
                        invalidPos[pos] = 69;
                    }
                }
                else {
                    gameGrid.Add(pos, new GameGridCell());
                    invalidPos[pos] = 69;
                }

                //South of Tile
                pos = new Vector3(x - 1, 0, z);
                if (gameGrid.ContainsKey(pos)) {
                    if (!gameGrid[pos].isFilled) {
                        invalidPos[pos] = 69;
                    }
                }
                else {
                    gameGrid.Add(pos, new GameGridCell());
                    invalidPos[pos] = 69;
                }

                //North East of Tile
                pos = new Vector3(x + .5f, 0, z + 1);
                if (gameGrid.ContainsKey(pos)) {
                    if (!gameGrid[pos].isFilled) {
                        invalidPos[pos] = 69;
                    }
                }
                else {
                    gameGrid.Add(pos, new GameGridCell());
                    invalidPos[pos] = 69;
                }

                //North West of Tile
                pos = new Vector3(x + .5f, 0, z - 1);
                if (gameGrid.ContainsKey(pos)) {
                    if (!gameGrid[pos].isFilled) {
                        invalidPos[pos] = 69;
                    }
                }
                else {
                    gameGrid.Add(pos, new GameGridCell());
                    invalidPos[pos] = 69;
                }

                //South East of Tile
                pos = new Vector3(x - .5f, 0, z + 1);
                if (gameGrid.ContainsKey(pos)) {
                    if (!gameGrid[pos].isFilled) {
                        invalidPos[pos] = 69;
                    }
                }
                else {
                    gameGrid.Add(pos, new GameGridCell());
                    invalidPos[pos] = 69;
                }

                //South West of Tile
                pos = new Vector3(x - .5f, 0, z - 1);
                if (gameGrid.ContainsKey(pos)) {
                    if (!gameGrid[pos].isFilled) {
                        invalidPos[pos] = 69;
                    }
                }
                else {
                    gameGrid.Add(pos, new GameGridCell());
                    invalidPos[pos] = 69;
                }
            }
        }

        while (positions.Count > 0) {
            bool isValid = true;
            Vector3 pos = positions.Pop();

            foreach (KeyValuePair<Vector3, int> badPos in invalidPos) {
                if (pos == badPos.Key) {
                    isValid = false;
                }
            }

            if (isValid) {
                ret.Push(pos);
            }
        }

        return ret;
    }

    int GetBoarderCount(Vector3 pos, GameObject tile) {
        int count = 0;

        float x = pos.x;
        float y = pos.y;
        float z = pos.z;

        //Above
        Vector3 newPos = new Vector3(x + 1, y, z);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled && gameGrid[newPos].tile != tile) {
                count++;
            }
        }
        else {
            Debug.Log("no key");
        }

        //Below
        newPos = new Vector3(x - 1, y, z);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled && gameGrid[newPos].tile != tile) {
                count++;
            }
        }
        else {
            Debug.Log("no key");
        }

        //Top Left
        newPos = new Vector3(x + .5f, y, z + 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled && gameGrid[newPos].tile != tile) {
                count++;
            }
        }
        else {
            Debug.Log("no key");
        }

        //Top Right
        newPos = new Vector3(x + .5f, y, z - 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled && gameGrid[newPos].tile != tile) {
                count++;
            }
        }
        else {
            Debug.Log("no key");
        }

        //Bottom Left
        newPos = new Vector3(x - .5f, y, z + 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled && gameGrid[newPos].tile != tile) {
                count++;
            }
        }
        else {
            Debug.Log("no key");
        }

        //Bottom Right
        newPos = new Vector3(x - .5f, y, z - 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled && gameGrid[newPos].tile != tile) {
                count++;
            }
        }
        else {
            Debug.Log("no key");
        }

        return count;
    }

    //Creates the selection grid at valid move positions
    public void SetMoveGrid(GameObject tile, bool isMove) {

        bool isWhite;

        if (p1.isTurn) {
            isWhite = true;
        }
        else {
            isWhite = false;
        }
        Debug.Log(tilesPlaced);
        if (tilesPlaced > 0) {
            ClearMoveGrid();
            Debug.Log(isMove);
            if (isMove) {
                if (!IsBreaksHive(tile.transform.position)) {
                    Stack<Vector3> positions = ValidateMoves(tile);
                    Stack<Vector3> tempPositions = ValidateMoves(tile);
                    Stack<Vector3> badPositions = new Stack<Vector3>();
                    Stack<Vector3> validPositons = new Stack<Vector3>();

                    //CheckForGates(tempPositions);

                    //foreach (KeyValuePair<Vector3, GameGridCell> gridCell in gameGrid) {
                    //    if (gridCell.Value.isVisited) {
                    //        Debug.Log("Grid Piece: " + gridCell.Key);
                    //        badPositions.Push(gridCell.Key);
                    //        gridCell.Value.isVisited = false;
                    //    }
                    //}

                    //Debug.Log(badPositions.Count);
                    //Debug.Log(positions.Count);

                    //while (positions.Count > 0) {
                    //    Vector3 pos = positions.Pop();
                    //    bool isValid = true;

                    //    foreach (Vector3 badPos in badPositions) {
                    //        Debug.Log("Bad Pos: " + badPos);
                    //        if (pos == badPos) {
                    //            Debug.Log(pos + " : " + badPos);
                    //            isValid = false;
                    //            break;
                    //        }
                    //    }

                    //    if (isValid) {
                    //        validPositons.Push(pos);
                    //    }
                    //}

                    //Debug.Log(validPositons.Count);

                    validPositons = positions;
                    while (validPositons.Count > 0) {
                        if (GetBoarderCount(validPositons.Peek(), tile) < 5 || tile.transform.position.y > 0) {
                            GameObject grid;
                            grid = Instantiate(GridTile, validPositons.Pop(), Quaternion.identity) as GameObject;
                            selectionGrids.Push(grid);
                        }
                        else {
                            validPositons.Pop();
                        }
                    }
                }
            }
            else {
                //Place Validation
                Stack<Vector3> positions = GetPlacePositions(isWhite);

                while (positions.Count > 0) {
                    GameObject temp;
                    temp = Instantiate(GridTile, positions.Pop(), Quaternion.identity) as GameObject;
                    selectionGrids.Push(temp);
                }
            }
        }
    }

    //Deletes the selection grid after piece has been placed
    void ClearMoveGrid() {
        while (selectionGrids.Count > 0) {
            GameObject temp = selectionGrids.Pop();
            Destroy(temp);
        }
    }

    public void ReceiveMoveString(string tileName, string tileColor, string tilePosX, string tilePosY, string tilePosZ, string moveType) {
        Debug.Log("GameCore received: " + tileName + " " + tileColor + " " + tilePosX + " " + tilePosY + " " + tilePosZ + " " + moveType);

        GameObject tile = new GameObject();
        Vector3 pos;
        bool isMove;

        //Translate to move and make move
        if (tileColor == "white") {
            switch (tileName) {
                case "Queen":
                    tile = p1.Tiles[0];
                    break;
                case "Ant":
                    tile = p1.Tiles[1];
                    break;
                case "Grasshopper":
                    tile = p1.Tiles[2];
                    break;
                case "Beetle":
                    tile = p1.Tiles[3];
                    break;
                case "Spider":
                    tile = p1.Tiles[4];
                    break;
            }
        }
        else {
            switch (tileName) {
                case "Queen":
                    tile = p2.Tiles[0];
                    break;
                case "Ant":
                    tile = p2.Tiles[1];
                    break;
                case "Grasshopper":
                    tile = p2.Tiles[2];
                    break;
                case "Beetle":
                    tile = p2.Tiles[3];
                    break;
                case "Spider":
                    tile = p2.Tiles[4];
                    break;
            }
        }
      

        pos.x = float.Parse(tilePosX);
        pos.y = float.Parse(tilePosY);
        pos.z = float.Parse(tilePosZ);

        if (moveType == "move") {
            isMove = true;
        }
        else {
            isMove = false;
        }

        MakeMove(tile, pos, isMove);
    }

    public void SendMove(GameObject tile, Vector3 pos, bool isMove, string tileColor) {
        TileScript tileScript = tile.GetComponent(typeof(TileScript)) as TileScript;
        string tileName = tileScript.tileName;

        string tilePosX = pos.x.ToString();
        string tilePosY = pos.y.ToString();
        string tilePosZ = pos.z.ToString();

        string moveType;

        if (isMove) {
            moveType = "move";
        }
        else {
            moveType = "place";
        }

        network.ReceiveMoveString(tileName, tileColor, tilePosX, tilePosY, tilePosZ, moveType);
    }
}