using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove {

}

public class AI : MonoBehaviour
{
    public int maxDepth = 5;
    private const int INT_MAX = 1000000000;

    public void MakeMove(Dictionary<Vector3, GameManager.GameGridCell> gameGrid, bool isWhite) {
        MiniMax(gameGrid, isWhite, maxDepth, 0 - INT_MAX, INT_MAX);
    }

    void MiniMax(Dictionary<Vector3, GameManager.GameGridCell> gameGrid, bool isWhite, int depth, int alpha, int beta) {

    }
}
