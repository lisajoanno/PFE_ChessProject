using UnityEngine;
using System.Collections.Generic;

public class PositionPawns : MonoBehaviour {

	[SerializeField]
	private GameObject whitePawn;

	[SerializeField]
	private GameObject blackPawn;

    // List of all PAWNS of the game (by Position, so Board and Vector2)
	private IDictionary<Position, GameObject> positions = new Dictionary<Position, GameObject>();
	public IDictionary<Position, GameObject> GetPositions {
		get {
			return positions;
		}
	}
	
    /// <summary>
    ///     Position all the pawns on the specified board
    /// </summary>
    /// <param name="plateau">The board where the game will take place</param>
	public void PositionSomeTypePawns(BoardGenerator plateau, Board[] boards) {
        InitPositionsOnBoard(boards);

        // Positionner les pions blancs
        PositionPawn[] positionPawn =  whitePawn.GetComponents<PositionPawn>();
        foreach(PositionPawn pos in positionPawn)
        {
            pos.PositionOneTypePawn(plateau, positions, boards);
        }

        // Positionner les pions noirs
        positionPawn = blackPawn.GetComponents<PositionPawn>();
        foreach (PositionPawn pos in positionPawn)
        {
            pos.PositionOneTypePawn(plateau, positions, boards);
        }
    }

    /// <summary>
    ///     Init the board as if there isn't any pawns.
    /// </summary>
    /// <param name="plateau">The board script as in the scene</param>
    private void InitPositionsOnBoard(Board[] boards)
    {
        Position pos;


        foreach (Board board in boards)
        {
            for (int i = 0; i < board.Rows; i++)
            {

                for (int j = 0; j < board.Columns; j++)
                {
                    pos = new Position(board, new Vector2(i + 1, j + 1));
                    positions.Add(pos, null);
                }
            }
        }
    }

    // Checks whether or not there is a pawn at the given postion
    public bool IsThereAPawn(Position pos)
    {
        // TODO 1. regarder si à la position, le go est null ou pas
        /**GameObject go;
        if (positions.TryGetValue(pos, out go))
        {
            if (go != null)
            {
                return true;
            }
        }
        return false;
        Debug.Log(pos.coo + " et " + pos.board);**/

        return (positions[pos] != null);
    }
}
