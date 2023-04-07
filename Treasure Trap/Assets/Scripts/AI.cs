using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move {
    public GameObject tile;
    public Vector3 pos;
    public bool isMove;

    public Move() {
        tile = null;
        pos = Vector3.zero;
        isMove = false;
    }

    public Move(GameObject tile, Vector3 pos, bool isMove) {
        this.tile = tile;
        this.pos = pos;
        this.isMove = isMove;
    }
}

public class AI : GameManager {

    public GameObject[] tiles;

    int totalTileCount = 11;

    public int queenCount = 1;
    public int antCount = 3;
    public int grasshopperCount = 3;
    public int beetleCount = 2;
    public int spiderCount = 2;

    GameObject tile = null;
    Vector3 pos = Vector3.zero;

    bool isQueenPlaced = false;

    public int maxDepth = 5;
    private const int INT_MAX = 1000000000;
    private const int INT_MIN = -1000000000;

    private void CheckQueenPlaced(Dictionary<Vector3, GameGridCell> gameGrid) {
        bool isQueenFound = false;
        foreach (GameGridCell tile in gameGrid.Values) {
            if (tile.tile.name == "Queen-Black") {
                isQueenFound = true;
            }
        }

        if (isQueenFound) {
            isQueenPlaced = true;
        }
        else {
            isQueenPlaced = false;
        }
    }

    public void Move(Dictionary<Vector3, GameGridCell> gameGrid, int round, bool isWhite) {

        CheckQueenPlaced(gameGrid);
        Move move = MiniMax(gameGrid, isWhite, maxDepth, 0 - INT_MAX, INT_MAX, round).Value;

        MakeMove(move.tile, move.pos, move.isMove);
    }

    Stack<Move> GenerateMoves(Dictionary<Vector3, GameGridCell> gameGrid, bool isWhite) {
        Stack<Move> moves = new Stack<Move>();

        Stack<Vector3> placePosition = GetPlacePositions(isWhite);

        while (placePosition.Count > 0) {
            Vector3 pos = placePosition.Pop();

            for (int i = 0; i < 5; i++) {
                bool canMove = false;
                switch (i) {
                    case 0:
                        if (queenCount > 0) {
                            canMove = true;
                        }
                        break;
                    case 1:
                        if (antCount > 0) {
                            canMove = true;
                        }
                        break;
                    case 2:
                        if (grasshopperCount > 0) {
                            canMove = true;
                        }
                        break;
                    case 3:
                        if (beetleCount > 0) {
                            canMove = true;
                        }
                        break;
                    case 4:
                        if (spiderCount > 0) {
                            canMove = true;
                        }
                        break;
                }
                if (canMove) {
                    moves.Push(new Move(tiles[i], pos, false));
                }
            }
        }

        if (isQueenPlaced) {
            foreach (KeyValuePair<Vector3, GameGridCell> gridTile in gameGrid) {
                if (gridTile.Value.isFilled) {
                    Stack<Vector3> movePositions = ValidateMoves(gridTile.Value.tile);

                    while (movePositions.Count > 0) {
                        Vector3 placePos = movePositions.Pop();

                        moves.Push(new Move(gridTile.Value.tile, placePos, true));
                    }
                }
            }
        }

        return moves;
    }
    KeyValuePair<int, Move> MiniMax(Dictionary<Vector3, GameGridCell> gameGrid, bool isAI, int depth, int alpha, int beta, int round) {
        Stack<Move> moves = GenerateMoves(gameGrid, !isAI);

        if (depth == 0 || depth >= moves.Count) {
            return new KeyValuePair<int, Move>(ScoreGame(gameGrid, isAI), new Move());
        }
        if (isAI) { //Max
            KeyValuePair<int, Move> moveSoFar = new KeyValuePair<int, Move>(INT_MIN, new Move());
            if (IsWinningMove(gameGrid, !isAI)) {
                return moveSoFar;
            }

            for (int i = 0; i < moves.Count; i++) {
                Move curMove = moves.Pop();

                Dictionary<Vector3, GameManager.GameGridCell> tempGrid = SimulateMove(gameGrid, curMove);
                int score = MiniMax(tempGrid, !isAI, depth - 1, alpha, beta, round++).Key;
                if (score > moveSoFar.Key) {
                    moveSoFar = new KeyValuePair<int, Move>(score, curMove);
                }
                if (moveSoFar.Key > alpha) {
                    alpha = moveSoFar.Key;
                }
                if (alpha >= beta) {
                    break;
                }
            }

            return moveSoFar;
        }
        else {  //Min
            KeyValuePair<int, Move> moveSoFar = new KeyValuePair<int, Move>(INT_MAX, new Move());

            if (IsWinningMove(gameGrid, !isAI)) {
                return moveSoFar;
            }

            for (int i = 0; i < moves.Count; i++) {
                Move curMove = moves.Pop();

                Dictionary<Vector3, GameManager.GameGridCell> tempGrid = SimulateMove(gameGrid, curMove);
                int score = MiniMax(tempGrid, !isAI, depth - 1, alpha, beta, round).Key;

                if (score > moveSoFar.Key) {
                    moveSoFar = new KeyValuePair<int, Move>(score, curMove);
                }

                if (moveSoFar.Key < beta) {
                    beta = moveSoFar.Key;
                }
                if (alpha >= beta) {
                    break;
                }
            }

            return moveSoFar;
        }
    }

