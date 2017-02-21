using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script initializing the board and the data model.
/// </summary>
public class BoardInitializer : MonoBehaviour {

    // The prefab of the board, already created
    //public GameObject boardGO;

    [SerializeField]
    private GameObject cubeBoard1;

    [SerializeField]
    private GameObject cubeBoard2;

    [SerializeField]
    private GameObject cubeBoard3;

    [SerializeField]
    private GameObject cubeBoard4;

    [SerializeField]
    private GameObject cubeBoard5;

    [SerializeField]
    private GameObject cubeBoard6;

    // A table of 6 boards
    public Board[] boards;

    private GameObject[] allCubeBoards;

    /// <summary>
    /// Initializes the 6 boards (matrices and squares game objects).
    /// </summary>
    public void Initialize()
    {
        //GameObject cubeBoard1 = (GameObject) GameObject.Instantiate(board1, new Vector3(0, 0, 0), new Quaternion());
        //cubeBoard1.transform.SetParent(GameObject.FindGameObjectWithTag("ImageTarget").gameObject.transform);

        //GameObject cubeBoard2 = (GameObject)GameObject.Instantiate(board2, new Vector3(0, 0, 0), new Quaternion());
        //cubeBoard2.transform.SetParent(GameObject.FindGameObjectWithTag("ImageTarget").gameObject.transform);

        //GameObject cubeBoard3 = (GameObject)GameObject.Instantiate(board3, new Vector3(0, 0, 0), new Quaternion());
        //cubeBoard3.transform.SetParent(GameObject.FindGameObjectWithTag("ImageTarget").gameObject.transform);

        //GameObject cubeBoard4 = (GameObject)GameObject.Instantiate(board4, new Vector3(0, 0, 0), new Quaternion());
        //cubeBoard4.transform.SetParent(GameObject.FindGameObjectWithTag("ImageTarget").gameObject.transform);

        //GameObject cubeBoard5 = (GameObject)GameObject.Instantiate(board5, new Vector3(0, 0, 0), new Quaternion());
        //cubeBoard5.transform.SetParent(GameObject.FindGameObjectWithTag("ImageTarget").gameObject.transform);

        //GameObject cubeBoard6 = (GameObject)GameObject.Instantiate(board6, new Vector3(0, 0, 0), new Quaternion());
        //cubeBoard6.transform.SetParent(GameObject.FindGameObjectWithTag("ImageTarget").gameObject.transform);

        // Initialization of data model : boards
        boards = new Board[6];
        for (int k = 0; k < boards.Length; k++)
        {
            boards[k] = new Board();
        }
        // Initialization of convert matrices
        InitBoardsMatrices(boards);

        allCubeBoards = new GameObject[6];
        allCubeBoards[0] = cubeBoard1;
        allCubeBoards[1] = cubeBoard2;
        allCubeBoards[2] = cubeBoard3;
        allCubeBoards[3] = cubeBoard4;
        allCubeBoards[4] = cubeBoard5;
        allCubeBoards[5] = cubeBoard6;

        IList<Transform> allBoards = new List<Transform>();
        for (int b = 0; b < allCubeBoards.Length; b++)
        {
            // for : ROW
            for (int r = 0; r < cubeBoard1 /*previously board1 */.transform.childCount; r++)
            {
                GameObject boardRow = allCubeBoards[b].transform.GetChild(r).gameObject;

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
                    squareScript.Position = position;
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
    private void InitBoardsMatrices(Board[] boards)
    {

        Passing up_down = new Passing();
        up_down.convertMatrix = new int[2, 2] { {1,0 },
                                                {0,1 } };

        up_down.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(coo.x, 0); };
        boards[0].AddBoard(Direction.UP, boards[1], up_down);
        boards[1].AddBoard(Direction.UP, boards[3], up_down);
        boards[3].AddBoard(Direction.UP, boards[2], up_down);
        boards[2].AddBoard(Direction.UP, boards[0], up_down);

        Passing down_up = new Passing();
        down_up.convertMatrix = new int[2, 2] { {1,0 },
                                                {0,1 } };
        down_up.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(coo.x, rows - 1); };
        boards[0].AddBoard(Direction.DOWN, boards[2], down_up);
        boards[2].AddBoard(Direction.DOWN, boards[3], down_up);
        boards[3].AddBoard(Direction.DOWN, boards[1], down_up);
        boards[1].AddBoard(Direction.DOWN, boards[0], down_up);


        Passing left_left = new Passing();
        left_left.convertMatrix = new int[2, 2] { { -1, 0 },
                                                  { 0, -1 } };
        left_left.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(0, (rows - coo.y)-1); };
        boards[4].AddBoard(Direction.LEFT, boards[3], left_left);
        boards[3].AddBoard(Direction.LEFT, boards[4], left_left);

        Passing right_right = new Passing();
        right_right.convertMatrix = new int[2, 2] { { -1, 0 },
                                                    { 0, -1 } };
        right_right.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(columns - 1, (rows - coo.y)-1); };
        boards[5].AddBoard(Direction.RIGHT, boards[3], right_right);
        boards[3].AddBoard(Direction.RIGHT, boards[5], right_right);


