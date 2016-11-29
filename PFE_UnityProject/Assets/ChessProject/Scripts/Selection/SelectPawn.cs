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
    
    public override bool CanSelect(GameObject newSelected)
    {
        if (base.CanSelect(newSelected))
        {
            Pawn pawn = newSelected.GetComponent<Pawn>();
            //return true;

            // The pawn's team needs to be the current team playing
            return (pawn.Team == teamTurn.CurrentTeamPlaying);
        }
        return false;
    }

    protected override void ColorNewlySelectedGameObject()
    {
        //base.ColorNewlySelectedGameObject();

        //set the color of the game object to the specified select color
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

    protected override void ResetColor()
    {
        //we put back the original color of the previously selected game object
        Renderer[] tabChildren = LastSelected.GetComponentsInChildren<Renderer>();
        Pawn pawn = LastSelected.GetComponent<Pawn>();
        foreach (Renderer r in tabChildren)
        {
            r.material.color = pawn.Color;
        }
        
        foreach (Position pos in pawn.PossibleMoveCases)
        {
            GameObject squareGO = pos.board.GetSquare(pos.coo);
            squareGO.GetComponent<Renderer>().material.color = squareGO.GetComponent<Square>().Color;
        }
    }
}
