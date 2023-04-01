using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour {

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

    //Contains Grid Prefab for creating MoveGrid
    public GameObject GridTile;
    //public GameObject Player;
    public GameObject[] Players;


    const int PIECE_COUNT = 22;

    public int tilesPlaced = 0;
    private int turn = 0;
    private int round = 1;

    private GameObject[] gamePieces;
    private Stack<GameObject> selectionGrids;

    bool isPlaying = false;
    bool isWin = false;
    bool isWhiteWin;
    bool isQueen1OnBoard = false;
    bool isQueen2OnBoard = false;
    bool isP1;

    PlayerScript p1;
    PlayerScript p2;
    PlayerScript activePlayer;
    GameObject playerObject1;
    GameObject playerObject2; 

    PhotonView photonView;

    MenusManager menuManager;
    GameObject menuManagerObject;

    Dictionary<Vector3, GameGridCell> gameGrid = new Dictionary<Vector3, GameGridCell>();

    public int GetRound() {
        return round;
    }
    public int GetTurn() {
        return turn;
    }

    // Start is called before the first frame update
    void Start() {

        photonView = GetComponent<PhotonView>();

        selectionGrids = new Stack<GameObject>();
        gamePieces = new GameObject[PIECE_COUNT];

        playerObject1 = PhotonNetwork.Instantiate(Players[0].name, Vector3.zero, Quaternion.identity);
        playerObject2 = PhotonNetwork.Instantiate(Players[1].name, Vector3.zero, Quaternion.identity);

        menuManagerObject = GameObject.FindWithTag("Menu");
        menuManager = menuManagerObject.GetComponent(typeof(MenusManager)) as MenusManager;

        //Is Host
        if (PhotonNetwork.IsMasterClient) {
            isP1 = true;
            Debug.Log("P1");
        }
        else {
            isP1 = false;
            Debug.Log("P2");
        }

        p1 = playerObject1.GetComponent(typeof(PlayerScript)) as PlayerScript;
        p1.color = "white";

        p2 = playerObject2.GetComponent(typeof(PlayerScript)) as PlayerScript;
        p2.color = "black";

        activePlayer = p1;
    }

    public void UpdateActivePlayer() {
        activePlayer = activePlayer == p1 ? p2 : p1;
        UpdateTurn();
    }

    private void UpdateTurn() {
        if (turn == 0) {
            turn = 1;
        }
        else {
            turn = 0;
            round++;
        }
    }

    // Update is called once per frame
    void Update() {

        if (!isWin) {
            if (!isPlaying) {
                if (activePlayer.color == "white" && isP1) {
                    isPlaying = true;
                    //White Move
                    Debug.Log("White Move");
                    StartCoroutine(p1.Move(true));
                }
                else if (activePlayer.color == "black" && !isP1) {
                    isPlaying = true;
                    //Black Move
                    Debug.Log("Black Move");
                    Debug.Log(round);
                    StartCoroutine(p2.Move(true));
                }
            }
        }
        else {
            if (isWhiteWin) {
                Debug.Log("White Wins!");
                menuManager.GoToWinnerScreen();

            }
            else {
                Debug.Log("Black Wins!");
                menuManager.GoToLoserScreen();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log(gameGrid.Count);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            menuManager.GoToMainMenu();
        }
    }

    public void Move(GameObject tile, Vector3 pos, bool isMove) {
        Debug.Log("test");
        photonView.RPC("NetworkMakeMove", RpcTarget.All, tile, pos, isMove);
    }

    [PunRPC]
    void NetworkMakeMove(GameObject tile, Vector3 pos, bool isMove) {
        Debug.Log("test");
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
        CheckForWin();
    }

    [PunRPC]
    void AddToGamePieces(GameObject tilePiece) {
        gamePieces[tilesPlaced] = tilePiece;
        Debug.Log(gamePieces.Length);
    }

    [PunRPC]
    void IncrementTilesPlaced(int i) {
        tilesPlaced += i;
    }

    public void MakeMove(GameObject tile, Vector3 pos, bool isMove) {
        
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

        ClearMoveGrid();
        CheckForWin();

        isPlaying = false;
    }


    void UpdateGameGrid(Vector3 pos) {
        float x = pos.x;
        float y = pos.y;
        float z = pos.z;

        GameGridCell gridCell = new GameGridCell();

        //On Top
        Vector3 newPos = new Vector3(x + 1, y + 1, z);
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
    Stack<Vector3> ValidateMoves(GameObject tile) {
        TileScript tileScript = tile.GetComponent(typeof(TileScript)) as TileScript;
        Stack<Vector3> validMoves = new Stack<Vector3>();
        Vector3 pos = tile.transform.position;

        //if (!IsBreaksHive(pos)) {

            Stack<Vector3> emptySpaces = new Stack<Vector3>();

            emptySpaces = GetEmptySpaces(pos);

            if (tileScript.GetTileName() == "Queen") {
                validMoves = QueenPossibleMoves(tile, emptySpaces);
            }
            else if (tileScript.GetTileName() == "Ant") {
                validMoves = AntPossibleMoves(tile);
            }
            else if (tileScript.GetTileName() == "Grasshopper") {
                validMoves = GrasshopperPossibleMoves(tile);
            }
            else if (tileScript.GetTileName() == "Beetle") {
                validMoves = BeetlePossibleMoves(tile, emptySpaces);
            }
            else if (tileScript.GetTileName() == "Spider") {
                //validMoves = AntPossibleMoves(tile);
                validMoves = SpiderPossibleMoves(tile, emptySpaces);
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

    /* Shows available moves for the queen*/
    Stack<Vector3> QueenPossibleMoves(GameObject tile, Stack<Vector3> emptySpaces) {
        Stack<Vector3> validMovePositions = new Stack<Vector3>();
        Vector3 tilePos = tile.transform.position;


        //Check Sides
        while (emptySpaces.Count > 0) {
            Vector3 pos = emptySpaces.Pop();

            if (CheckIfValid(tilePos, pos)) {
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
            if (!gridCell.Value.isFilled && CheckIfValid(tilePos, gridCell.Key)) {
                validMovePositions.Push(gridCell.Key);
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

    Stack<Vector3> BeetlePossibleMoves(GameObject tile, Stack<Vector3> emptySpaces) {
        Stack<Vector3> validMovePositions = new Stack<Vector3>();
        Vector3 tilePos = tile.transform.position;


        //Check Sides
        while (emptySpaces.Count > 0) {
            Vector3 pos = emptySpaces.Pop();

            if (CheckIfBeetleValid(tilePos, pos)) {
                validMovePositions.Push(pos);
            }
            Dictionary<Vector3, int> validPos = CheckOntopHive(tilePos);

            foreach (KeyValuePair<Vector3, int> goodPos in validPos) {
                validMovePositions.Push(goodPos.Key);
            }
        }

        return validMovePositions;
    }

    Dictionary<Vector3, int> CheckOntopHive(Vector3 pos) {
        Dictionary<Vector3, int> validPos = new Dictionary<Vector3, int>();
        float x = pos.x;
        float y = pos.y;
        float z = pos.z;

        //Above
        Vector3 newPos = new Vector3(x + 1, y, z);
        if (gameGrid.ContainsKey(newPos)) {
            while (gameGrid[newPos].isFilled) {
                newPos = new Vector3(x + 1, y += 1, z);
            }
            validPos[newPos] = 420;
        }
        else {
            gameGrid.Add(new Vector3(x + 1, y += 1, z), new GameGridCell());
        }

        y = 0;

        //Below
        newPos = new Vector3(x - 1, y, z);
        if (gameGrid.ContainsKey(newPos)) {
            while (gameGrid[newPos].isFilled) {
                newPos = new Vector3(x - 1, y += 1, z);
            }
            validPos[newPos] = 420;
        }
        else {
            gameGrid.Add(new Vector3(x - 1, y += 1, z), new GameGridCell());
        }

        y = 0;

        //Top Left
        newPos = new Vector3(x + .5f, y, z + 1);
        if (gameGrid.ContainsKey(newPos)) {
            while (gameGrid[newPos].isFilled) {
                newPos = new Vector3(x + .5f, y += 1, z + 1);
            }
            validPos[newPos] = 420;
        }
        else {
            gameGrid.Add(new Vector3(x + .5f, y += 1, z + 1), new GameGridCell());
        }

        y = 0;

        //Bottom Left
        newPos = new Vector3(x - .5f, y, z + 1);
        if (gameGrid.ContainsKey(newPos)) {
            while (gameGrid[newPos].isFilled) {
                newPos = new Vector3(x - .5f, y += 1, z + 1);
            }
            validPos[newPos] = 420;
        }
        else {
            gameGrid.Add(new Vector3(x - .5f, y += 1, z + 1), new GameGridCell());
        }

        y = 0;

        //Top Right
        newPos = new Vector3(x + .5f, y, z - 1);
        if (gameGrid.ContainsKey(newPos)) {
            while (gameGrid[newPos].isFilled) {
                newPos = new Vector3(x + .5f, y += 1, z - 1);
            }
            validPos[newPos] = 420;
        }
        else {
            gameGrid.Add(new Vector3(x + .5f, y += 1, z - 1), new GameGridCell());
        }

        y = 0;

        //Bottom Right
        newPos = new Vector3(x - .5f, y, z - 1);
        if (gameGrid.ContainsKey(newPos)) {
            while (gameGrid[newPos].isFilled) {
                newPos = new Vector3(x - .5f, y += 1, z - 1);
            }
            validPos[newPos] = 420;
        }
        else {
            gameGrid.Add(new Vector3(x - .5f, y += 1, z - 1), new GameGridCell());
        }

        return validPos;
    }

    bool CheckIfBeetleValid(Vector3 tilePos, Vector3 spacePos) {
        float x = spacePos.x;
        float y = spacePos.y;
        float z = spacePos.z;

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

        //On Top Hive
        //Above
        newPos = new Vector3(x + 1, y + 1, z);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled && newPos != tilePos) {
                return true;
            }
        }

        //Below
        newPos = new Vector3(x - 1, y + 1, z);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled && newPos != tilePos) {
                return true;
            }
        }


        //Top Left
        newPos = new Vector3(x + .5f, y + 1, z + 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled && newPos != tilePos) {
                return true;
            }
        }


        //Bottom Left
        newPos = new Vector3(x - .5f, y + 1, z + 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled && newPos != tilePos) {
                return true;
            }
        }


        //Top Right
        newPos = new Vector3(x + .5f, y + 1, z - 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled && newPos != tilePos) {
                return true;
            }
        }


        //Bottom Right
        newPos = new Vector3(x - .5f, y + 1, z - 1);
        if (gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled && newPos != tilePos) {
                return true;
            }
        }

        return false;
    }

    Stack<Vector3> SpiderPossibleMoves(GameObject tile, Stack<Vector3> emptySpaces) {
        Stack<Vector3> validMovePositions = new Stack<Vector3>();
        Vector3 tilePos = tile.transform.position;

        validMovePositions = SimulateMoves(tilePos, emptySpaces);

        return validMovePositions;
    }
    Stack<Vector3> SimulateMoves(Vector3 curPos, Stack<Vector3> emptySpaces) {
        //Check Sides
        Stack<Vector3> ret = new Stack<Vector3>();
        Stack<Vector3> validMovePos = new Stack<Vector3>();
        Stack<Vector3> firstMoves = new Stack<Vector3>();
        Stack<Vector3> firstMovesValid = new Stack<Vector3>();
        Stack<Vector3> secondMoves = new Stack<Vector3>();
        Stack<Vector3> secondMovesValid = new Stack<Vector3>();
        Stack<Vector3> thirdMoves = new Stack<Vector3>();
        Stack<Vector3> thirdMovesValid = new Stack<Vector3>();
        Dictionary<Vector3, int> invalidPos = new Dictionary<Vector3, int>();
        Dictionary<Vector3, int> prevPos = new Dictionary<Vector3, int>();
        Dictionary<Vector3, int> boarderTiles = new Dictionary<Vector3, int>();

        //1st Moves
        while (emptySpaces.Count > 0) {
            Vector3 pos = emptySpaces.Pop();

            if (CheckIfValid(curPos, pos)) {
                firstMoves.Push(pos);
            }
            else if (!invalidPos.ContainsKey(pos)) {
                invalidPos.Add(pos, 69);
            }

            boarderTiles = GetBoarderTiles(curPos);

            //Build KeyValuePairs Dictionary here

            foreach (KeyValuePair<Vector3, int> tile in boarderTiles) {
                if (firstMoves.Count > 0) {
                    bool isValid = false;
                    Vector3 temp = firstMoves.Pop();
                    Dictionary<Vector3, int> keyValuePairs = GetBoarderTiles(temp);

                    foreach (KeyValuePair<Vector3, int> tile2 in keyValuePairs) {
                        if (tile2.Key == tile.Key) {
                            isValid = true;
                        }
                    }

                    if (isValid) {
                        firstMovesValid.Push(temp);
                    }
                }
            }
        }


        //2nd Moves
        while (firstMovesValid.Count > 0) {
            prevPos[curPos] = 69;   //Nice
            curPos = firstMovesValid.Peek();
            emptySpaces = GetEmptySpaces(firstMovesValid.Pop());

            Vector3 pos;

            while (emptySpaces.Count > 0) {
                pos = emptySpaces.Pop();

                if (CheckIfValid(curPos, pos)) {
                    secondMoves.Push(pos);
                }
                else if (!invalidPos.ContainsKey(pos)) {
                    invalidPos.Add(pos, 69);
                }
            }

            boarderTiles = GetBoarderTiles(curPos);

            //Build KeyValuePairs Dictionary here

            foreach (KeyValuePair<Vector3, int> tile in boarderTiles) {
                if (secondMoves.Count > 0) {
                    bool isValid = false;
                    Vector3 temp = secondMoves.Pop();
                    Dictionary<Vector3, int> keyValuePairs = GetBoarderTiles(temp);

                    foreach (KeyValuePair<Vector3, int> tile2 in keyValuePairs) {
                        if (tile2.Key == tile.Key) {
                            isValid = true;
                        }
                    }

                    if (isValid) {
                        secondMovesValid.Push(temp);
                    }
                }
            }
        }

        //3rd Moves
        while (secondMovesValid.Count > 0) {
            prevPos[curPos] = 69;
            curPos = secondMovesValid.Peek();
            emptySpaces = GetEmptySpaces(secondMovesValid.Pop());

            Vector3 pos;

            while (emptySpaces.Count > 0) {
                pos = emptySpaces.Pop();

                if (CheckIfValid(curPos, pos)) {
                    thirdMoves.Push(pos);
                }
            }

            boarderTiles = GetBoarderTiles(curPos);

            //Build KeyValuePairs Dictionary here

            foreach (KeyValuePair<Vector3, int> tile in boarderTiles) {
                if (thirdMoves.Count > 0) {
                    bool isValid = false;
                    Vector3 temp = thirdMoves.Pop();
                    Dictionary<Vector3, int> keyValuePairs = GetBoarderTiles(temp);

                    foreach (KeyValuePair<Vector3, int> tile2 in keyValuePairs) {
                        if (tile2.Key == tile.Key) {
                            isValid = true;
                        }
                    }

                    if (isValid) {
                        thirdMovesValid.Push(temp);
                    }
                }
            }
        }

        while (thirdMovesValid.Count > 0) {
            bool isValid = true;
            Vector3 pos = thirdMovesValid.Pop();

            foreach (KeyValuePair<Vector3, int> badPos in invalidPos) {
                if (pos == badPos.Key) {
                    isValid = false;
                }
            }

            foreach (KeyValuePair<Vector3, int> badPos in prevPos) {
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
                if (tileScript.GetId() == "Queen1" || tileScript.GetId() == "Queen2") {

                    if (IsSurrounded(pos)) {
                        isWin = true;

                        if (tileScript.GetTileColor()) {
                            isWhiteWin = false;
                        }
                        else {
                            isWhiteWin = true;
                        }
                        
                    }
                }
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

    //Returns a vector of all the valid movement/placement positions
    Stack<Vector3> GetMovePositions() {
        Stack<Vector3> positions = new Stack<Vector3>();
        Stack<Vector3> ret = new Stack<Vector3>();
        Dictionary<Vector3, int> invalidMove = new Dictionary<Vector3, int>();

        //Checks for the open positions around a tile for creating potential move positions

        for (int i = 0; gamePieces[i] != null; i++) {
            bool canPlace = true;
            TileScript tileScript = gamePieces[i].GetComponent(typeof(TileScript)) as TileScript;

            if (round != 1) {
                if (turn == 0) {
                    if (!tileScript.GetTileColor()) {
                        canPlace = false;
                    }
                }
                else {
                    if (tileScript.GetTileColor()) {
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
                positions.Push(pos);

                //South of Tile
                pos = new Vector3(x - 1, 0, z);
                positions.Push(pos);

                //North East of Tile
                pos = new Vector3(x + .5f, 0, z + 1);
                positions.Push(pos);

                //North West of Tile
                pos = new Vector3(x + .5f, 0, z - 1);
                positions.Push(pos);

                //South East of Tile
                pos = new Vector3(x - .5f, 0, z + 1);
                positions.Push(pos);

                //South West of Tile
                pos = new Vector3(x - .5f, 0, z - 1);
                positions.Push(pos);
            }
            else {
                float x = gamePieces[i].transform.position.x;
                float z = gamePieces[i].transform.position.z;

                Vector3 pos;

                //North of Tile

                pos = new Vector3(x + 1, 0, z);
                invalidMove[pos] = 69;

                //South of Tile
                pos = new Vector3(x - 1, 0, z);
                invalidMove[pos] = 69;

                //North East of Tile
                pos = new Vector3(x + .5f, 0, z + 1);
                invalidMove[pos] = 69;

                //North West of Tile
                pos = new Vector3(x + .5f, 0, z - 1);
                invalidMove[pos] = 69;

                //South East of Tile
                pos = new Vector3(x - .5f, 0, z + 1);
                invalidMove[pos] = 69;

                //South West of Tile
                pos = new Vector3(x - .5f, 0, z - 1);
                invalidMove[pos] = 69;
            }
        }

        while (positions.Count > 0) {
            bool isValid = true;
            Vector3 pos = positions.Pop();

            foreach (KeyValuePair<Vector3, int> badPos in invalidMove) {
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

   
    //Creates the selection grid at valid move positions
    public void SetMoveGrid(GameObject tile, bool isMove) {
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
                Stack<Vector3> positions = GetMovePositions();
                Debug.Log(positions.Count);

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

    public bool AreQueensOnBoard() {
        foreach (GameGridCell tile in gameGrid.Values) {
            if (tile != null) {
                if (tile.tile != null) {
                    TileScript tileScript = tile.tile.GetComponent(typeof(TileScript)) as TileScript;

                    if (tileScript.GetId() == "Queen1" && !isQueen1OnBoard) {
                        isQueen1OnBoard = true;
                    }
                    if (tileScript.GetId() == "Queen2" && !isQueen2OnBoard) {
                        isQueen2OnBoard = true;
                    }
                }
            }
        }

        if (isQueen1OnBoard && isQueen2OnBoard) {
            return true;
        }
        else {
            return false;
        }
    }
}