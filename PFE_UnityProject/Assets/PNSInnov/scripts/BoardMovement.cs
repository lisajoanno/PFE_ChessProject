using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Component that executes the movements
///     of the pawns on the chess board
/// </summary>
[RequireComponent(typeof(PositionPawns))]
public class BoardMovement : MonoBehaviour {

    [SerializeField]
    private PositionPawn positionPawn;

    private EatPawn eat;                    //the component that permits a pawn to eat another one
    private PositionPawns positionPawns;    //the component that manage the position of the pawns on the chessboard

    /// <summary>
    ///     Initialize the controller of the selection
    /// </summary>
    void Awake()
    {
        eat = GetComponent<EatPawn>();
        positionPawns = GetComponent<PositionPawns>();
    }

    
    /// <summary>
    /// Move a pawn from its square to the other square
    /// </summary>
    /// <param name="pawn">the pawn we want to move</param>
    /// <param name="square">the square where we want to move the pawn</param>
    public bool Move(PawnElement pawn, ChessElement square)
    {
        // It is a possible move for this Pawn Element
		if (pawn.MoveCases.Contains (square.Position)) {

            // Check if there is a pawn at the destination => if yes, we can only eat it (if it's on a different team)
            if (positionPawns.GetPositions[square.Position] != null)
            {
                PawnElement pe = positionPawns.GetPositions[square.Position].GetComponent<PawnElement>();
                // We can only eat if the teams are different
                if (pe.GetTeam != pawn.GetTeam)
                {
                    eat.Eat(positionPawns.GetPositions, square.Position);
                    // The pawn is eaten but the actual movement is yet to be done
                } else
                {
                    // the pawn cannot be eaten => the movement can't be finished
                    return false;
                }
			} 
            positionPawns.GetPositions[square.Position] = pawn.gameObject;
            positionPawns.GetPositions[pawn.Position] = null;
            //change the position that the pawn knows
            pawn.Position = square.Position;
            positionPawn.PutPawnIntoPosition(pawn.gameObject);
            pawn.UpdateSelectableCases();
            return true;
        }
        return false;
        
    }
}