    int ScoreGame(Dictionary<Vector3, GameGridCell> gameGrid, bool isAI) {

        int score = 0;

        score += TilesAroundQueenHeuristic(gameGrid, isAI);
        //Heuristic to check how many movable pieces available, AI and player

        return score;
    }

    int TilesAroundQueenHeuristic(Dictionary<Vector3, GameGridCell> gameGrid, bool isAI) {
        int score = 0;
        int numOfOpponentBoarderTiles = 0;
        int numOfBoarderTiles = 0;

        if (isAI) {
            foreach (GameGridCell tile in gameGrid.Values) {
                if (tile.tile.name == "Queen-White") {
                    numOfOpponentBoarderTiles = GetBoarderTiles(gameGrid, tile.tile);
                }
                else if (tile.tile.name == "Queen-Black") {
                    numOfBoarderTiles = GetBoarderTiles(gameGrid, tile.tile);
                }
            }
        } else {
            foreach (GameGridCell tile in gameGrid.Values) {
                if (tile.tile.name == "Queen-Black") {
                    numOfOpponentBoarderTiles = GetBoarderTiles(gameGrid, tile.tile);
                }
                else if (tile.tile.name == "Queen-White") {
                    numOfBoarderTiles = GetBoarderTiles(gameGrid, tile.tile);
                }
            }
        }

        switch (numOfOpponentBoarderTiles) {
            case 0:
                score += 0;
                break;
            case 1:
                score += 10;
                break;
            case 2:
                score += 20;
                break;
            case 3:
                score += 30;
                break;
            case 4:
                score += 40;
                break;
            case 5:
                score += 50;
                break;
            case 6:
                score += 100000;
                break;
        }

        switch (numOfBoarderTiles) {
            case 0:
                score -= 0;
                break;
            case 1:
                score -= 10;
                break;
            case 2:
                score -= 20;
                break;
            case 3:
                score -= 30;
                break;
            case 4:
                score -= 40;
                break;
            case 5:
                score -= 50;
                break;
            case 6:
                score -= 100000;
                break;
        }

        return score;
    }

    bool IsWinningMove(Dictionary<Vector3, GameGridCell> gameGrid, bool isAI) {

        //Check if Queen has only one spot empty
        if (isAI) {
            foreach (GameGridCell tile in gameGrid.Values) {
                if (tile.tile.name == "Queen-White") {
                    int numOfBoarderTiles = GetBoarderTiles(gameGrid, tile.tile);

                    if (numOfBoarderTiles >= 5) {
                        return true;
                    }

                    return false;
                }
            }

            return false;
        }
        else {
            foreach (GameGridCell tile in gameGrid.Values) {
                if (tile.tile.name == "Queen-Black") {
                    int numOfBoarderTiles = GetBoarderTiles(gameGrid, tile.tile);

                    if (numOfBoarderTiles >= 5) {
                        return true;
                    }

                    return false;
                }
            }

            return false;
        }
        //Later Improvement, Check if opponent has moveable pieces or can spawn a piece by your queen
    }

    int GetBoarderTiles(Dictionary<Vector3, GameGridCell> gameGrid, GameObject tile) {
        Vector3 newPos;
        int boarderTiles = 0;
        float x = tile.transform.position.x;
        float y = tile.transform.position.y;
        float z = tile.transform.position.z;

        //Above
        newPos = new Vector3(x + 1, y, z);
        if (!gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled) {
                boarderTiles++;
            }
        }

        //Below
        newPos = new Vector3(x - 1, y, z);
        if (!gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled) {
                boarderTiles++;
            }
        }

        //Top Left
        newPos = new Vector3(x + .5f, y, z + 1);
        if (!gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled) {
                boarderTiles++;
            }
        }

        //Top Right
        newPos = new Vector3(x + .5f, y, z - 1);
        if (!gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled) {
                boarderTiles++;
            }
        }

        //Bottom Left
        newPos = new Vector3(x - .5f, y, z + 1);
        if (!gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled) {
                boarderTiles++;
            }
        }

        //Bottom Right
        newPos = new Vector3(x - .5f, y, z - 1);
        if (!gameGrid.ContainsKey(newPos)) {
            if (gameGrid[newPos].isFilled) {
                boarderTiles++;
            }
        }

        return boarderTiles;
    }

    Dictionary<Vector3, GameGridCell> SimulateMove(Dictionary<Vector3, GameGridCell> gameGrid, Move move) {
        Dictionary<Vector3, GameGridCell> ret = gameGrid;

        GameObject tile = move.tile;
        Vector3 pos = move.pos;
        bool isMove = move.isMove;

        if (isMove) {
            ret[tile.transform.position] = new GameGridCell();
            tile.transform.position = pos;

            GameGridCell gridCell = new GameGridCell(true, tile);

            if (!ret.ContainsKey(pos)) {
                ret.Add(pos, gridCell);
            }
            else {
                ret[pos] = gridCell;
            }
        }
        else {
            GameGridCell gridCell = new GameGridCell(true, tile);

            if (!ret.ContainsKey(pos)) {
                ret.Add(pos, gridCell);
            }
            else {
                ret[pos] = gridCell;
            }
        }
        if (!isQueenPlaced) {
            CheckQueenPlaced(ret);
        }

        return ret;
    }
}
