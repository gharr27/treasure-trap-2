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
    //GameObject For Creating the Move Grid
    public GameObject GridTile;
    
    //Boolean that determines if a piece has been selected to move
    public bool isSelectionMade = false;

    //Constant of how many total pieces are in the game
    const int PIECE_COUNT = 22;

    //Keeps track of how many tiles are currently on the board
    private int tilesPlaced = 0;
    //Stores each GameObject on the board
    private GameObject[] gamePieces;

    //Stores the selected piece to place
    private GameObject selectedPiece;
    //Holds the selection grid objects
    private Stack<GameObject> selectionGrids;

    //Value of tile that is selected, used to select proper gameobject piece
    private int pieceSelection;

    //Keeps track of when pieces are selected to place
    private bool isPieceSelected = false;

    //Keeps track of when pieces are selected to move
    private bool isMovePiece = false;

    //Keeps track of the move grid has been created
    private bool isGridSet = false;

    //Keeps track of if there is a game over state
    private bool isWin = false;


    //Vector storing the position the player wishing to move/place a piece at
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
        if (isWin)
        {

            //Placing A Piece==================================
            //Checks if a piece has been selecte to place
            IsPieceSelected();

            //Waits until the player has selected a piece to place on the board
            if (isPieceSelected)
            {

                //Checks if it is the first tile placed on board
                //If it is, the piece is placed at position 0,0
                if (tilesPlaced == 0)
                {
                    PlacePiece(pieceSelection, pos);
                    isPieceSelected = false;
                }
                //If it is not first turn, wait until player selects where they want to place the piece
                else
                {
                    //Creates the grid showing valid placement positions
                    if (!isGridSet)
                    {
                        SetMoveGrid();
                    }

                    //Places piece at selected position, resets selection values to false to wait for next turn
                    if (isSelectionMade)
                    {
                        PlacePiece(pieceSelection, pos);
                        isWin = CheckForWin();
                        isPieceSelected = false;
                        isGridSet = false;
                        isSelectionMade = false;
                    }
                }
            }
            //================================================

            //Moving A Piece==================================
            //Waits until the player has selected a piece to move
            if (isMovePiece)
            {
                isPieceSelected = false;

                //Creates the grid showing valid movement positions
                if (!isGridSet)
                {
                    SetMoveGrid();
                }

                //Moves pieces to selected position, resets selection values to false to wait for next turn
                if (isSelectionMade)
                {
                    MovePiece(selectedPiece, pos);
                    isWin = CheckForWin();
                    isMovePiece = false;
                    isGridSet = false;
                    isSelectionMade = false;
                }
            }
            //===============================================
        }
    }

    bool CheckForWin()
    {
        bool ret = false;

        //Check if Queen is surrounded

        return ret;
    }

    //Sets the selected position for a piece to be placed
    public void SetPosition(Vector3 position) {
        pos = position;
        isSelectionMade = true;
    }

    //Selects the game object of the tile the player wants to move
    public void SetSelectedPiece(GameObject piece) {
        selectedPiece = piece;
        isMovePiece = true;
    }

    //Determines what kind of tile the player wishes to place
    void IsPieceSelected() {
        //Selected Queen
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            pieceSelection = 0;
            isPieceSelected = true;
        }
        //Selected Ant
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            pieceSelection = 1;
            isPieceSelected = true;
        }
        //Selected Grasshopper
        else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            pieceSelection = 2;
            isPieceSelected = true;
        }
        //Selected Beetle
        else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            pieceSelection = 3;
            isPieceSelected = true;
        }
        //Selected Spider
        else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            pieceSelection = 4;
            isPieceSelected = true;
        }
    }

    //Places the given piece at the given position
    void PlacePiece(int piece, Vector3 pos) {
        GameObject tile = TilePieces[piece];

        gamePieces[tilesPlaced] = Instantiate(tile, pos, Quaternion.identity) as GameObject;
        tilesPlaced++;
        
        //Deletes the selection grid after piece has been placed
        while(selectionGrids.Count != 0) {
            GameObject temp = selectionGrids.Pop();
            Destroy(temp);
        }
    }

    //Moves the given piece to the given position
    void MovePiece(GameObject piece, Vector3 pos) {
        piece.transform.position = pos;
        
        //Deletes the selection grid after piece has been moved
        while(selectionGrids.Count != 0) {
            GameObject temp = selectionGrids.Pop();
            Destroy(temp);
        }
    }

    //Returns a vector of all the valid movement/placement positions
    Vector3[,] GetMovePositions() {
        Vector3[,] positions = new Vector3[tilesPlaced, 6];

        //Checks for the open positions around a tile for creating potential move positions
        for (int i = 0; gamePieces[i] != null; i++) {
            float xCoord = gamePieces[i].transform.position.x;
            float zCoord = gamePieces[i].transform.position.z;

            Vector3 pos;

            //North of Tile
            pos = new Vector3(xCoord + 1, 0, zCoord);  
            positions[i, 0] = pos;

            //South of Tile
            pos = new Vector3(xCoord - 1, 0, zCoord);  
            positions[i, 1] = pos;

            //North East of Tile
            pos = new Vector3(xCoord + .5f, 0, zCoord + 1);  
            positions[i, 2] = pos;

            //North West of Tile
            pos = new Vector3(xCoord + .5f, 0, zCoord - 1);  
            positions[i , 3] = pos;

            //South East of Tile
            pos = new Vector3(xCoord - .5f, 0, zCoord + 1);  
            positions[i, 4] = pos;

            //South West of Tile
            pos = new Vector3(xCoord - .5f, 0, zCoord - 1);  
            positions[i, 5] = pos;
        }

        return positions;
    }

    //Creates the selection grid at valid move positions
    void SetMoveGrid() {
        if (tilesPlaced > 0) {
            Vector3[,] positions = GetMovePositions();

            for (int i = 0; gamePieces[i] != null; i++) {
                for (int j = 0; j < 6; j++) {
                    GameObject temp;
                    temp = Instantiate(GridTile, positions[i, j], Quaternion.identity) as GameObject;
                    selectionGrids.Push(temp);
                    //Debug.Log(temp.transform.position);
                }
            }
        }

        isGridSet = true;
    }
}
