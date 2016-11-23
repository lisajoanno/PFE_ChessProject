using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script initializing the board and the data model.
/// </summary>
public class BoardInitializer : MonoBehaviour {

    // The prefab of the board, already created
    public GameObject boardGO;

	
	void Start () {
        GameObject cubeBoard = (GameObject) GameObject.Instantiate(boardGO, new Vector3(0,0,0), new Quaternion());

        // Initialization of data model : boards
        Board[] boards = new Board[6];
        for (int k = 0; k < boards.Length; k++)
        {
            boards[k] = new Board();
        }
        // Initialization of convert matrices
        InitBoards(boards);


        IList<Transform> allBoards = new List<Transform>();
        // for : BOARD
        for (int b = 0; b < cubeBoard.transform.childCount; b++)
        {
            allBoards.Add(cubeBoard.transform.GetChild(b));
            GameObject board = cubeBoard.transform.GetChild(b).gameObject;

            // for : ROW
            for (int r = 0; r < board.transform.childCount; r++)
            {
                GameObject boardRow = board.transform.GetChild(r).gameObject;

                // for : CASE
                for (int c = 0; c < boardRow.transform.childCount; c++)
                {
                    GameObject boardCase = boardRow.transform.GetChild(c).gameObject;
                    
                    // TODO: peut etre que l'erreur vient de là 
                    // c, r inversés ?
                    // The current board, column and row.
                    Position position = new Position(boards[b], new Vector2(c, r));
                    boards[b].AddASquare(new Vector2(c, r), boardCase);

                    // Set of the right board in the case 
                    Square squareScript = boardCase.GetComponent<Square>();
                    squareScript.SetBoard(boards[b]);
                }
            }
        }
	}




    /// <summary>
    ///     Initialize the different boards : 
    /// 
    ///         Convertion matrices
    /// </summary>
    /// <param name="boardAbove">the boards we want to init</param>
    private void InitBoards(Board[] boards)
    {

        Passing up_down = new Passing();
        up_down.convertMatrix = new int[2, 2] { {1,0 },
                                                {0,1 } };

        up_down.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(coo.x, 1); };
        boards[0].AddBoard(Direction.UP, boards[3], up_down);
        boards[1].AddBoard(Direction.UP, boards[0], up_down);
        boards[2].AddBoard(Direction.UP, boards[1], up_down);
        boards[3].AddBoard(Direction.UP, boards[2], up_down);

        Passing down_up = new Passing();
        down_up.convertMatrix = new int[2, 2] { {1,0 },
                                                {0,1 } };
        down_up.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(coo.x, rows); };
        boards[0].AddBoard(Direction.DOWN, boards[1], down_up);
        boards[1].AddBoard(Direction.DOWN, boards[2], down_up);
        boards[2].AddBoard(Direction.DOWN, boards[3], down_up);
        boards[3].AddBoard(Direction.DOWN, boards[0], down_up);


        Passing left_left = new Passing();
        left_left.convertMatrix = new int[2, 2] { { -1, 0 },
                                                  { 0, -1 } };
        left_left.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(1, (rows - coo.y) + 1); };
        boards[4].AddBoard(Direction.LEFT, boards[3], left_left);
        boards[3].AddBoard(Direction.LEFT, boards[4], left_left);

        Passing right_right = new Passing();
        right_right.convertMatrix = new int[2, 2] { { -1, 0 },
                                                    { 0, -1 } };
        right_right.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(columns, (rows - coo.y) + 1); };
        boards[5].AddBoard(Direction.RIGHT, boards[3], right_right);
        boards[3].AddBoard(Direction.RIGHT, boards[5], right_right);


        Passing right_left = new Passing();
        right_left.convertMatrix = new int[2, 2] { { 1, 0 },
                                                   { 0, 1 } };
        right_left.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(1, coo.y); };
        boards[1].AddBoard(Direction.RIGHT, boards[5], right_left);
        boards[4].AddBoard(Direction.RIGHT, boards[1], right_left);

        Passing left_right = new Passing();
        left_right.convertMatrix = new int[2, 2] { { 1, 0 },
                                                   { 0, 1 } };
        left_right.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(columns, coo.y); };
        boards[5].AddBoard(Direction.LEFT, boards[1], left_right);
        boards[1].AddBoard(Direction.LEFT, boards[4], left_right);

        Passing right_up = new Passing();
        right_up.convertMatrix = new int[2, 2] { { 0, -1 },
                                                 { 1, 0 } };
        right_up.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(coo.y, columns); };
        boards[0].AddBoard(Direction.RIGHT, boards[5], right_up);

        Passing up_right = new Passing();
        up_right.convertMatrix = new int[2, 2] { { 0, 1 },
                                                 {-1, 0 } };
        up_right.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(rows, coo.x); };
        boards[5].AddBoard(Direction.UP, boards[0], up_right);

        Passing left_down = new Passing();
        left_down.convertMatrix = new int[2, 2] { { 0, -1 },
                                                  { 1, 0 } };
        left_down.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(coo.y, 1); };
        boards[2].AddBoard(Direction.LEFT, boards[4], left_down);

        Passing down_left = new Passing();
        down_left.convertMatrix = new int[2, 2] { { 0, 1 },
                                                  { -1, 0 } };
        down_left.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(1, coo.x); };
        boards[4].AddBoard(Direction.DOWN, boards[2], down_left);


        Passing right_down = new Passing();
        right_down.convertMatrix = new int[2, 2] {{ 0, 1 },
                                                  {-1, 0 } };
        right_down.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(1 + rows - coo.y, 1); };
        boards[2].AddBoard(Direction.RIGHT, boards[5], right_down);


        Passing down_right = new Passing();
        down_right.convertMatrix = new int[2, 2] {{ 0, -1 },
                                                  { 1, 0 } };
        down_right.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(columns, (rows - coo.x) + 1); };
        boards[5].AddBoard(Direction.DOWN, boards[2], down_right);


        Passing left_up = new Passing();
        left_up.convertMatrix = new int[2, 2] {{ 0, 1 },
                                               {-1, 0 } };
        left_up.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(1 + columns - coo.y, rows); };
        boards[0].AddBoard(Direction.LEFT, boards[4], left_up);


        Passing up_left = new Passing();
        up_left.convertMatrix = new int[2, 2] {{ 0,-1 },
                                               { 1, 0 } };
        up_left.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(1, (columns - coo.x) + 1); };
        boards[4].AddBoard(Direction.UP, boards[0], up_left);
    }

}


