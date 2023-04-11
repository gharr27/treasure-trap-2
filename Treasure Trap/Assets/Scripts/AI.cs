using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move {
    public GameObject tile;
    public Vector3 pos;
    public bool isMove;
    public string tileName;

    public Move() {
        tile = null;
        pos = Vector3.zero;
        isMove = false;
        tileName = null;
    }

    public Move(GameObject tile, Vector3 pos, bool isMove) {
        this.tile = tile;
        this.pos = pos;
        this.isMove = isMove;
    }
}

public class AI : MonoBehaviour {

    public GameObject[] tiles;

    int totalTileCount = 11;

    public int queenCount = 1;
    public int antCount = 3;
    public int grasshopperCount = 3;
    public int beetleCount = 2;
    public int spiderCount = 2;

    public bool isTurn;

    public GameObject gameManagerObj;
    GameManager gameManager;

    public bool isQueenPlaced = false;

    public int maxDepth = 5;
    private const int INT_MAX = 1000000000;
    private const int INT_MIN = -1000000000;

    private void Start() {
        gameManager = gameManagerObj.GetComponent(typeof(GameManager)) as GameManager;
    }

    public void DecrementTile(string name) {

        switch(name) {
            case "Queen":
                queenCount--;
                isQueenPlaced = true;
                break;
            case "Ant":
                antCount--;
                break;
            case "Grasshopper":
                grasshopperCount--;
                break;
            case "Beetle":
                beetleCount--;
                break;
            case "Spider":
                spiderCount--;
                break;
        }

    }

    public void Move(Dictionary<Vector3, GameManager.GameGridCell> gameGrid, int round, bool isWhite) {

        Debug.Log("Generating Moves");
        Stack<Move> moves = GenerateMoves(gameGrid, round, isWhite);
        Debug.Log("Test");

        int randIndex = Random.Range(0, moves.Count);

        for (int i = 0; i < randIndex; i++) {
            moves.Pop();
        }

        Move move = moves.Pop();

        gameManager.AIMove(move.tile, move.pos, move.isMove);
    }

    Stack<Move> GenerateMoves(Dictionary<Vector3, GameManager.GameGridCell> gameGrid, int round, bool isWhite) {
        Stack<Move> moves = new Stack<Move>();

        Stack<Vector3> placePosition = gameManager.GetPlacePositions(isWhite);

        if (round < 4 || isQueenPlaced) {
            while (placePosition.Count > 0) {
                Vector3 pos = placePosition.Pop();

                for (int i = 0; i < 5; i++) {
                    bool canPlace = false;
                    switch (i) {
                        case 0:
                            if (queenCount > 0) {
                                canPlace = true;
                            }
                            break;
                        case 1:
                            if (antCount > 0) {
                                canPlace = true;
                            }
                            break;
                        case 2:
                            if (grasshopperCount > 0) {
                                canPlace = true;
                            }
                            break;
                        case 3:
                            if (beetleCount > 0) {
                                canPlace = true;
                            }
                            break;
                        case 4:
                            if (spiderCount > 0) {
                                canPlace = true;
                            }
                            break;
                    }
                    if (canPlace) {
                        moves.Push(new Move(tiles[i], pos, false));
                    }
                }
            }

            if (isQueenPlaced) {
                foreach (GameManager.GameGridCell gridTile in gameGrid.Values) {
                    if (gridTile.isFilled) {
                        Stack<Vector3> movePositions = gameManager.ValidateMoves(gridTile.tile);

                        while (movePositions.Count > 0) {
                            Vector3 placePos = movePositions.Pop();

                            moves.Push(new Move(gridTile.tile, placePos, true));
                        }
                    }
                }
            }
        }
        else {
            while (placePosition.Count > 0) {
                Vector3 pos = placePosition.Pop();
                moves.Push(new Move(tiles[0], pos, false));
            }
        }

        Debug.Log("done Generating Moves");
        return moves;
    }
}
