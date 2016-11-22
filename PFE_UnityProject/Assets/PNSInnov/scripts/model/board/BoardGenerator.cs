using UnityEngine;
using System.Collections.Generic;
using System;

public class BoardGenerator : MonoBehaviour {
    [SerializeField]
    private int tailleCase;

    [SerializeField]
    private GameObject caseNoire;

    [SerializeField]
    private GameObject caseBlanche;

    private int nbCaseLargeur;
	public int GetNbCaseLargeur
	{
		get
		{
			return nbCaseLargeur;
		}
	}

    private int nbCaseLongueur;
	public int GetNbCaseLongueur
	{
		get
		{
			return nbCaseLongueur;
		}
	}

    private Dictionary<Vector3, GameObject> coordonnees = new Dictionary<Vector3, GameObject>();
    public Dictionary<Vector3, GameObject> Coordonnees
    {
        get
        {
            return coordonnees;
        }
    }

	public void initSize(int s) {
		nbCaseLargeur = s;
		nbCaseLongueur = s;
	}

    // Use this for initialization
    public void DessinerEchiquier (Board[] boards) {
        //link the two boards together
        InitBoards(boards);
         
        for (int i = 0; i < nbCaseLargeur; i++)
        {
            for (int j = 0; j < nbCaseLongueur; j++)
            {
                //GameObject caseHaut;
                //GameObject caseBas;
                Position posUp = new Position(boards[0], new Vector2(i+1, j+1));
                Position posFront = new Position(boards[1], new Vector2(i + 1, j + 1));
                Position posDown = new Position(boards[2], new Vector2(i+1, j+1));
                Position posBehind = new Position(boards[3], new Vector2(i + 1, j + 1));
                Position posLeft = new Position(boards[4], new Vector2(i + 1, j + 1));
                Position posRight = new Position(boards[5], new Vector2(i + 1, j + 1));

                //
                GameObject plaque = caseNoire;
                //TODO faire du quaternion
                Quaternion rotationHaut = Quaternion.Euler(180, 0, 0);
                Quaternion rotationBas = Quaternion.Euler(0, 0, 0);
                Quaternion rotationGauche = Quaternion.Euler(0, 180, 90);
                Quaternion rotationDroite = Quaternion.Euler(0, 180, -90);
                Quaternion rotationAvant = Quaternion.Euler(90, 180, 180);
                Quaternion rotationArriere = Quaternion.Euler(-90, 0, 0);
                //cases blanches
                if ((i + j) % 2 == 0)
                {
                    plaque = caseBlanche;
                }
                //up
                Vector3 position = new Vector3((int)((0.5+ i - nbCaseLargeur*0.5) * tailleCase), (int)((nbCaseLargeur*0.5) * tailleCase), (int)((j - nbCaseLongueur*0.5) * tailleCase));
                InstantiateSquares(plaque, position, rotationHaut, posUp, i, j, boards[0]);
                //front
                position = new Vector3((int) ((0.5 + i - nbCaseLargeur*0.5)*tailleCase), (int)((0.5+(j - nbCaseLongueur*0.5)) * tailleCase), (int)((-0.5-(nbCaseLargeur*0.5)) * tailleCase));
                InstantiateSquares(plaque, position, rotationAvant, posFront, i, j, boards[1]);
                //down
                position = new Vector3((int)((0.5 + i - nbCaseLargeur*0.5) * tailleCase), (int)((-nbCaseLargeur*0.5) * tailleCase), (int)(((nbCaseLongueur*0.5) - j - 1) * tailleCase));
                InstantiateSquares(plaque, position, rotationBas, posDown, i, j, boards[2]);
                //behind
                position = new Vector3((int)((0.5 + i - nbCaseLargeur*0.5) * tailleCase), (int)((0.5 + (nbCaseLongueur*0.5) - j - 1) * tailleCase), (int)((-0.5 + (nbCaseLargeur*0.5)) * tailleCase));
                InstantiateSquares(plaque, position, rotationArriere, posBehind, i, j, boards[3]);
                //left
                position = new Vector3((int)((- (nbCaseLargeur*0.5)) * tailleCase),(int)((0.5 + j - nbCaseLargeur*0.5) * tailleCase), (int)(((nbCaseLongueur*0.5) - i - 1) * tailleCase));
                InstantiateSquares(plaque, position, rotationGauche, posLeft, i, j, boards[4]);
                //right
                position = new Vector3((int)((nbCaseLargeur*0.5) * tailleCase), (int)((0.5 + j - nbCaseLargeur*0.5) * tailleCase), (int)(((i - nbCaseLongueur*0.5) * tailleCase)));
                InstantiateSquares(plaque, position, rotationDroite, posRight, i, j, boards[5]);
                
            }
        }
    }

    private void InstantiateSquares(GameObject plaque, Vector3 position,Quaternion rot, Position pos, int row, int col, Board board)
    {
        GameObject square = (GameObject)Instantiate(plaque, position, rot);
        coordonnees.Add(position, square);
        square.GetComponent<ChessElement>().Position = pos;

        square.transform.parent = GameObject.FindGameObjectWithTag("board").transform;
        board.AddASquare(new Vector2(row + 1, col + 1), square);
        /*
        GameObject g = new GameObject();
        g.AddComponent<TextMesh>();
        g.transform.position = position + new Vector3(0, tailleCase / 2, 0);
        g.GetComponent<TextMesh>().fontSize = 200;
        g.GetComponent<TextMesh>().color = Color.red;
        g.GetComponent<TextMesh>().transform.rotation *= Quaternion.Euler(Vector3.right * 90);
        g.GetComponent<TextMesh>().text = "" + ((char)((char)'A' + (row))) + "," + (col + 1);
        g.transform.parent = GameObject.FindGameObjectWithTag("board").transform;*/
    }

    /// <summary>
    ///     Initialize the different boards
    /// </summary>
    /// <param name="boardAbove">the boards we want to init</param>
    private void InitBoards(Board[]boards)
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
        left_left.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(1, (rows-coo.y)+1); };
        boards[4].AddBoard(Direction.LEFT, boards[3], left_left);
        boards[3].AddBoard(Direction.LEFT, boards[4], left_left);

        Passing right_right = new Passing();
        right_right.convertMatrix = new int[2, 2] { { -1, 0 },
                                                    { 0, -1 } };
        right_right.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(columns, (rows - coo.y)+1); };
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
        right_down.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(1+rows-coo.y, 1); };
        boards[2].AddBoard(Direction.RIGHT, boards[5], right_down);
        

        Passing down_right = new Passing();
        down_right.convertMatrix = new int[2, 2] {{ 0, -1 },
                                                  { 1, 0 } };
        down_right.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(columns, (rows-coo.x)+1); };
        boards[5].AddBoard(Direction.DOWN, boards[2], down_right);


        Passing left_up = new Passing();
        left_up.convertMatrix = new int[2, 2] {{ 0, 1 },
                                               {-1, 0 } };
        left_up.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(1+columns-coo.y, rows); };
        boards[0].AddBoard(Direction.LEFT, boards[4], left_up);


        Passing up_left = new Passing();
        up_left.convertMatrix = new int[2, 2] {{ 0,-1 },
                                               { 1, 0 } };
        up_left.newBeginning = (Vector2 coo, int rows, int columns) => { return new Vector2(1, (columns - coo.x) + 1); };
        boards[4].AddBoard(Direction.UP, boards[0], up_left);
    }
}
