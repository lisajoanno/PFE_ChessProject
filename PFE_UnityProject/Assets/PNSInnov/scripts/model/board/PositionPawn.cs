using UnityEngine;
using System;
using System.Collections.Generic;


public class PositionPawn : MonoBehaviour {

	[SerializeField]
	private GameObject pawn;

	[SerializeField]
	private String coordonnees; // "1,2;3,1; ..... "
    [SerializeField]
    [Range(0,5)]
    private int board;
    
	public void PositionOneTypePawn(BoardGenerator plateau, IDictionary<Position, GameObject> chessboard, Board[] boards)
	{
        Board boardWhereInstantiate = boards[board];

		String[] coords = coordonnees.Split (';');
		foreach (String i in coords) {
            String strX = (String) i.Split(',').GetValue(0);
            String strY = (String)i.Split(',').GetValue(1);
            if (strX.Equals("-"))
            {
                int stringY = Convert.ToInt32(strY);
                for (int k = 1; k<=plateau.GetNbCaseLargeur; k++)
                {
                    Position p = new Position(boardWhereInstantiate, new Vector2(k, stringY));

                    CreatePawn(p, chessboard);
                }
            } else if (strY.Equals("-"))
            {
                int stringX = Convert.ToInt32(strX);
                for (int k = 1; k <= plateau.GetNbCaseLongueur; k++)
                {
                    Position p = new Position(boardWhereInstantiate, new Vector2(stringX, k));

                    CreatePawn(p, chessboard);
                }
            } else
            {
                int stringX = Convert.ToInt32 (strX);
			    int stringY = Convert.ToInt32 (strY);

                Position p = new Position(boardWhereInstantiate, new Vector2(stringX, stringY));

                CreatePawn(p, chessboard);
            }
            
		}
	}

    public GameObject CreatePawn(Position p, IDictionary<Position, GameObject> chessboard)
    {
        if (chessboard[p] == null)
        {
            GameObject gObj = (GameObject)Instantiate(pawn);
            gObj.GetComponent<ChessElement>().Position = p;

            PutPawnIntoPosition(gObj);
            gObj.transform.parent = GameObject.FindGameObjectWithTag("pawns").transform;

            chessboard[p] = gObj;
            return gObj;
        } else
        {
            
        }
        return null;
    }
    
	/// <summary>
	///     Put a game object at its right position on the chessboard
	/// </summary>
	/// <param name="pawnToPositionate">the gameobject to positionate</param>
	public void PutPawnIntoPosition(GameObject pawnToPositionate) {
        Position pos = pawnToPositionate.GetComponent<ChessElement>().Position;
        Vector2 position = pos.coo;

        GameObject square = pos.board.GetSquare(position);
		Vector3 p = square.transform.position;
		int x = (int) p.x;
		int y = (int) p.y;
		int z = (int) p.z;

		pawnToPositionate.transform.position = new Vector3(x, y, z);
        pawnToPositionate.transform.up = -square.transform.up; // The orientation is unchanged, must be set in the prefab


        // About the rotation
        PawnElement pawnElement = pawnToPositionate.GetComponent<PawnElement>();
        if (pawnElement != null)
        {
            if (pawnElement.GetTeam != 0)
            {
                pawnToPositionate.transform.Rotate(new Vector3(0, 180, 0));
            }
        }
    } 
}