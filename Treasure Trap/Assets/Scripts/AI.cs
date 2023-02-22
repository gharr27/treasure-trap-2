using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove {
    public GameObject tile;
    public Vector3 pos;
    public int score;


    public AIMove() {
        tile = null;
        pos = Vector3.zero;
        score = -100000000;
    }
    public AIMove(GameObject tile, Vector3 pos, int score) {
        this.tile = tile;
        this.pos = pos;
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

public class AI : MonoBehaviour
{
    GameObject tile = null;
    Vector3 pos = Vector3.zero;

    public int maxDepth = 5;
    private const int INT_MAX = 1000000000;

    public void MakeMove(Dictionary<Vector3, GameManager.GameGridCell> gameGrid, bool isWhite) {
       AIMove move = MiniMax(gameGrid, isWhite, maxDepth, 0 - INT_MAX, INT_MAX);
    }

    AIMove MiniMax(Dictionary<Vector3, GameManager.GameGridCell> gameGrid, bool isWhite, int depth, int alpha, int beta) {
        GameObject tile = null;
        Vector3 pos = Vector3.zero;

        if (depth == 0) {
            //Return Current Score
            return new AIMove(tile, pos, scoreGame(gameGrid, isWhite));
        }

        //AI move
        if (!isWhite) {

        }

        return new AIMove(tile, pos, scoreGame(gameGrid, isWhite));
    }

    int scoreGame(Dictionary<Vector3, GameManager.GameGridCell> gameGrid, bool isWhite) {


        return 0;
    }

    Stack<AIMove> getPossibleMoves() {

        return new Stack<AIMove>();
    }
    
}
