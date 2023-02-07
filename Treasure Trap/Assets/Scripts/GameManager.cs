using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Holds all the Game Tile Pieces
    // Indexes:
    //      0. Queen
    //      1. Ant
    //      2. Grasshopper
    //      3. Beetle
    //      4. Spider
    public GameObject[] TilePieces;
    public GameObject SelectionTile;
    
    public bool isSelectionMade = false;

    const int PIECE_COUNT = 22;

    private int tilesPlaced = 0;
    private GameObject[] gamePieces;

    private GameObject selectedPiece;
    private Stack<GameObject> selectionGrids;

    Ray ray;
    RaycastHit hit;

    int pieceSelection;

    bool isPieceSelected = false;
    bool isMovePiece = false;
    bool isGridSet = false;

    Vector3 pos = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        selectionGrids = new Stack<GameObject>();
        gamePieces = new GameObject[PIECE_COUNT];
    }

    // Update is called once per frame
    void Update()
    {
        //Placing A Piece
        IsPieceSelected();

        if (isPieceSelected) {

            if (tilesPlaced == 0) {
                PlacePiece(pieceSelection, pos);
                isPieceSelected = false;
            }
            else {
                if (!isGridSet){
                    SetMoveGrid();
                }

                if (isSelectionMade) {
                    PlacePiece(pieceSelection, pos);
                    isPieceSelected = false;
                    isGridSet = false;
                    isSelectionMade = false;
                }
            }
        }

        //Moving A Piece
        if (isMovePiece) {
            isPieceSelected = false;

            if (!isGridSet) {
                SetMoveGrid();
            }

            if (isSelectionMade) {
                MovePiece(selectedPiece, pos);
                isMovePiece = false;
                isGridSet = false;
                isSelectionMade = false;
            }
        }
    }

    public void SetPosition(Vector3 position) {
        pos = position;
        isSelectionMade = true;
    }

    public void SetSelectedPiece(GameObject piece) {
        selectedPiece = piece;
        isMovePiece = true;
    }

    void IsPieceSelected() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            pieceSelection = 0;
            isPieceSelected = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            pieceSelection = 1;
            isPieceSelected = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            pieceSelection = 2;
            isPieceSelected = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            pieceSelection = 3;
            isPieceSelected = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            pieceSelection = 4;
            isPieceSelected = true;
        }
    }

    void PlacePiece(int piece, Vector3 pos) {
        GameObject tile = TilePieces[piece];

        gamePieces[tilesPlaced] = Instantiate(tile, pos, Quaternion.identity) as GameObject;
        tilesPlaced++;
        //Test

        while(selectionGrids.Count != 0) {
            GameObject temp = selectionGrids.Pop();
            Destroy(temp);
        }

    }

    void MovePiece(GameObject piece, Vector3 pos) {
        piece.transform.position = pos;

        while(selectionGrids.Count != 0) {
            GameObject temp = selectionGrids.Pop();
            Destroy(temp);
        }
    }

    Vector3[,] GetMovePositions() {
        Vector3[,] positions = new Vector3[tilesPlaced, 6];

        for (int i = 0; gamePieces[i] != null; i++) {
            float xCoord = gamePieces[i].transform.position.x;
            float zCoord = gamePieces[i].transform.position.z;

            Vector3 pos;
            pos = new Vector3(xCoord + 1, 0, zCoord);  //North of Tile
            positions[i, 0] = pos;

            pos = new Vector3(xCoord - 1, 0, zCoord);  //South of Tile
            positions[i, 1] = pos;

            pos = new Vector3(xCoord + .5f, 0, zCoord + 1);  //North East of Tile
            positions[i, 2] = pos;

            pos = new Vector3(xCoord + .5f, 0, zCoord - 1);  //North West of Tile
            positions[i , 3] = pos;

            pos = new Vector3(xCoord - .5f, 0, zCoord + 1);  //South East of Tile
            positions[i, 4] = pos;

            pos = new Vector3(xCoord - .5f, 0, zCoord - 1);  //South West of Tile
            positions[i, 5] = pos;
        }

        return positions;
    }

    void SetMoveGrid() {
        if (tilesPlaced > 0) {
            Vector3[,] positions = GetMovePositions();

            for (int i = 0; gamePieces[i] != null; i++) {
                for (int j = 0; j < 6; j++) {
                    GameObject temp;
                    temp = Instantiate(SelectionTile, positions[i, j], Quaternion.identity) as GameObject;
                    selectionGrids.Push(temp);
                    //Debug.Log(temp.transform.position);
                }
            }
        }

        isGridSet = true;
    }
}



/*
    //NE side of GH (x + .5, z + 1)
            Vector3 newPosition = gamePieces[tilesPlaced - 1].transform.pos;
            newPosition = new Vector3(newPosition.x + .5f, 0, newPosition.z - 1);

            Instantiate(tile, newPosition, Quaternion.identity);
            gamePieces[tilesPlaced] = tile;
            tilesPlaced++;
            //Debug.Log(tileScript.GetId());
*/