        Passing right_left = new Passing();
        right_left.convertMatrix = new int[2, 2] { { 1, 0 },
                                                   { 0, 1 } };
        right_left.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(0, coo.y); };
        boards[0].AddBoard(Direction.RIGHT, boards[5], right_left);
        boards[4].AddBoard(Direction.RIGHT, boards[0], right_left);

        Passing left_right = new Passing();
        left_right.convertMatrix = new int[2, 2] { { 1, 0 },
                                                   { 0, 1 } };
        left_right.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(columns - 1, coo.y ); };
        boards[0].AddBoard(Direction.LEFT, boards[4], left_right);
        boards[5].AddBoard(Direction.LEFT, boards[0], left_right);

        Passing right_up = new Passing();
        right_up.convertMatrix = new int[2, 2] { { 0, -1 },
                                                 { 1, 0  } };
        right_up.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(coo.y, columns - 1); };
        boards[1].AddBoard(Direction.RIGHT, boards[5], right_up);

        Passing up_right = new Passing();
        up_right.convertMatrix = new int[2, 2] { { 0, 1 },
                                                 { -1, 0  } };
        up_right.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(rows - 1, coo.x); };
        boards[5].AddBoard(Direction.UP, boards[1], up_right);

        Passing left_down = new Passing();
        left_down.convertMatrix = new int[2, 2] { { 0, -1 },
                                                  { 1, 0  } };
        left_down.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(coo.y, 0); };
        boards[2].AddBoard(Direction.LEFT, boards[4], left_down);

        Passing down_left = new Passing();
        down_left.convertMatrix = new int[2, 2] { { 0, 1 },
                                                  { -1, 0 } };
        down_left.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(0, coo.x); };
        boards[4].AddBoard(Direction.DOWN, boards[2], down_left);


        Passing right_down = new Passing();
        right_down.convertMatrix = new int[2, 2] {{ 0, 1 },
                                                  {-1, 0 } };
        right_down.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(rows - coo.y - 1, 0); };
        boards[2].AddBoard(Direction.RIGHT, boards[5], right_down);


        Passing down_right = new Passing();
        down_right.convertMatrix = new int[2, 2] {{ 0, -1 },
                                                  { 1, 0 } };
        down_right.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(columns - 1, (rows - coo.x) - 1); };
        boards[5].AddBoard(Direction.DOWN, boards[2], down_right);


        Passing left_up = new Passing();
        left_up.convertMatrix = new int[2, 2] {{ 0, 1 },
                                               {-1, 0 } };
        left_up.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(columns - coo.y - 1, rows - 1); };
        boards[1].AddBoard(Direction.LEFT, boards[4], left_up);


        Passing up_left = new Passing();
        up_left.convertMatrix = new int[2, 2] {{ 0,-1 },
                                               { 1, 0 } };
        up_left.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(0, (columns - coo.x)-1); };
        boards[4].AddBoard(Direction.UP, boards[1], up_left);
    }

}


