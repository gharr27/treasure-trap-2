using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove {
    Move move;
    public int score;


    public AIMove() {
        Move move = new Move();
        score = -100000000;
    }
    public AIMove(Move move, int score) {
        this.move = move;
        this.score = score;
    }
}

public class Move {
    public GameObject tile;
    public Vector3 pos;

    public Move() {
        tile = null;
        pos = Vector3.zero;
    }

    public Move(GameObject tile, Vector3 pos) {
        this.tile = tile;
        this.pos = pos;
    }
}

public class AI : MonoBehaviour {

    public GameObject[] tiles;

    int totalTileCount = 11;

    int queenCount = 1;
    int antCount = 3;
    int grasshopperCount = 3;
    int beetleCount = 2;
    int spiderCount = 2;

    GameObject tile = null;
    Vector3 pos = Vector3.zero;

    bool canMove = false;

    public int maxDepth = 5;
    private const int INT_MAX = 1000000000;

    private void Update() {
        if (queenCount == 0) {
            canMove = true;
        }
    }

    GameManagerAI gameManager;
    GameObject gameManagerObj;

    private void Start() {
        gameManagerObj = GameObject.FindWithTag("GameController");
        gameManager = gameManagerObj.GetComponent(typeof(GameManagerAI)) as GameManagerAI;
    }

    public Move Move(Dictionary<Vector3, GameManagerAI.GameGridCell> gameGrid, int round, bool isWhite) {
        //AIMove move = MiniMax(gameGrid, isWhite, maxDepth, 0 - INT_MAX, INT_MAX);

        Stack<Move> moves = GetPossibleMoves(gameGrid, round, isWhite);
        Move move = moves.Pop();

        gameManager.MakeMove(move.tile, move.pos, false);

        return moves.Pop();
    }

    AIMove MiniMax(Dictionary<Vector3, GameManagerAI.GameGridCell> gameGrid, bool isWhite, int depth, int alpha, int beta) {
        GameObject tile = null;
        Vector3 pos = Vector3.zero;

        if (depth == 0) {
            //Return Current Score
            return new AIMove(new Move(tile, pos), scoreGame(gameGrid, isWhite));
        }

        //AI move
        if (!isWhite) {

        }
        
        return new AIMove(new Move(tile, pos), scoreGame(gameGrid, isWhite));
    }

    int scoreGame(Dictionary<Vector3, GameManagerAI.GameGridCell> gameGrid, bool isWhite) {


        return 0;
    }

    Stack<Move> GetPossibleMoves(Dictionary<Vector3, GameManagerAI.GameGridCell> gameGrid, int round, bool isWhite) {
        Stack<Move> moves = new Stack<Move>();

        if (gameGrid.Count == 0) {
            //Place Grasshopper First
            Move move = new Move(tiles[2], Vector3.zero);
            moves.Push(move);
        }
        else {
            Stack<KeyValuePair<Vector3, GameManagerAI.GameGridCell>> tilesOnBoard = GetTilesOnBoard(gameGrid, isWhite);
        }

        return moves;
    }

    //Gets all AIs tile pieces from the board
    Stack<KeyValuePair<Vector3, GameManagerAI.GameGridCell>> GetTilesOnBoard(Dictionary<Vector3, GameManagerAI.GameGridCell> gameGrid, bool isWhite) {
        Stack<KeyValuePair<Vector3, GameManagerAI.GameGridCell>> tilesOnGrid = new Stack<KeyValuePair<Vector3, GameManagerAI.GameGridCell>>();

        foreach(KeyValuePair<Vector3, GameManagerAI.GameGridCell> gridTile in gameGrid) {
            if (gridTile.Value.isFilled) {
                GameObject tile = gridTile.Value.tile;
                TileScript tileScript = tile.GetComponent(typeof(TileScript)) as TileScript;

                if (isWhite) {
                    if (tileScript.GetTileColor()) {
                        tilesOnGrid.Push(gridTile);
                    }
                }
                else {
                    if (!tileScript.GetTileColor()) {
                        tilesOnGrid.Push(gridTile);
                    }
                }
            }
        }

        return tilesOnGrid;
    }
    
}
