using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

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

    public class GameGridCell {
        public bool isFilled;
        public GameObject tile;

        public GameGridCell() {
            isFilled = false;
            tile = null;
        }

        public GameGridCell(bool isFilled, GameObject tile) {
            this.isFilled = isFilled;
            this.tile = tile;
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
    public GameObject AI;

    const int PIECE_COUNT = 22;

    public int tilesPlaced = 0;
    public int turn = 0;
    public int round = 1;
    public bool isPlaying = false;
    public bool isAIPlaying = false;

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

        //Network Game
        if (isNetworkGame) {
            Debug.Log("Network Game");
            playerObject1 = PhotonNetwork.Instantiate(Players[0].name, Vector3.zero, Quaternion.identity);
            playerObject2 = PhotonNetwork.Instantiate(Players[1].name, Vector3.zero, Quaternion.identity);
            photonView = GetComponent<PhotonView>();

            //Is Host
            if (PhotonNetwork.IsMasterClient) {
                Debug.Log("P1");
            }
            else {
                Debug.Log("P2");
            }

            p1 = playerObject1.GetComponent(typeof(PlayerScript)) as PlayerScript;
            p1.color = "white";

            p2 = playerObject2.GetComponent(typeof(PlayerScript)) as PlayerScript;
            p2.color = "black";

            p1.isTurn = true;
            p2.isTurn = false;
        }
        else if (isAIGame) { //Single Player Game
            Debug.Log("AI Game");
            playerObject1 = Players[0];
            playerObject2 = AI;

            p1 = playerObject1.GetComponent(typeof(PlayerScript)) as PlayerScript;
            ai = playerObject2.GetComponent(typeof(AI)) as AI;
            p1.isTurn = true;
            ai.isTurn = false;
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

    void UpdateGUITileCount() {
        if (isNetworkGame) {
            p1QueenCount.text = "Queen" + p1.queenCount.ToString();
            p1AntCount.text = "Ant" + p1.antCount.ToString();
            p1GrasshopperCount.text = "Grass" + p1.grasshopperCount.ToString();
            p1BeetleCount.text = "Beetle" + p1.beetleCount.ToString();
            p1SpiderCount.text = "Spider" + p1.spiderCount.ToString();

            p2QueenCount.text = "Queen" + p2.queenCount.ToString();
            p2AntCount.text = "Ant" + p2.antCount.ToString();
            p2GrasshopperCount.text = "grass" + p2.grasshopperCount.ToString();
            p2BeetleCount.text = "Beetle" + p2.beetleCount.ToString();
            p2SpiderCount.text = "Spider" + p2.spiderCount.ToString();
        }
        else if (isAIGame) {
            p1QueenCount.text = "Queen" + p1.queenCount.ToString();
            p1AntCount.text = "Ant" + p1.antCount.ToString();
            p1GrasshopperCount.text = "Grass" + p1.grasshopperCount.ToString();
            p1BeetleCount.text = "Beetle" + p1.beetleCount.ToString();
            p1SpiderCount.text = "Spider" + p1.spiderCount.ToString();

            p2QueenCount.text = "Queen" + ai.queenCount.ToString();
            p2AntCount.text = "Ant" + ai.antCount.ToString();
            p2GrasshopperCount.text = "grass" + ai.grasshopperCount.ToString();
            p2BeetleCount.text = "Beetle" + ai.beetleCount.ToString();
            p2SpiderCount.text = "Spider" + ai.spiderCount.ToString();
        }
        else {

        }
    }

    public void Move(GameObject tile, Vector3 pos, bool isMove) {
        photonView.RPC("NetworkMakeMove", RpcTarget.All, tile, pos, isMove);
    }

    [PunRPC]
    public void NetworkMakeMove(GameObject tile, Vector3 pos, bool isMove) {
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
            GameObject tilePiece = PhotonNetwork.Instantiate(tile.name, pos, Quaternion.identity);
            photonView.RPC("AddToGamePieces", RpcTarget.All, tilePiece);
            GameGridCell gridCell = new GameGridCell(true, tilePiece);

            if (gameGrid.ContainsKey(pos)) {
                gameGrid[pos] = gridCell;
            }
            else {
                gameGrid.Add(pos, gridCell);
            }
            photonView.RPC("IncrementTilesPlaced", RpcTarget.All, 1);
        }

        UpdateGameGrid(pos);

        ClearMoveGrid();
        //CheckForWin();

    }

    public void MakeMove(GameObject tile, Vector3 pos, bool isMove) {
        if (p1.isTurn) {
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
            CheckForWin(true);

            if (isAIGame) {
                ai.Move(gameGrid, round, false);
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
                TileScript tileScript = tilePiece.GetComponent(typeof(TileScript)) as TileScript;

                ai.DecrementTile(tileScript.tileName);

                gamePieces[tilesPlaced] = tilePiece;
                GameGridCell gridCell = new GameGridCell(true, tilePiece);

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

            ClearMoveGrid();
            CheckForWin(false);

            UpdateTurn();
        }
    }

    void UpdateTurn() {
        if (isNetworkGame) {
            if (p1.isTurn) {
                p1.isTurn = false;
                p2.isTurn = true;
                turn = 1;
            }
            else {
                p1.isTurn = true;
                p2.isTurn = false;
                turn = 0;
                round++;
            }
        }
        else if (isAIGame) {
            if (p1.isTurn) {
                p1.isTurn = false;
                ai.isTurn = true;
                turn = 1;
            }
            else {
                p1.isTurn = true;
                ai.isTurn = false;
                turn = 0;
                round++;
            }
        }
        else {

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

    //Currently Unused, if we can get it working this function will deleted spaces that are not directly connected to the game tiles on the grid
    void RemoveUnnecessaryGridSpaces() {

        foreach (Vector3 key in gameGrid.Keys) {
            float x = key.x;
            float y = key.y;
            float z = key.z;

            bool isNecessary = false;

            if (!gameGrid[key].isFilled) {

                //Above
                Vector3 newPos = new Vector3(x + 1, y, z);
                if (gameGrid.ContainsKey(newPos) && !isNecessary) {
                    if (gameGrid[newPos].isFilled) {
                        isNecessary = true;
                    }
                }

                //Below
                newPos = new Vector3(x - 1, y, z);
                if (gameGrid.ContainsKey(newPos) && !isNecessary) {
                    if (gameGrid[newPos].isFilled) {
                        isNecessary = true;
                    }
                }

                //Top Left
                newPos = new Vector3(x + .5f, y, z + 1);
                if (gameGrid.ContainsKey(newPos) && !isNecessary) {
                    if (gameGrid[newPos].isFilled) {
                        isNecessary = true;
                    }
                }

                //Top Right
                newPos = new Vector3(x + .5f, y, z - 1);
                if (gameGrid.ContainsKey(newPos) && !isNecessary) {
                    if (gameGrid[newPos].isFilled) {
                        isNecessary = true;
                    }
                }

                //Bottom Left
                newPos = new Vector3(x - .5f, y, z + 1);
                if (gameGrid.ContainsKey(newPos) && !isNecessary) {
                    if (gameGrid[newPos].isFilled) {
                        isNecessary = true;
                    }
                }

                //Bottom Right
                newPos = new Vector3(x - .5f, y, z - 1);
                if (gameGrid.ContainsKey(newPos) && !isNecessary) {
                    if (gameGrid[newPos].isFilled) {
                        isNecessary = true;
                    }
                }

                if (!isNecessary) {
                    gameGrid.Remove(key);
                }

                isNecessary = false;
            }
        }
    }

    // Checks the surrounding of the selected tile and if the position is filled then it adds a true to a list at that postion.
    public Stack<Vector3> ValidateMoves(GameObject tile) {
        TileScript tileScript = tile.GetComponent(typeof(TileScript)) as TileScript;
        Stack<Vector3> validMoves = new Stack<Vector3>();
        Vector3 pos = tile.transform.position;

        //if (!IsBreaksHive(pos)) {

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

        //}

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
        else {
            gameGrid[newPos] = new GameGridCell();
            emptySpaces.Push(newPos);
        }

        //Below
        newPos = new Vector3(x - 1, y, z);
        if (gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                emptySpaces.Push(newPos);
            }
        }
        else {
            gameGrid[newPos] = new GameGridCell();
            emptySpaces.Push(newPos);
        }


        //Top Left
        newPos = new Vector3(x + .5f, y, z + 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                emptySpaces.Push(newPos);
            }
        }
        else {
            gameGrid[newPos] = new GameGridCell();
            emptySpaces.Push(newPos);
        }


        //Bottom Left
        newPos = new Vector3(x - .5f, y, z + 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                emptySpaces.Push(newPos);
            }
        }
        else {
            gameGrid[newPos] = new GameGridCell();
            emptySpaces.Push(newPos);
        }


        //Top Right
        newPos = new Vector3(x + .5f, y, z - 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                emptySpaces.Push(newPos);
            }
        }
        else {
            gameGrid[newPos] = new GameGridCell();
            emptySpaces.Push(newPos);
        }


        //Bottom Right
        newPos = new Vector3(x - .5f, y, z - 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                emptySpaces.Push(newPos);
            }
        }
        else {
            gameGrid[newPos] = new GameGridCell();
            emptySpaces.Push(newPos);
        }

        return emptySpaces;
    }

    //gets all occupied spaces of pieces and puts them in a Stack
    Stack<Vector3> GetOccupiedSpaces(Vector3 pos) {
        Stack<Vector3> occupiedSpaces = new Stack<Vector3>();

        float x = pos.x;
        float y = pos.y;
        float z = pos.z;

        //Above
        Vector3 newPos = new Vector3(x + 1, y, z);
        if (gameGrid[newPos].isFilled) {
            occupiedSpaces.Push(newPos);

        }

        //Below
        newPos = new Vector3(x - 1, y, z);
        if (gameGrid[newPos].isFilled) {
            occupiedSpaces.Push(newPos);
        }


        //Top Left
        newPos = new Vector3(x + .5f, y, z + 1);
        if (gameGrid[newPos].isFilled) {
            occupiedSpaces.Push(newPos);
        }


        //Bottom Left
        newPos = new Vector3(x - .5f, y, z + 1);
        if (gameGrid[newPos].isFilled) {
            occupiedSpaces.Push(newPos);
        }


        //Top Right
        newPos = new Vector3(x + .5f, y, z - 1);
        if (gameGrid[newPos].isFilled) {
            occupiedSpaces.Push(newPos);
        }


        //Bottom Right
        newPos = new Vector3(x - .5f, y, z - 1);
        if (gameGrid[newPos].isFilled) {
            occupiedSpaces.Push(newPos);
        }

        return occupiedSpaces;
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
        Stack<Vector3> occupiedSpaces = new Stack<Vector3>();
        Stack<Vector3> visitedPieces = new Stack<Vector3>();
        Vector3 newPos = pos;

        // if (/*piece is being moved*/) {
        while (visitedPieces.Count < occupiedSpaces.Count) {

            //fill occupiedSpaces with positions returned from GetOccupiedSpaces
            occupiedSpaces = GetOccupiedSpaces(pos);
            visitedPieces.Push(newPos);

            if (visitedPieces.Count < occupiedSpaces.Count) {
                return true;
            }
            else {
                return false;
            }

        }

        return false;
        // }


        //if (GetBoarderCount(pos) == 1) {
        //    return false;
        //}

        //return true;
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
        else {
            gameGrid.Add(newPos, new GameGridCell());
        }

        //Below
        newPos = new Vector3(x - 1, y, z);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled) {
                ret.Add(newPos, 69);
            }
        }
        else {
            gameGrid.Add(newPos, new GameGridCell());
        }


        //Top Left
        newPos = new Vector3(x + .5f, y, z + 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled) {
                ret.Add(newPos, 69);
            }
        }
        else {
            gameGrid.Add(newPos, new GameGridCell());
        }


        //Bottom Left
        newPos = new Vector3(x - .5f, y, z + 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled) {
                ret.Add(newPos, 69);
            }
        }
        else {
            gameGrid.Add(newPos, new GameGridCell());
        }


        //Top Right
        newPos = new Vector3(x + .5f, y, z - 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled) {
                ret.Add(newPos, 69);
            }
        }
        else {
            gameGrid.Add(newPos, new GameGridCell());
        }


        //Bottom Right
        newPos = new Vector3(x - .5f, y, z - 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled) {
                ret.Add(newPos, 69);
            }
        }
        else {
            gameGrid.Add(newPos, new GameGridCell());
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

    void CheckForWin(bool isWhite) {

        foreach (GameGridCell tile in gameGrid.Values) {
            if (tile.isFilled) {
                Vector3 pos = tile.tile.transform.position;

                TileScript tileScript = tile.tile.GetComponent(typeof(TileScript)) as TileScript;

                //Check if Queen is surrounded
                if (tileScript.GetTileColor() == "white") {
                    //White Lose
                    if (IsSurrounded(pos)) {
                        GameOver(isWhite);
                    }
                }
                else {
                    //Black Lose
                    if (IsSurrounded(pos)) {
                        GameOver(!isWhite);
                    }
                }
            }
        }
    }

    void GameOver(bool isWhite) {
        if (isWhite) {
            //White Lose
            menuManager.GoToLoserScreen();
        }
        else {
            //Black Lose
            menuManager.GoToWinnerScreen();
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

        for (int i = 0; gamePieces[i] != null; i++) {
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
                    break;
                }
            }

            if (isValid) {
                ret.Push(pos);
            }
        }

        return ret;
    }

   
    //Creates the selection grid at valid move positions
    public void SetMoveGrid(GameObject tile, bool isMove) {

        bool isWhite;

        if (turn == 0) {
            isWhite = true;
        }
        else {
            isWhite = false;
        }

        if (tilesPlaced > 0) {
            ClearMoveGrid();

            if (isMove) {

                if (!IsBreaksHive(tile.transform.position)) {
                    Stack<Vector3> positions = ValidateMoves(tile);
                    //Checks for Gates
                

                    while (positions.Count > 0) {
                        GameObject grid;
                        grid = Instantiate(GridTile, positions.Pop(), Quaternion.identity) as GameObject;
                        selectionGrids.Push(grid);
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
}