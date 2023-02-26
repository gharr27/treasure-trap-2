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

    public AIMove Move(Dictionary<Vector3, GameManager.GameGridCell> gameGrid, int round, bool isWhite) {
        //AIMove move = MiniMax(gameGrid, isWhite, maxDepth, 0 - INT_MAX, INT_MAX);

        Stack<AIMove> moves = getPossibleMoves(gameGrid, round, isWhite);

        return moves.Pop();
    }

    AIMove MiniMax(Dictionary<Vector3, GameManager.GameGridCell> gameGrid, bool isWhite, int depth, int alpha, int beta) {
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

    int scoreGame(Dictionary<Vector3, GameManager.GameGridCell> gameGrid, bool isWhite) {


        return 0;
    }

    Stack<AIMove> getPossibleMoves(Dictionary<Vector3, GameManager.GameGridCell> gameGrid, int round, bool isWhite) {

        Stack<AIMove> moves = new Stack<AIMove>();
        Move move;
        AIMove aiMove;

        if (round == 1) {
            //Place Grasshopper
            if (isWhite) {
                move = new Move(tiles[3], Vector3.zero);
                aiMove = new AIMove(move, 10);
            }
            else {
                move = new Move(tiles[3], new Vector3(1, 0, 0));
                aiMove = new AIMove(move, 10);
            }

            moves.Push(aiMove);
        }

        return new Stack<AIMove>();
    }
    
}
