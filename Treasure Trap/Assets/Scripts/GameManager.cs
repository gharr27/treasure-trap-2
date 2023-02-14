using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private class GameGridCell {
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
        
        MovePosition(bool isFilled, Vector3 pos) {
            this.isFilled = isFilled;
            this.pos = pos;
        }
    }

    //Contains Grid Prefab for creating MoveGrid
    public GameObject GridTile;

    const int PIECE_COUNT = 22;

    private int tilesPlaced = 0;

    private GameObject[] gamePieces;
    private Stack<GameObject> selectionGrids;

    bool isPlaying = false;
    bool isWin = false;

    int turnCounter = 0;

    Player player;
    GameObject playerObject;

    Dictionary<Vector3, GameGridCell> gameGrid = new Dictionary<Vector3, GameGridCell>();

    // Start is called before the first frame update
    void Start()
    {
        selectionGrids = new Stack<GameObject>();
        gamePieces = new GameObject[PIECE_COUNT];

        playerObject = GameObject.FindWithTag("Player");
        player = playerObject.GetComponent(typeof(Player)) as Player;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWin) {
            if (!isPlaying) {
                StartCoroutine(player.Move(turnCounter));
                isPlaying = true;
                if (turnCounter == 0) {
                    turnCounter++;
                }
                else {
                    turnCounter--;
                }
            }
        }
        else {
            Debug.Log("Game Over");
        }


        if(Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log(gameGrid.Count);
        }
    }

    public void MakeMove(GameObject tile, Vector3 pos, bool isMove) {
        if (isMove) {
            tile.transform.position = pos;
            UpdateGameGrid(pos);
        }
        else {
            gamePieces[tilesPlaced] = Instantiate(tile, pos, Quaternion.identity) as GameObject;
            GameGridCell gridCell = new GameGridCell(true, tile);
            
            if(gameGrid.ContainsKey(pos)) {
                gameGrid[pos] = gridCell;
            }
            else {
                gameGrid.Add(pos, gridCell);
            }
            tilesPlaced++;

            UpdateGameGrid(pos);
        }

        ClearMoveGrid();
        CheckForWin();

        isPlaying = false;

    }

    void UpdateGameGrid(Vector3 pos) {
            gameGrid.Remove(tile.transform.position);
            tile.transform.position = pos;
            
            GameGridCell gridCell = new GameGridCell(true, tile);
          
            if (!gameGrid.ContainsKey(pos)) {
                gameGrid.Add(pos, gridCell);
            }
            else {
                gameGrid[pos] = gridCell;
            }
            
            UpdateGameGrid(pos);
        }
        else {
            GameObject tilePiece = Instantiate(tile, pos, Quaternion.identity) as GameObject;
            gamePieces[tilesPlaced] = tilePiece;
            GameGridCell gridCell = new GameGridCell(true, tilePiece);
            
            if(gameGrid.ContainsKey(pos)) {
                gameGrid[pos] = gridCell;
            }
            else {
                gameGrid.Add(pos, gridCell);
            }
            tilesPlaced++;

            UpdateGameGrid(pos);
        }
        
        ClearMoveGrid();
        isWin = CheckForWin();
        
        isPlaying = false;
   
    // Checks the surrounding of the selected tile and if the position is filled then it adds a true to a list at that postion. 
    void CheckSurrounding()
    {
        float x = pos.x;
        float y = pos.y;
        float z = pos.z;

        GameGridCell gridCell = new GameGridCell();

        //Above
        Vector3 newPos = new Vector3(x + 1, y, z);
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
        Debug.Log(gameGrid.Count);

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
    Vector3[] ValidateMoves(GameObject tile)
    {
        Vector3 pos = tile.transform.position;

        float x = pos.x;
        float y = pos.y;
        float z = pos.z;


        Vector3[,] movePositions = GetMovePositions();
        Vector3<MovePosition>[6] sides;
        Vector3 storedPositions;

        for (int i = 1; i < movePositions.Length(); i++)
        {
            storedPositions = movePositions[i];

            //Above
            Vector3 newPos = new Vector3(x + 1, y, z);
            if (gameGrid[newPos].isFilled)
            {
                sides.push(MovePosition(true, newPos));
            }
            else
            {
                sides.push(MovePosition(false, newPos));
            }


            //Below
            newPos = new Vector3(x - 1, y, z);
            if (gameGrid[newPos].isFilled)
            {
                sides.push(MovePosition(true, newPos));
            }
            else
            {
                sides.push(MovePosition(false, newPos));
            }


            //Top Left
            newPos = new Vector3(x + .5f, y, z + 1);
            if (gameGrid[newPos].isFilled)
            {
                sides.push(MovePosition(true, newPos));
            }
            else
            {
                sides.push(MovePosition(false, newPos));
            }


            //Bottom Left
            newPos = new Vector3(x - .5f, y, z + 1);
            if (gameGrid[newPos].isFilled)
            {
                sides.push(MovePosition(true, newPos));
            }
            else
            {
                sides.push(MovePosition(false, newPos));
            }


            //Top Right
            newPos = new Vector3(x + .5f, y, z - 1);
            if (gameGrid[newPos].isFilled)
            {
                sides.push(MovePosition(true, newPos));
            }
            else
            {
                sides.push(MovePosition(false, newPos));
            }


            //Bottom Right
            newPos = new Vector3(x - .5f, y, z - 1);
            if (gameGrid[newPos].isFilled)
            {
                sides.push(MovePosition(true, newPos));
            }
            else
            {
                sides.push(MovePosition(false, newPos));
            }




            //if (sides[i].isFilled)
            //{
            //    returnVector.push(sides[i].pos)
            //}

        }
    }

    /* Shows available moves for the queen*/
    Vector3[] QueenPossibleMoves(Vector3<MovePosition>[6] sides)
    {
        Vector3[] validMovePositions;
        int i = 0;
        trueCount = 0;
        falseCount = 0;
        for(int i =0; i< sides.Length; i++){
            if(i=0){
                validMovePositions.push(sides.Length-1);
                validMovePositions.push(sides[i]+1);
            }
            else if(i= sides.Length - 1) {
                validMovePositions.push(sides[0]);
                validMovePositions.push(i - 1);
            } 

      
        }


        if (trueCount =1 && trueCount<5 && falseCount ==0) /*one or more positons is true in a row and the rest are false*/
        {
            /*create selection grid tiles on the first instance of false before and after*/
        }
        else if (/*a true followed by two falses and then a true and there are a total of three trues*/)
        {
            /*fill the positons where the two falses in a row are*/
        }
        else if (/*a true followed by one false and then true and there are a total of three trues*/)
        {
            /*fill the positons where the two falses in a row are*/
        }

    }
    
    void UpdateGameGrid(Vector3 pos) {
        float x = pos.x;
        float y = pos.y;
        float z = pos.z;

        GameGridCell gridCell = new GameGridCell();

    void CheckForWin()
    {
        Debug.Log(1);
        Vector3 test = new Vector3(0, 0, 0);
        TileScript tileScript =  gameGrid[test].tile.GetComponent(typeof(TileScript)) as TileScript;

        Debug.Log(tileScript.GetId());
        //Check if Queen is surrounded
        if(tileScript.GetId() == "Queen1") {
            Debug.Log(2);
            isWin = isSurrounded(test);
        }
    }

    bool isSurrounded(Vector3 pos) {
        float x = pos.x;
        float y = pos.y;
        float z = pos.z;

        //Above
        Vector3 newPos = new Vector3(x + 1, y, z);
        if (!gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                Debug.Log("test");
                return false;
            }
        }

        //Below
        newPos = new Vector3(x - 1, y, z);
        if (!gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                return false;

        //Above
        Vector3 newPos = new Vector3(x + 1, y, z);
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
        Debug.Log(gameGrid.Count);

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

        //Top Left
        newPos = new Vector3(x + .5f, y, z + 1);
        if (!gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                return false;
            }
        }

        //Top Right
        newPos = new Vector3(x + .5f, y, z - 1);
        if (!gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                return false;
            }
        }

        //Bottom Left
        newPos = new Vector3(x - .5f, y, z + 1);
        if (!gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                return false;
            }
        }

        //Bottom Right
        newPos = new Vector3(x - .5f, y, z - 1);
        if (!gameGrid.ContainsKey(newPos)) {
            if (!gameGrid[newPos].isFilled) {
                return false;
            }
        }

        return true;
    }

    bool CheckForWin() {

        foreach (GameGridCell tile in gameGrid.Values) {
            if (tile.isFilled) {
                Vector3 pos = tile.tile.transform.position;

                TileScript tileScript = tile.tile.GetComponent(typeof(TileScript)) as TileScript;

                //Check if Queen is surrounded
                if (tileScript.GetId() == "Queen1" || tileScript.GetId() == "Queen2") {
                    Debug.Log(tileScript.GetId());
                    Debug.Log(pos);
                    if(IsSurrounded(pos)) {
                        return true;
                    }
                    
                }
            }
        }
        return false;
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
    public void SetMoveGrid() {
        ClearMoveGrid();

        Vector3[] validPos;


        if (tilesPlaced > 0) {
            Vector3[,] positions = GetMovePositions();

            for (int i = 0; gamePieces[i] != null; i++) {
                for (int j = 0; j < 6; j++) {
                    GameObject temp;
                    temp = Instantiate(GridTile, positions[i, j], Quaternion.identity) as GameObject;
                    selectionGrids.Push(temp);
                }
            }
        }
    }

    //Deletes the selection grid after piece has been placed
    void ClearMoveGrid() {
        while(selectionGrids.Count != 0) {
            GameObject temp = selectionGrids.Pop();
            Destroy(temp);
        }
    }
}
