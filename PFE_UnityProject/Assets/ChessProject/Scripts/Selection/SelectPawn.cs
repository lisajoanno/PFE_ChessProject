using UnityEngine;

/// <summary>
///     A class to permit the selection of a pawn
/// </summary>
public class SelectPawn : Select
{
    //the component managing the teams
    private TeamTurn teamTurn;
    // the color of the squares where the pawn can move
    private Color possibleSquaresColor;







    public SelectPawn(Color color, Color possibleMoveColor, LayerMask mask) : base(color, mask)
    {
        teamTurn = GameObject.FindGameObjectWithTag("GamePlay").GetComponent<TeamTurn>();
        this.possibleSquaresColor = possibleMoveColor;
    }


    /// <summary>
    ///     Do the selection on the newSelected object
    /// </summary>
    /// <param name="newSelected">the gameobject we want to select</param>
    /// <returns>true if we succeed to select something, false otherwise</returns>
    public override bool CanSelect(GameObject newSelected)
    {
        if (base.CanSelect(newSelected))
        {
            Pawn pawn = newSelected.GetComponent<Pawn>();

            // The pawn's team needs to be the current team playing
            return (/**pawn.Team == teamTurn.CurrentTeamPlaying**/ teamTurn.thisTeamCanPlay(pawn.Team));
        }
        return false;
    }
    

    /// <summary>
    /// Color all components children with the select color.
    /// </summary>
    public override void RecolorModel()
    {
        if (!LastSelected) return;
        
        Renderer[] tabChildren = LastSelected.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in tabChildren)
        {
            r.material.color = SelectColor;
        }

        //put a special color on the squares you can move
        Pawn pawn = LastSelected.GetComponent<Pawn>();
        foreach (Position pos in pawn.PossibleMoveCases)
        {
            pos.board.GetSquare(pos.coo).GetComponent<Renderer>().material.color = possibleSquaresColor;
        }
    }

    
    /// <summary>
    /// Resets all the components color at their original color.
    /// </summary>
    public override void ResetColorModel()
    {
        base.ResetColorModel();
        // Also reset possible cases color
        if (LastSelected == null) return;
        Pawn pawnee = LastSelected.GetComponent<Pawn>();
        foreach (Position pos in pawnee.PossibleMoveCases)
        {
            GameObject squareGO = pos.board.GetSquare(pos.coo);
            squareGO.GetComponent<Renderer>().material.color = squareGO.GetComponent<Square>().Color;
        }
    }
}
